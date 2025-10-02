using UnityEngine;

public class BoneTower : BaseTower
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;
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

        // Instantiate and launch the projectile towards the target
        Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
        projectile.target = _target.transform;
        projectile.SetDamage(_damage);

        Destroy(projectile.gameObject, 5f); // Destroy after 5 seconds to avoid clutter
    }
}
