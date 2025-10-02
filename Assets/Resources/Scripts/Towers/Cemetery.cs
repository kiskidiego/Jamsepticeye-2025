using UnityEngine;

public class Cemetery : BaseTower
{
    [SerializeField] int _numberBodiesToAdd;
    [SerializeField] private Canvas _barracksMenu;

    protected override void OnBuy()
    {
        GameManager.Instance.AddMaxBodies(_numberBodiesToAdd);
    }
    protected override void OnSell()
    {
        GameManager.Instance.AddMaxBodies(- _numberBodiesToAdd);
        base.OnSell();
    }

    protected override void WhenDestroyed()
    {
        GameManager.Instance.AddMaxBodies(- _numberBodiesToAdd);
        base.WhenDestroyed();
    }
    
    

    protected override void OnClick()
    {
        _barracksMenu.enabled = true;
    }

    public void BuyUnit(int idUnit)
    {
        GameManager manager = GameManager.Instance;
        int unitPrice = 0;
        switch (idUnit)
        {
            case 1:
                unitPrice = 50;
                break;
            default:
                Debug.Log("Wrong Unit ID");
                return;
        }

        if (manager.GetBodies() < unitPrice)
        {
            Debug.Log("Not enough blood");
            return;
        }

        manager.AddBodies(-unitPrice);

        //TODO: Add unit to the gameManagerList
        //manager.alliedUnits.Add();
    }
}
