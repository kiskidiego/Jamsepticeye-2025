using UnityEngine;

[System.Serializable]
public class FixedRoundEnemy
{
    public BaseUnit EnemyUnit;
    [SerializeField] public int amount; // Amount of this enemy to spawn in the round
}