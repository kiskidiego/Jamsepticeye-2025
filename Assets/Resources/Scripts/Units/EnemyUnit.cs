using UnityEngine;

public class EnemyUnit : BaseUnit
{
    public int BodyReward => _bodyReward;
    public int BloodReward => _bloodReward;
    [SerializeField] int _bodyReward = 1;
    [SerializeField] int _bloodReward = 3;
}