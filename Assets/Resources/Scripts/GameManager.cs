using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance, new game managers will override old ones
    public Hittable Castle => _castle;
    [HideInInspector] public bool[] UnlockedSpells;
    public PhaseEnum CurrentPhase => _currentPhase;
    public AllyUnitPrice[] UnitPrices => _unitPrices;
    [SerializeField] private Cemetery _castle;
    [SerializeField] private int _initialZombies = 5;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private float _tileSize = 10f;
    [SerializeField] private int _mapWidth = 10;
    [SerializeField] private int _mapHeight = 10;
    [SerializeField] Round[] _rounds;
    [SerializeField] Round _freePlay;
    [SerializeField] float _freePlayEnemyIncreaseRate = 1.1f; // Percentage increase of enemies per round in free play mode
    [SerializeField] int _maxBlood = 20;
    [SerializeField] AllyUnit _zombiePrefab;
    [SerializeField] Transform _enemySpawnPoint;
    [SerializeField] Vector2 _enemySpawnSize = new Vector2(40f, 20f);
    [SerializeField] AllyUnitPrice[] _unitPrices;

    List<AllyUnit> _alliedUnits = new List<AllyUnit>();
    List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();
    List<BaseTower> _towers = new List<BaseTower>();
    List<Cemetery> _cemeteries = new List<Cemetery>();
    int _currentRound = 0;
    int _blood = 0;
    PhaseEnum _currentPhase = PhaseEnum.Build;
    Tile[] _tiles;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this && Instance.gameObject != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }
        UnlockedSpells = new bool[1];
        _towers.Add(_castle);
        _cemeteries.Add(_castle);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (GetBodies() >= 2)
            {
                RemoveBodies(2);
            }
        }
        if (_currentPhase == PhaseEnum.Build)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StartRoundCoroutine());
            }
        }
        else if (_currentPhase == PhaseEnum.Combat)
        {
            if (_enemyUnits.Count == 0)
            {
                EndRound();
            }
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        _tiles = new Tile[_mapWidth * _mapHeight];
        GameObject treePrefab = Resources.Load<GameObject>("Prefabs/Tree");
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                Tile newTile = Instantiate(_tilePrefab, new Vector3(x * _tileSize, 0, y * _tileSize), Quaternion.identity);
                if (x <= 5 || y <= 4 || x >= 9 || y >= _mapHeight - 7)
                {
                    if (y <= 4 || y >= _mapHeight - 7 || x <= 2)
                    {
                        Instantiate(treePrefab, new Vector3(x * _tileSize, 0, y * _tileSize), Quaternion.identity, newTile.transform);
                    }
                    if (x <= 5 && (y <= 7 || y >= _mapHeight - 12))
                    {
                        Instantiate(treePrefab, new Vector3(x * _tileSize, 0, y * _tileSize), Quaternion.identity, newTile.transform);
                    }
                    newTile.InitializeTile(Tile.TileState.Battlefield);
                }
                else if (y <= 7 || y >= _mapHeight - 12)
                {
                    GameObject tree = Instantiate(treePrefab, new Vector3(x * _tileSize, 0, y * _tileSize), Quaternion.identity, newTile.transform);
                    newTile.InitializeTile(Tile.TileState.Occupied, tree);
                }
                else newTile.InitializeTile(Tile.TileState.Buildable);
                _tiles[x + y * _mapWidth] = newTile;
            }
        }
        StartCoroutine(MapSpawnAnimation());
    }

    /// <summary>
    /// Prepares the next round by clearing existing enemies and spawning new ones based on defined probabilities.
    /// </summary>
    void PrepareRound()
    {
        Round round;
        if (_currentRound < _rounds.Length)
        {
            round = _rounds[_currentRound];
        }
        else
        {
            round = _freePlay;
            round.TotalRandomEnemies = Mathf.RoundToInt(round.TotalRandomEnemies * Mathf.Pow(_freePlayEnemyIncreaseRate, _currentRound - _rounds.Length + 1)); // Increase enemies in free play mode
        }
        for (int i = 0; i < round.TotalRandomEnemies; i++)
        {
            float rand = Random.Range(0f, 1f);
            float cumulativeProbability = 0f;
            foreach (RandomRoundEnemy roundEnemy in round.RandomEnemiesInRound)
            {
                cumulativeProbability += roundEnemy.Probability;
                if (rand <= cumulativeProbability)
                {
                    _enemyUnits.Add(Instantiate(
                        roundEnemy.enemyUnit,
                        _enemySpawnPoint.position + new Vector3(Random.Range(-_enemySpawnSize.x, _enemySpawnSize.x), 0, Random.Range(-_enemySpawnSize.y, _enemySpawnSize.y)),
                        Quaternion.identity
                    ));
                    break;
                }
            }
        }
        foreach (FixedRoundEnemy fixedEnemy in round.FixedEnemiesInRound)
        {
            for (int j = 0; j < fixedEnemy.amount; j++)
            {
                Debug.Log("Spawning fixed enemy: " + fixedEnemy.enemyUnit.name);
                _enemyUnits.Add(Instantiate(
                    fixedEnemy.enemyUnit,
                    _enemySpawnPoint.position + new Vector3(Random.Range(-_enemySpawnSize.x, _enemySpawnSize.x), 0, Random.Range(-_enemySpawnSize.y, _enemySpawnSize.y)),
                    Quaternion.identity
                ));
            }
        }
        _currentPhase = PhaseEnum.Combat;
    }

    /// <summary>
    /// Starts the round by unpausing all units.
    /// </summary>
    void StartRound()
    {
        foreach (BaseUnit unit in _enemyUnits)
        {
            unit.Unpause();
        }
        foreach (BaseUnit unit in _alliedUnits)
        {
            unit.Unpause();
        }
    }

    /// <summary>
    /// Ends the round by rewarding the player and cleaning up units.
    /// </summary>
    void EndRound()
    {
        foreach (EnemyUnit unit in _enemyUnits)
        {
            if (!unit.Dead)
            {
                Debug.Log("Some enemies are still alive, cannot end round.");
                return;
            }
        }

        foreach (AllyUnit unit in _alliedUnits)
        {
            unit.Pause();
            unit.Reset();
            if (unit.Dead)
            {
                Destroy(unit.gameObject);
            }
        }
        _alliedUnits.RemoveAll(unit => unit.Dead);

        foreach (EnemyUnit unit in _enemyUnits)
        {
            for(int i = 0; i < unit.BodyReward; i++)
            {
                AddBody();
            }
            _blood += unit.BloodReward;
            if (_blood > _maxBlood) _blood = _maxBlood;
            Destroy(unit.gameObject);
        }
        _enemyUnits.Clear();

        _currentRound++;
        if (_currentRound < _rounds.Length)
        {
            Debug.Log("All rounds completed!");
        }
        _currentPhase = PhaseEnum.Build;
    }

    /// <summary>
    /// Returns the closest allied unit to the given position. If no allied units exist, returns the closest tower. If no towers exist, returns the castle.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Hittable GetClosestAllyUnit(Vector3 position, Hittable exclude = null)
    {
        if (_alliedUnits.Count == 0) return GetClosestTower(position); // If no allied units, return closest tower

        Hittable closestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit.Dead || unit == exclude) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr < closestDistance && unit)
            {
                closestDistance = distanceSqr;
                closestUnit = unit;
            }
        }

        if (closestUnit == null) return GetClosestTower(position); // If all allied units are dead, return closest tower

        return closestUnit;
    }

    /// <summary>
    /// Returns the closest enemy unit to the given position. If no enemies exist, ends the round and returns null.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Hittable GetClosestEnemy(Vector3 position, Hittable exclude = null)
    {
        if (_enemyUnits.Count == 0)
        {
            EndRound();
            return null;
        }

        Hittable closestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseUnit unit in _enemyUnits)
        {
            if (unit.Dead || unit == exclude) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr < closestDistance)
            {
                closestDistance = distanceSqr;
                closestUnit = unit;
            }
        }

        if (closestUnit == null)
        {
            EndRound();
            return null;
        }

        return closestUnit;
    }

    /// <summary>
    /// Returns the closest tower to the given position. If no towers exist, returns the castle.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Hittable GetClosestTower(Vector3 position, Hittable exclude = null)
    {
        if (_towers.Count == 0) return _castle; // If no towers, return castle

        Hittable closestTower = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseTower tower in _towers)
        {
            if (tower == exclude) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - tower.transform.position);
            if (distanceSqr < closestDistance)
            {
                closestDistance = distanceSqr;
                closestTower = tower;
            }
        }

        if (closestTower == null) return _castle; // If all towers are destroyed, return castle

        return closestTower;
    }

    /// <summary>
    /// Returns the allied unit with the highest max health. If no allied units exist, returns the closest tower.
    /// </summary>
    public Hittable GetHighestHealthAllyUnit(Vector3 position, Hittable exclude = null)
    {
        if (_alliedUnits.Count == 0) return GetClosestTower(position, exclude);

        Hittable highestHealthUnit = null;
        float highestHealth = -Mathf.Infinity;
        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit.Dead || unit == exclude) continue;

            if (unit.MaxHealth > highestHealth)
            {
                highestHealth = unit.MaxHealth;
                highestHealthUnit = unit;
            }
        }

        if (highestHealthUnit == null) return GetClosestTower(position);

        return highestHealthUnit;
    }

    /// <summary>
    /// Returns the enemy unit with the highest max health. If no enemies exist, ends the round and returns null.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Hittable GetHighestHealthEnemy(Hittable exclude = null)
    {
        if (_enemyUnits.Count == 0)
        {
            EndRound();
            return null;
        }

        Hittable highestHealthUnit = null;
        float highestHealth = -Mathf.Infinity;
        foreach (BaseUnit unit in _enemyUnits)
        {
            if (unit.Dead || unit == exclude) continue;

            if (unit.MaxHealth > highestHealth)
            {
                highestHealth = unit.MaxHealth;
                highestHealthUnit = unit;
            }
        }

        if (highestHealthUnit == null)
        {
            EndRound();
            return null;
        }

        return highestHealthUnit;
    }

    /// <summary>
    /// Returns all enemies in range of the given position. If no enemies exist, ends the round and returns null.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public List<Hittable> GetAllEnemiesInRange(Vector3 position, float range)
    {
        List<Hittable> enemiesInRange = new List<Hittable>();

        if (_enemyUnits.Count == 0)
        {
            EndRound();
            return enemiesInRange;
        }

        float rangeSquared = range * range;
        foreach (BaseUnit unit in _enemyUnits)
        {
            if (unit.Dead) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr <= rangeSquared)
            {
                enemiesInRange.Add(unit);
            }
        }

        if (enemiesInRange.Count == 0)
        {
            EndRound();
        }

        return enemiesInRange;
    }

    /// <summary>
    /// Returns all allied hittables (units, towers and castle) in range of the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<Hittable> GetAllAlliesInRange(Vector3 position, float range)
    {
        float rangeSquared = range * range;
        List<Hittable> alliesInRange = new List<Hittable>();
        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit.Dead) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr <= rangeSquared)
            {
                alliesInRange.Add(unit);
            }
        }
        foreach (BaseTower tower in _towers)
        {
            float distanceSqr = Vector3.SqrMagnitude(position - tower.transform.position);
            if (distanceSqr <= rangeSquared)
            {
                alliesInRange.Add(tower);
            }
        }
        float castleDistanceSqr = Vector3.SqrMagnitude(position - _castle.transform.position);
        if (castleDistanceSqr <= rangeSquared)
        {
            alliesInRange.Add(_castle);
        }
        return alliesInRange;
    }

    public void AddBody()
    {
        List<Cemetery> cemeteries = new List<Cemetery>();
        foreach (BaseTower tower in _towers)
        {
            if (tower is Cemetery)
            {
                cemeteries.Add(tower as Cemetery);
            }
        }
        bool added = false;
        while (cemeteries.Count > 0 && !added)
        {
            int index = Random.Range(0, cemeteries.Count);
            Cemetery cemetery = cemeteries[index];
            if (cemetery != null && !cemetery.IsFull())
            {
                Debug.Log("Adding body to cemetery: " + cemetery.name + " at " + cemetery.transform.position);
                AllyUnit newZombie = Instantiate(_zombiePrefab, cemetery.transform.position, Quaternion.identity);
                added = true;
                newZombie.ChangeCemetery(cemetery);
                _alliedUnits.Add(newZombie);
            }
            cemeteries.RemoveAt(index);
        }

    }

    public void RemoveBodies(int amount)
    {
        int bodiesToRemove = amount;
        for (int i = _alliedUnits.Count - 1; i >= 0 && bodiesToRemove > 0; i--)
        {
            if (_alliedUnits[i].unitType == AllyUnitsEnum.Zombie)
            {
                Destroy(_alliedUnits[i].gameObject);
                _alliedUnits.RemoveAt(i);
                bodiesToRemove--;
            }
        }
        if (bodiesToRemove > 0)
        {
            Debug.LogError("Not enough bodies to remove!");
        }
    }

    public void EndGame()
    {
        Debug.Log($"Castle is destroyed! You survived {_currentRound} rounds.");
    }

    public void AddBlood(int amount)
    {
        _blood += amount;
        if (_blood > _maxBlood) _blood = _maxBlood;
    }

    public int GetBodies()
    {
        int zombieCount = 0;
        foreach (AllyUnit unit in _alliedUnits)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
            {
                zombieCount++;
            }
        }
        return zombieCount;
    }

    public int GetBlood()
    {
        return _blood;
    }

    IEnumerator StartRoundCoroutine()
    {
        PrepareRound();
        yield return new WaitForSeconds(3f); // Wait for 3 seconds before starting the round
        StartRound();
    }

    public void AddMaxBlood(int numberBlood)
    {
        _maxBlood += numberBlood;
        if (_blood > _maxBlood) _blood = _maxBlood;
    }

    public void AddEnemyUnit(EnemyUnit enemy)
    {
        _enemyUnits.Add(enemy);
    }

    public void AddAllyUnit(AllyUnit ally)
    {
        _alliedUnits.Add(ally);
    }

    public AllyUnitPrice GetUnitPrice(AllyUnitsEnum unitType)
    {
        foreach (AllyUnitPrice unitPrice in _unitPrices)
        {
            if (unitPrice.unitType == unitType)
            {
                return unitPrice;
            }
        }
        return null;
    }

    public List<Cemetery> GetCemeteries(Cemetery exclude = null)
    {
        List<Cemetery> cemeteries = new List<Cemetery>();
        foreach (BaseTower tower in _towers)
        {
            if (tower is Cemetery && tower != exclude)
            {
                cemeteries.Add(tower as Cemetery);
            }
        }
        return cemeteries;
    }

    public void CleanUpCemetery(Cemetery cemetery)
    {
        foreach (AllyUnit unit in cemetery.GetComponentsInChildren<AllyUnit>())
        {
            unit.ChangeCemetery(null);
            unit.Reset();
            if (unit.Dead)
            {
                Destroy(unit.gameObject);
            }
        }
        _alliedUnits.RemoveAll(unit => unit.Dead);
    }

    public void RemoveTower(BaseTower tower)
    {
        _towers.Remove(tower);
        Destroy(tower.gameObject);
    }

    private IEnumerator MapSpawnAnimation()
    {
        for (int i = 0; i < _mapWidth; i++)
        {
            StartCoroutine(TileRowAnimation(i));
            yield return new WaitForSeconds(0.15f);
        }
        StartCoroutine(InitialZombieSpawns());
    }
    private IEnumerator TileRowAnimation(int row)
    {
        for (int i = 0; i < _mapHeight; i++)
        {
            _tiles[i + row * _mapWidth].SpawnTileAnimation();
            yield return new WaitForSeconds(0.05f);
        }
    }
    private IEnumerator InitialZombieSpawns()
    {
        for (int i = 0; i < _initialZombies; i++)
        {
            AddBody();
            yield return new WaitForSeconds(0.5f);
        }
    }
}