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

    public BaseUnit GetClosestUnit(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track units first.");
    }

    public BaseTower GetClosestTower(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track towers first.");
    }

    public BaseUnit GetStrongestUnit(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track units first.");
    }

    public Hittable GetClosestHittable(Vector3 position)
    {
        throw new System.NotImplementedException("Need to implement a way to track hittables first.");
    }
}