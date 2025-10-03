using UnityEngine;

[System.Serializable]
public class RandomRoundEnemy
{
    public EnemyUnit enemyUnit;
    [SerializeField, Range(0, 1)] public float Probability; // Probability of this enemy appearing in the round (0 to 1)
}