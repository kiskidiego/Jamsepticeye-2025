using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance, new game managers will override old ones
    public Hittable Castle { get; private set; }
    [SerializeField] private Hittable _castle;
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
    public BaseUnit GetClosestSubject(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track units first.");
    }

    /// <summary>
    /// Returns the closest enemy unit to the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit GetClosestEnemy(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track units first.");
    }

    /// <summary>
    /// Returns the closest tower to the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseTower GetClosestTower(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track towers first.");
    }

    /// <summary>
    /// Returns the strongest allied unit to the given position. Not implemented yet.
    /// </summary>
    public BaseUnit GetStrongestFollower()
    {
        throw new System.NotImplementedException("Need to implement a way to track units first.");
    }

    /// <summary>
    /// Returns the strongest enemy unit to the given position. Not implemented yet.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BaseUnit GetStrongestEnemy()
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
}