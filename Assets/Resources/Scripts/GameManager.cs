using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance, new game managers will override old ones
    public Hittable Castle { get; private set; }
    [SerializeField] private Hittable _castle;
    BaseUnit[] _alliedUnits;
    BaseUnit[] _enemyUnits;
    BaseTower[] _towers;
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

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
        BaseUnit highestHealthUnit = null;
        float highestHealth = -Mathf.Infinity;
        foreach (BaseUnit unit in _enemyUnits)
        {
            if (unit.MaxHealth > highestHealth)
            {
                highestHealth = unit.MaxHealth;
                highestHealthUnit = unit;
            }
        }
        return highestHealthUnit;
    }

    /// <summary>
    /// Returns all enemies in range of the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit[] GetAllEnemiesInRange(Vector3 position, float range)
    {
        float rangeSquared = range * range;
        List<BaseUnit> enemiesInRange = new List<BaseUnit>();
        foreach (BaseUnit unit in _enemyUnits)
        {
            float distanceSqr = Vector3.SqrMagnitude(position - unit.transform.position);
            if (distanceSqr <= rangeSquared)
            {
                enemiesInRange.Add(unit);
            }
        }
        return enemiesInRange.ToArray();
    }

    /// <summary>
    /// Returns all allied hittables (units, towers and castle) in range of the given position. Not implemented yet.
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
        return alliesInRange.ToArray();
    }
}