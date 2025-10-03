using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : BaseAttack
{
    [SerializeField] float _radius = 5f; // Radius of the area attack

    /// <summary>
    /// Executes an area attack, dealing damage to all valid targets within the specified radius of the target's position.
    /// </summary>
    protected override void AttackEffect(Hittable target)
    {
        if (target == null) return;

        if (isAlly)
        {
            List<Hittable> hittables = GameManager.Instance.GetAllEnemiesInRange(target.transform.position, _radius);
            foreach (var hittable in hittables)
            {
                hittable.TakeDamage(_damage);
            }
        }
        else
        {
            List<Hittable> hittables = GameManager.Instance.GetAllAlliesInRange(target.transform.position, _radius);
            foreach (var hittable in hittables)
            {
                hittable.TakeDamage(_damage);
            }
        }
    }
}