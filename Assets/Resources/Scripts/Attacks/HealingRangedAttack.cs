using UnityEngine;

public class RangedHealingAttack : HealingAttack
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;
    
    /// <summary>
    /// Executes a healing ranged attack, restoring health to the target if it is an ally.
    /// </summary>
    protected override void AttackEffect(Hittable target)
    {
        if (target == null) return;

        // Instantiate and launch the projectile towards the target
        Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
        projectile.target = target.transform;
        projectile.targetSize = target.GetSize();
        projectile.SetDamage(_damage);
        projectile.isAlly = _isAlly;

        Destroy(projectile.gameObject, 5f); // Destroy after 5 seconds to avoid clutter
    }
}