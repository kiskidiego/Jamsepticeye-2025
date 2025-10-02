using UnityEngine;

public class Cemetery : BaseTower
{
    [SerializeField] int _numberBodiesToAdd;
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
    
    [SerializeField] private Canvas _spellMenu;

    protected override void OnClick()
    {
        _spellMenu.enabled = true;
    }

    public void BuySpell(int idSpell)
    {
        GameManager manager = GameManager.Instance;
        int spellPrice = 0;
        switch (idSpell)
        {
            case 1:
                spellPrice = 50;
                break;
            default:
                Debug.Log("Wrong Spell ID");
                return;
        }

        if (manager.GetBlood() < spellPrice)
        {
            Debug.Log("Not enough blood");
            return;
        }

        manager.AddBlood(-spellPrice);
        manager.UnlockedSpells[idSpell] = true;
    }
}
