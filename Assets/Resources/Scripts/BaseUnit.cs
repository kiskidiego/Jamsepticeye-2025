using UnityEngine;

// Base class for all units, enemy and ally
public abstract class BaseUnit : Hittable
{
    
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _movementSpeed;
    [SerializeField] protected float _range;
    [SerializeField] protected float _size;
    [SerializeField] protected TargetingPriorities _targetingPriority;
    protected Hittable target;

    /// <summary>
    /// Default Unit behavior. Finds a target and attacks it if in range. Can be overriden by derived classes.
    /// </summary>
    protected virtual void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        if (Vector3.SqrMagnitude(transform.position - target.transform.position) > _range * _range)
        {
            MoveToTarget();
        }
        else
        {
            Attack();
        }
    }

    /// <summary>
    /// Moves the unit towards its target. Can be overridden by derived classes for custom movement behavior.
    /// </summary>
    protected virtual void MoveToTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _movementSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Attacks the current target, dealing damage. Can be overridden by derived classes for custom attack behavior.
    /// </summary>
    protected virtual void Attack()
    {
        if (target != null)
        {
            target.TakeDamage(_damage);
        }
    }

    /// <summary>
    /// Finds a target based on the unit's targeting priority. To be implemented.
    /// </summary>
    protected abstract void FindTarget();
}
