using TMPro;
using UnityEngine;

public class UnitMenu : MonoBehaviour
{
    [HideInInspector] public Cemetery cemetery;
    [SerializeField] TextMeshPro TotalZombieAmountText;
    [SerializeField] TextMeshPro BloodAmountText;
    [SerializeField] TextMeshPro ZombiePriceText;
    [SerializeField] TextMeshPro ArcherPriceText;
    [SerializeField] TextMeshPro GhoulPriceText;
    [SerializeField] TextMeshPro VampirePriceText;
    [SerializeField] TextMeshPro AlchemistPriceText;
    [SerializeField] TextMeshPro ZombieAmountText;
    [SerializeField] TextMeshPro ArcherAmountText;
    [SerializeField] TextMeshPro GhoulAmountText;
    [SerializeField] TextMeshPro VampireAmountText;
    [SerializeField] TextMeshPro AlchemistAmountText;
    public void Init(Price zombiePrice, Price archerPrice, Price ghoulPrice, Price vampirePrice, Price alchemistPrice)
    {
        ZombiePriceText.text = zombiePrice.ToString();
        ArcherPriceText.text = archerPrice.ToString();
        GhoulPriceText.text = ghoulPrice.ToString();
        VampirePriceText.text = vampirePrice.ToString();
        AlchemistPriceText.text = alchemistPrice.ToString();
    }
    public void OpenMenu(Cemetery cemetery)
    {
        this.cemetery = cemetery;
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
        BloodAmountText.text = GameManager.Instance.GetBlood().ToString();

        int zombieAmount = 0;
        int archerAmount = 0;
        int ghoulAmount = 0;
        int vampireAmount = 0;
        int alchemistAmount = 0;

        foreach (AllyUnit unit in cemetery.Units)
        {
            switch (unit.unitType)
            {
                case AllyUnitsEnum.Zombie:
                    zombieAmount++;
                    break;
                case AllyUnitsEnum.Archer:
                    archerAmount++;
                    break;
                case AllyUnitsEnum.Ghoul:
                    ghoulAmount++;
                    break;
                case AllyUnitsEnum.Vampire:
                    vampireAmount++;
                    break;
                case AllyUnitsEnum.Alchemist:
                    alchemistAmount++;
                    break;
            }
        }

        ZombieAmountText.text = zombieAmount.ToString();
        ArcherAmountText.text = archerAmount.ToString();
        GhoulAmountText.text = ghoulAmount.ToString();
        VampireAmountText.text = vampireAmount.ToString();
        AlchemistAmountText.text = alchemistAmount.ToString();

        gameObject.SetActive(true);
    }
    public void BuyZombie()
    {
        cemetery.BuyUnit(AllyUnitsEnum.Zombie);
        int zombieAmount = 0;
        foreach (AllyUnit unit in cemetery.Units)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
                zombieAmount++;
        }
        ZombieAmountText.text = zombieAmount.ToString();
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
    }
    public void BuyArcher()
    {
        cemetery.BuyUnit(AllyUnitsEnum.Archer);
        int archerAmount = 0;
        int zombieAmount = 0;
        foreach (AllyUnit unit in cemetery.Units)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
                zombieAmount++;
            else
            if (unit.unitType == AllyUnitsEnum.Archer)
                archerAmount++;
        }
        ArcherAmountText.text = archerAmount.ToString();
        ZombieAmountText.text = zombieAmount.ToString();
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
    }
    public void BuyGhoul()
    {
        cemetery.BuyUnit(AllyUnitsEnum.Ghoul);
        int ghoulAmount = 0;
        int zombieAmount = 0;
        foreach (AllyUnit unit in cemetery.Units)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
                zombieAmount++;
            else
            if (unit.unitType == AllyUnitsEnum.Ghoul)
                ghoulAmount++;
        }
        GhoulAmountText.text = ghoulAmount.ToString();
        ZombieAmountText.text = zombieAmount.ToString();
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
    }
    public void BuyVampire()
    {
        cemetery.BuyUnit(AllyUnitsEnum.Vampire);
        int vampireAmount = 0;
        int zombieAmount = 0;
        foreach (AllyUnit unit in cemetery.Units)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
                zombieAmount++;
            else
            if (unit.unitType == AllyUnitsEnum.Vampire)
                vampireAmount++;
        }
        VampireAmountText.text = vampireAmount.ToString();
        ZombieAmountText.text = zombieAmount.ToString();
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
    }
    public void BuyAlchemist()
    {
        cemetery.BuyUnit(AllyUnitsEnum.Alchemist);
        int alchemistAmount = 0;
        int zombieAmount = 0;
        foreach (AllyUnit unit in cemetery.Units)
        {
            if (unit.unitType == AllyUnitsEnum.Zombie)
                zombieAmount++;
            else
            if (unit.unitType == AllyUnitsEnum.Alchemist)
                alchemistAmount++;
        }
        AlchemistAmountText.text = alchemistAmount.ToString();
        ZombieAmountText.text = zombieAmount.ToString();
        TotalZombieAmountText.text = GameManager.Instance.GetBodies().ToString();
    }
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
