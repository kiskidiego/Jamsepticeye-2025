using UnityEngine;

public class GoblinomancyStudy : BaseTower
{
    [SerializeField] private Canvas _spellMenu;

    protected override void OnClick()
    {
        _spellMenu.enabled = true;
    }

    public void BuySpell(int idSpell)
    {
        GameManager manager = GameManager.Instance;
        int spellPrice;
        
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
