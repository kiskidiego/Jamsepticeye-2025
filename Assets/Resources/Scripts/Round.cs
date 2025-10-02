using UnityEngine;

[System.Serializable]
public class Round
{
    public RoundEnemy[] EnemiesInRound; // Array of enemies that can appear in this round
    public int TotalEnemies; // Total number of enemies to spawn in this round
}