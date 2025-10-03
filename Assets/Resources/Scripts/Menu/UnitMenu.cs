using FMODUnity;
using TMPro;
using UnityEngine;

public class UnitMenu : MonoBehaviour
{
    [HideInInspector] public Cemetery cemetery;
    [SerializeField] TextMeshProUGUI TotalZombieAmountText;
    [SerializeField] TextMeshProUGUI BloodAmountText;
    [SerializeField] TextMeshProUGUI ZombieZombiePriceText;
    [SerializeField] TextMeshProUGUI ZombieBloodPriceText;
    [SerializeField] TextMeshProUGUI ArcherZombiePriceText;
    [SerializeField] TextMeshProUGUI ArcherBloodPriceText;
    [SerializeField] TextMeshProUGUI GhoulZombiePriceText;
    [SerializeField] TextMeshProUGUI GhoulBloodPriceText;
    [SerializeField] TextMeshProUGUI VampireZombiePriceText;
    [SerializeField] TextMeshProUGUI VampireBloodPriceText;
    [SerializeField] TextMeshProUGUI AlchemistZombiePriceText;
    [SerializeField] TextMeshProUGUI AlchemistBloodPriceText;
    [SerializeField] TextMeshProUGUI ZombieAmountText;
    [SerializeField] TextMeshProUGUI ArcherAmountText;
    [SerializeField] TextMeshProUGUI GhoulAmountText;
    [SerializeField] TextMeshProUGUI VampireAmountText;
    [SerializeField] TextMeshProUGUI AlchemistAmountText;
    [SerializeField] EventReference _menuInteractionSound;
    public void Init(Price zombiePrice, Price archerPrice, Price ghoulPrice, Price vampirePrice, Price alchemistPrice)
    {
        ZombieZombiePriceText.text = zombiePrice.bodyPrice.ToString();
        ZombieBloodPriceText.text = zombiePrice.bloodPrice.ToString();
        ArcherZombiePriceText.text = archerPrice.bodyPrice.ToString();
        ArcherBloodPriceText.text = archerPrice.bloodPrice.ToString();
        GhoulZombiePriceText.text = ghoulPrice.bodyPrice.ToString();
        GhoulBloodPriceText.text = ghoulPrice.bloodPrice.ToString();
        VampireZombiePriceText.text = vampirePrice.bodyPrice.ToString();
        VampireBloodPriceText.text = vampirePrice.bloodPrice.ToString();
        AlchemistZombiePriceText.text = alchemistPrice.bodyPrice.ToString();
        AlchemistBloodPriceText.text = alchemistPrice.bloodPrice.ToString();
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
        if(!cemetery.BuyUnit(AllyUnitsEnum.Zombie)) return;

        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);

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
        if(!cemetery.BuyUnit(AllyUnitsEnum.Archer)) return;

        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);

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
        if(!cemetery.BuyUnit(AllyUnitsEnum.Ghoul)) return;

        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);

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
        if(!cemetery.BuyUnit(AllyUnitsEnum.Vampire)) return;

        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);

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
        if(!cemetery.BuyUnit(AllyUnitsEnum.Alchemist)) return;

        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);

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
        AudioManager.instance.PlayOneShot(_menuInteractionSound, transform.position);
        gameObject.SetActive(false);
    }
}
