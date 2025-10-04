using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the spell unlock menu.
/// </summary>
public class SpellUnlockMenu : BaseMenu
{
    class article
    {
        public SpellEnum spellType;
        public TextMeshProUGUI bodyPriceText;
        public TextMeshProUGUI bloodPriceText;
    }
    [SerializeField] List<article> _articles = new List<article>();
    [SerializeField] SpellUnlockButton[] _buttons;
    List<SpellPrice> _prices = new List<SpellPrice>();

    public void Init(List<SpellPrice> prices)
    {
        _prices = prices;

        foreach (SpellPrice price in _prices)
        {
            foreach (article article in _articles)
            {
                if (article.spellType == price.spellType)
                {
                    article.bodyPriceText.text = price.price.bodyPrice.ToString() + " Z";
                    article.bloodPriceText.text = price.price.bloodPrice.ToString() + " B";
                    break;
                }
            }
        }
    }

    public void UnlockSpell(SpellEnumHolder spellEnumHolder)
    {
        SpellPrice price = GameManager.Instance.GetSpellPrice(spellEnumHolder.spellType);
        if (GameManager.Instance.GetBlood() < price.price.bloodPrice || GameManager.Instance.GetBodies() < price.price.bodyPrice) return;

        GameManager.Instance.AddBlood(-price.price.bloodPrice);
        GameManager.Instance.RemoveBodies(price.price.bodyPrice);

        GameManager.Instance.UnlockSpell(price.spellPrefab);
        Debug.Log("Unlocked spell: " + spellEnumHolder.spellType.ToString());
        GameObject button = GetUnlockButton(spellEnumHolder);
        Debug.Log("Button to destroy: " + (button != null ? button.name : "null"));
        Destroy(button);
    }

    GameObject GetUnlockButton(SpellEnumHolder spellEnumHolder)
    {
        foreach (SpellUnlockButton button in _buttons)
            if (button.spellType == spellEnumHolder.spellType)
                return button.gameObject;
        return null;
    }

    public override void OpenMenu()
    {
        if (_state == MenuState.Open) return;

        _menuTransform.DOLocalMove(Vector3.zero, _animationDuration).SetEase(Ease.OutBack);
        _state = MenuState.Open;
    }

    public override void CloseMenu()
    {
        Debug.Log("Closing Spell Unlock Menu");
        if (_state == MenuState.Closed) return;
        Debug.Log("Closed Spell Unlock Menu");
        _menuTransform.DOLocalMove(_closedPosition, _animationDuration).SetEase(Ease.OutBack);
        _state = MenuState.Closed;
    }
}