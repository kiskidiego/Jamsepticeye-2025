using UnityEngine;

public class BloodPool : BaseTower
{
    [SerializeField] int _numberBloodToAdd;
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.AddMaxBlood(_numberBloodToAdd);
    }
    protected override void OnSell()
    {
        GameManager.Instance.AddMaxBlood(- _numberBloodToAdd);
        base.OnSell();
    }

    protected override void Die()
    {
        GameManager.Instance.AddMaxBlood(- _numberBloodToAdd);
        base.Die();
    }
}
