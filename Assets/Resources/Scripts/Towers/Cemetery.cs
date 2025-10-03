using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cemetery : BaseTower
{
    public List<AllyUnit> Units => _units;
    public Transform UnitSpawnPoint => _unitSpawnPoint;
    private List<AllyUnit> _units = new List<AllyUnit>();
    [SerializeField] int _capacity = 20;
    [SerializeField] float _spawnRadius;
    [SerializeField] private Transform _unitSpawnPoint = null; // Point where units will spawn

    protected override void Start()
    {
        base.Start();
        if (_unitSpawnPoint == null)
        {
            _unitSpawnPoint = transform;
        }
    }

    protected override void OnSell()
    {
        MoveUnitsAway();

        GameManager.Instance.CleanUpCemetery(this);

        base.OnSell();
    }

    protected override void Die()
    {
        MoveUnitsAway();

        GameManager.Instance.CleanUpCemetery(this);

        base.Die();
    }

    void MoveUnitsAway()
    {
        List<Cemetery> cemeteries = GameManager.Instance.GetCemeteries(this);

        while (cemeteries.Count > 0 && _units.Count > 0)
        {
            foreach (Cemetery cemetery in cemeteries)
            {
                if (cemetery == this)
                {
                    cemeteries.Remove(cemetery);
                    continue;
                }

                if (cemetery.IsFull())
                {
                    cemeteries.Remove(cemetery);
                    continue;
                }

                if (_units.Count == 0) break;

                _units[Random.Range(0, _units.Count)].ChangeCemetery(cemetery);
            }
        }
    }

    public override void OnInteract()
    {
        if (_paused) return;
        base.OnInteract();

        GameManager.Instance.unitMenu.OpenMenu(this);
    }

    /// <summary>
    /// Buys a unit of the specified type if enough resources are available.
    /// </summary>
    /// <param name="unitId"></param>
    public bool BuyUnit(AllyUnitsEnum unitId)
    {
        GameManager manager = GameManager.Instance;
        AllyUnitPrice unitPrice = manager.GetUnitPrice(unitId);
        if (unitPrice == null) return false;

        if (manager.GetBodies() < unitPrice.price.bodyPrice || manager.GetBlood() < unitPrice.price.bloodPrice) return false;

        if (IsFull()) return false;

        manager.RemoveBodies(-unitPrice.price.bodyPrice);
        manager.AddBlood(-unitPrice.price.bloodPrice);

        AllyUnit newUnit = Instantiate(unitPrice.unitPrefab, transform.position, Quaternion.identity);
        newUnit.spawnRadius = _spawnRadius;
        manager.AddAllyUnit(newUnit);

        newUnit.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack);

        AddUnit(newUnit);
        return true;
    }
    public void AddUnit(AllyUnit unit)
    {
        unit._cemetery = this;
        _units.Add(unit);
    }
    public void RemoveUnit(AllyUnit unit)
    {
        unit._cemetery = null;
        _units.Remove(unit);
    }
    public bool IsFull()
    {
        return _units.Count >= _capacity;
    }
}
