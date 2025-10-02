using UnityEngine;

[System.Serializable]
public class RoundEnemy
{
    public BaseUnit EnemyUnit;
    [SerializeField, Range(0, 1)] public float Probability; // Probability of this enemy appearing in the round (0 to 1)
}