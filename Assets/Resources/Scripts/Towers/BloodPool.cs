using UnityEngine;

public class BloodPool : BaseTower
{
    [SerializeField] int _numberBloodToAdd;
    protected override void OnBuy()
    {
        GameManager.Instance.AddMaxBlood(_numberBloodToAdd);
    }
    protected override void OnSell()
    {
        GameManager.Instance.AddMaxBlood(- _numberBloodToAdd);
        base.OnSell();
    }

    protected override void WhenDestroyed()
    {
        GameManager.Instance.AddMaxBlood(- _numberBloodToAdd);
        base.WhenDestroyed();
    }
}
