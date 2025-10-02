using UnityEngine;

[System.Serializable]
public class Round
{
    public RandomRoundEnemy[] RandomEnemiesInRound; // Array of enemies that can appear in this round
    public int TotalRandomEnemies; // Total number of enemies to spawn in this round
    public FixedRoundEnemy[] FixedEnemiesInRound; // Array of enemies that will definitely appear in this round
}