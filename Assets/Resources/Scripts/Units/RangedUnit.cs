using UnityEditor.SearchService;
using UnityEngine;

public class RangedUnit : BaseUnit
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] Transform _projectileSpawnPoint;

    /// <summary>
    /// Launches a projectile towards the target.
    /// </summary>
    protected override void Attack()
    {
        if (_target == null) return;

        AudioManager.instance.PlayOneShot(_attackSound, transform.position);
        
        // Instantiate and launch the projectile towards the target
        Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
        projectile.target = _target.transform;
        projectile.targetSize = _target.GetSize();
        projectile.SetDamage(_damage);

        Destroy(projectile.gameObject, 5f); // Destroy after 5 seconds to avoid clutter
    }
}
