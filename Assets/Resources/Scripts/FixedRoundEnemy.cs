using UnityEngine;

[System.Serializable]
public class FixedRoundEnemy
{
    public EnemyUnit enemyUnit;
    [SerializeField] public int amount; // Amount of this enemy to spawn in the round
}