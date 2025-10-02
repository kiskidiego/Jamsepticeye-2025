using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance, new game managers will override old ones
    public Hittable Castle { get; private set; }
    [SerializeField] private Hittable _castle;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private int _mapWidth = 10;
    [SerializeField] private int _mapHeight = 10;
    BaseUnit[] _alliedUnits;
    BaseUnit[] _enemyUnits;
    BaseTower[] _towers;
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
        //Castle = _castle;
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
                Tile newTile = Instantiate(_tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                if (x == 0 || y == 0 || x == _mapWidth - 1 || y == _mapHeight - 1)
                {
                    GameObject tree = Instantiate(treePrefab, new Vector3(x, 0, y), Quaternion.identity, newTile.transform);
                    newTile.InitializeTile(Tile.TileState.Occupied, tree);
                }
                else if (x > 3) newTile.InitializeTile(Tile.TileState.Battlefield);
                else newTile.InitializeTile(Tile.TileState.Buildable);
                _tiles[x + y * _mapWidth] = newTile;
            }
        }
        StartCoroutine(MapSpawnAnimation());
    }

    /// <summary>
    /// Returns the closest allied unit to the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit GetClosestAllyUnit(Vector3 position)
    {
        BaseUnit closestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit == null) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr < closestDistance)
            {
                closestDistance = distanceSqr;
                closestUnit = unit;
            }
        }

        return closestUnit;
    }

    /// <summary>
    /// Returns the closest enemy unit to the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit GetClosestEnemy(Vector3 position)
    {
        BaseUnit closestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseUnit unit in _enemyUnits)
        {
            if (unit == null) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr < closestDistance)
            {
                closestDistance = distanceSqr;
                closestUnit = unit;
            }
        }

        return closestUnit;
    }

    /// <summary>
    /// Returns the closest tower to the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseTower GetClosestTower(Vector3 position)
    {
        BaseTower closestTower = null;
        float closestDistance = Mathf.Infinity;

        foreach (BaseTower tower in _towers)
        {
            if (tower == null) continue;

            float distanceSqr = Vector3.SqrMagnitude(position - tower.transform.position);
            if (distanceSqr < closestDistance)
            {
                closestDistance = distanceSqr;
                closestTower = tower;
            }
        }

        return closestTower;
    }

    /// <summary>
    /// Returns the allied unit with the highest max health. Not implemented yet.
    /// </summary>
    public BaseUnit GetHighestHealthAllyUnit()
    {
        BaseUnit highestHealthUnit = null;
        float highestHealth = -Mathf.Infinity;
        foreach (BaseUnit unit in _alliedUnits)
        {
            if (unit == null) continue;

            if (unit.MaxHealth > highestHealth)
            {
                highestHealth = unit.MaxHealth;
                highestHealthUnit = unit;
            }
        }
        return highestHealthUnit;
    }

    /// <summary>
    /// Returns the enemy unit with the highest max health. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit GetHighestHealthEnemy()
    {
        throw new System.NotImplementedException("Need to implement a way to track units first.");
    }

    /// <summary>
    /// Returns all enemies in range of the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit[] GetAllEnemiesInRange(Vector3 position, float range)
    {
        throw new System.NotImplementedException("Need to implement a way to track hittables first.");
    }

    /// <summary>
    /// Returns all allied hittables (units, towers and castle) in range of the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Hittable[] GetAllAlliesInRange(Vector3 position, float range)
    {
        throw new System.NotImplementedException("Need to implement a way to track hittables first.");
    }

    private IEnumerator MapSpawnAnimation()
    {
        foreach (Tile tile in _tiles)
        {
            tile.SpawnTileAnimation();
            yield return new WaitForSeconds(0.02f);
        }
    }
}