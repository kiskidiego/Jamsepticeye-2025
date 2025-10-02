using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance, new game managers will override old ones
    public Hittable Castle { get; private set; }
    [SerializeField] private Hittable _castle;
    [SerializeField] Round[] _rounds;
    [SerializeField] BaseUnit _testAlly;
    List<BaseUnit> _alliedUnits = new List<BaseUnit>();
    List<BaseUnit> _enemyUnits = new List<BaseUnit>();
    List<BaseTower> _towers = new List<BaseTower>();
    int _currentRound = 0;
    int _blood = 0;
    int _bodies = 0;
    PhaseEnum _currentPhase = PhaseEnum.Build;
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
        Castle = _castle;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 110; i++)
                _alliedUnits.Add(Instantiate(_testAlly, new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-10f, -5f)), Quaternion.identity)); // Replace Vector3.zero with spawn point
        }
        if (_currentPhase == PhaseEnum.Build)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PrepareRound();
                StartRound();
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

    /// <summary>
    /// Prepares the next round by clearing existing enemies and spawning new ones based on defined probabilities.
    /// </summary>
    void PrepareRound()
    {
        Round round = _rounds[_currentRound];
        for (int i = 0; i < round.TotalEnemies; i++)
        {
            float rand = Random.Range(0f, 1f);
            float cumulativeProbability = 0f;
            foreach (RoundEnemy roundEnemy in round.EnemiesInRound)
            {
                cumulativeProbability += roundEnemy.Probability;
                if (rand <= cumulativeProbability)
                {
                    _enemyUnits.Add(Instantiate(roundEnemy.EnemyUnit, new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-5f, 5f)), Quaternion.identity)); // Replace Vector3.zero with spawn point
                    break;
                }
            }
        }
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
        _currentPhase = PhaseEnum.Combat;
    }

    /// <summary>
    /// Ends the round by rewarding the player and cleaning up units.
    /// </summary>
    void EndRound()
    {
        foreach (BaseUnit unit in _enemyUnits)
        {
            _bodies += unit.BodyReward;
            _blood += unit.BloodReward;
            Destroy(unit.gameObject);
        }
        _enemyUnits.Clear();

        Debug.Log($"Round {_currentRound + 1} completed! Rewards: {_bodies} bodies, {_blood} blood.");

        foreach (BaseUnit unit in _alliedUnits)
        {
            unit.Pause();
            unit.Reset();
            unit.transform.position = new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-10f, -5f)); // Replace with spawn point
            if (unit.Dead)
            {
                Destroy(unit.gameObject);
            }
        }
        _alliedUnits.RemoveAll(unit => unit.Dead);
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
    public Hittable GetClosestAllyUnit(Vector3 position)
    {
        if (_alliedUnits.Count == 0) return GetClosestTower(position); // If no allied units, return closest tower

        Hittable closestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit.Dead) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr < closestDistance)
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
    public Hittable GetClosestEnemy(Vector3 position)
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
            if (unit.Dead) continue;

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
    public Hittable GetClosestTower(Vector3 position)
    {
        if (_towers.Count == 0) return _castle; // If no towers, return castle

        Hittable closestTower = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseTower tower in _towers)
        {            
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
    public Hittable GetHighestHealthAllyUnit(Vector3 position)
    {
        if (_alliedUnits.Count == 0) return GetClosestTower(position);

        Hittable highestHealthUnit = null;
        float highestHealth = -Mathf.Infinity;
        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit.Dead) continue;

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
    public Hittable GetHighestHealthEnemy()
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
            if (unit.Dead) continue;

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
    public Hittable[] GetAllEnemiesInRange(Vector3 position, float range)
    {
        if (_enemyUnits.Count == 0)
        {
            EndRound();
            return null;
        }

        float rangeSquared = range * range;
        List<Hittable> enemiesInRange = new List<Hittable>();
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
            return null;
        }

        return enemiesInRange.ToArray();
    }

    /// <summary>
    /// Returns all allied hittables (units, towers and castle) in range of the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Hittable[] GetAllAlliesInRange(Vector3 position, float range)
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
        return alliesInRange.ToArray();
    }
}