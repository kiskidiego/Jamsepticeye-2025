using UnityEngine;

public class BoneTower : BaseTower
{
    [SerializeField] private float _explosionRadius = 1.5f;
    void Update()
    {
        if (_paused) return;

        if (_target == null || (_target is BaseUnit && (_target as BaseUnit).Dead))
        {
            FindTarget();
            return;
        }
        CheckAttack();
    }
    
    protected override void Attack()
    {
        if (_target == null) return;
        Hittable[] hittedUnits = GameManager.Instance.GetAllEnemiesInRange(_target.transform.position, _explosionRadius);

        foreach (Hittable enemyUnit in hittedUnits)
        {
            _target.TakeDamage(_damage);
        }
    }
}
