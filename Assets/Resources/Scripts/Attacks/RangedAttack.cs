using System;
using UnityEngine;

public class RangedAttack : BaseAttack
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;

    /// <summary>
    /// Spawns a projectile and launches it towards the target.
    /// </summary>
    protected override void AttackEffect(Hittable target)
    {
        if (target == null) return;

        // Instantiate and launch the projectile towards the target
        Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
        projectile.target = target.transform;
        projectile.targetSize = target.GetSize();
        projectile.SetDamage(_damage);
        projectile.isAlly = isAlly;

        Destroy(projectile.gameObject, 5f); // Destroy after 5 seconds to avoid clutter
    }
}