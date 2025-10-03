using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;

public class ConstructionMenu : MonoBehaviour
{
    [System.Serializable]
    class TowerArticle
    {
        public TowersEnum towerType;
        public TextMeshProUGUI bodyPriceText;
        public TextMeshProUGUI bloodPriceText;
    }
    [SerializeField] EventReference _menuInteractionSound;
    [SerializeField] List<TowerArticle> _articles = new List<TowerArticle>();
    [SerializeField] TextMeshProUGUI ZombieAmountText;
    [SerializeField] TextMeshProUGUI BloodAmountText;
    List<TowerPrice> _prices = new List<TowerPrice>();

    void Update()
    {
        ZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
        BloodAmountText.text = GameManager.Instance.GetBlood().ToString();
    }

    public void Init(List<TowerPrice> towerPrices)
    {
        _prices = towerPrices;

        foreach (TowerPrice price in _prices)
        {
            foreach (TowerArticle article in _articles)
            {
                if (article.towerType == price.towerType)
                {
                    article.bodyPriceText.text = price.price.bodyPrice.ToString() + " Z";
                    article.bloodPriceText.text = price.price.bloodPrice.ToString() + " B";
                    break;
                }
            }
        }
    }
    public void BuyTower(TowerEnumHolder towerEnumHolder)
    {
        int priceIndex = GetPriceIndex(towerEnumHolder.towerType);
        if (priceIndex != -1)
        {
            Price price = _prices[priceIndex].price;
            if (GameManager.Instance.GetBodies() >= price.bodyPrice && GameManager.Instance.GetBlood() >= price.bloodPrice)
            {
                GameManager.Instance.RemoveBodies(price.bodyPrice);
                GameManager.Instance.AddBlood(-price.bloodPrice);
                GameManager.Instance.Construct(towerEnumHolder.towerType);
                RuntimeManager.PlayOneShot(_menuInteractionSound);
            }
        }
    }

    int GetPriceIndex(TowersEnum towerType)
    {
        for (int i = 0; i < _prices.Count; i++)
        {
            if (_prices[i].towerType == towerType)
                return i;
        }
        return -1;
    }
}