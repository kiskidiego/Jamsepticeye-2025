using UnityEngine;

public class GoblinomancyStudy : BaseTower
{
    [SerializeField] private Canvas _spellMenu;

    protected override void OnInteract()
    {
        if (_paused) return;

        base.OnInteract();
        _spellMenu.enabled = true;
    }

    public void BuySpell(SpellEnum spellId)
    {
        GameManager manager = GameManager.Instance;
        SpellPrice spellPrice = manager.GetSpellPrice(spellId);
        if (spellPrice == null)
        {
            Debug.LogError($"SpellPrice for {spellId} not found.");
            return;
        }
        if (manager.GetBodies() >= spellPrice.price.bodyPrice && manager.GetBlood() >= spellPrice.price.bloodPrice)
        {
            manager.RemoveBodies(spellPrice.price.bodyPrice);
            manager.AddBlood(-spellPrice.price.bloodPrice);
            manager.UnlockSpell(spellPrice.spellPrefab);
            Debug.Log($"{spellId} spell purchased and unlocked.");
        }
        else
        {
            Debug.Log("Not enough resources to buy this spell.");
        }
    }
}
