using System;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;

public class UnitMenu : MonoBehaviour
{
    [Serializable]
    class UnitArticle
    {
        public AllyUnitsEnum unitType;
        public TextMeshProUGUI bodyPriceText;
        public TextMeshProUGUI bloodPriceText;
    }
    [Serializable]
    class UnitAmount
    {
        public AllyUnitsEnum unitType;
        public TextMeshProUGUI amountText;
        [HideInInspector] public int amount;
    }
    [HideInInspector] public Cemetery cemetery;
    [SerializeField] TextMeshProUGUI TotalZombieAmountText;
    [SerializeField] TextMeshProUGUI BloodAmountText;
    [SerializeField] List<UnitArticle> articles = new List<UnitArticle>();
    [SerializeField] List<UnitAmount> amounts = new List<UnitAmount>();
    [SerializeField] EventReference _menuInteractionSound;
    List<AllyUnitPrice> _prices = new List<AllyUnitPrice>();
    public void Init(List<AllyUnitPrice> prices)
    {
        _prices = prices;
        foreach (AllyUnitPrice price in _prices)
        {
            foreach (UnitArticle article in articles)
            {
                if (article.unitType == price.unitType)
                {
                    article.bodyPriceText.text = price.price.bodyPrice.ToString() + " Z";
                    article.bloodPriceText.text = price.price.bloodPrice.ToString() + " B";
                    break;
                }
            }
        }

    }
    public void OpenMenu(Cemetery cemetery)
    {
        this.cemetery = cemetery;
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
        BloodAmountText.text = GameManager.Instance.GetBlood().ToString();

        foreach (UnitAmount amount in amounts)
        {
            amount.amount = 0;
            foreach (AllyUnit unit in cemetery.Units)
            {
                if (unit.unitType == amount.unitType)
                    amount.amount++;
            }
            amount.amountText.text = "x" + amount.amount.ToString();
        }
        gameObject.SetActive(true);
    }
    public void BuyUnit(UnitEnumHolder unitEnumHolder)
    {
        if (!cemetery.BuyUnit(unitEnumHolder.unitType)) return;

        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);

        int zombieAmount = 0;
        int unitAmount = 0;
        foreach (AllyUnit unit in cemetery.Units)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
                zombieAmount++;
            if (unit.unitType == unitEnumHolder.unitType)
                unitAmount++;
        }
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
        BloodAmountText.text = GameManager.Instance.GetBlood().ToString();

        GetUnitAmount(AllyUnitsEnum.Zombie).amountText.text = "x" + zombieAmount.ToString();
        GetUnitAmount(unitEnumHolder.unitType).amountText.text = "x" + unitAmount.ToString();
    }
    UnitAmount GetUnitAmount(AllyUnitsEnum unitType)
    {
        foreach (UnitAmount amount in amounts)
        {
            if (amount.unitType == unitType)
                return amount;
        }
        return null;
    }
    public void CloseMenu()
    {
        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);
        gameObject.SetActive(false);
    }
}
