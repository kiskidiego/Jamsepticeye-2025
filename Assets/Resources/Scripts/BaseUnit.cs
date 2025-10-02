using UnityEngine;

// Base class for all units, enemy and ally
public abstract class BaseUnit : Hittable
{

    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed; // Attacks per second
    [SerializeField] protected float _movementSpeed;
    [SerializeField] protected float _range;
    [SerializeField] protected TargetingPriorities _targetingPriority;
    protected Hittable _target;
    protected float _rangeSquared;
    protected float _attackCooldown; // Time between attacks
    protected float _currentAttackCooldown; // Time left until next attack

    /// <summary>
    /// Initializes the unit's current health, its squared size, its squared range and its attack cooldown. Can be overriden by derived classes.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        _rangeSquared = _range * _range;
        _attackCooldown = 1f / _attackSpeed;
    }
    /// <summary>
    /// Default Unit behavior. Finds a target and tries to attack it if in range. Can be overriden by derived classes.
    /// </summary>
    protected virtual void Update()
    {
        if (_target == null)
        {
            FindTarget();
            return;
        }
        if (Vector3.SqrMagnitude(transform.position - _target.transform.position) - _sizeSquared - _target.GetSizeSquared() > _rangeSquared)
        {
            MoveToTarget();
        }
        else
        {
            CheckAttack();
        }
    }

    /// <summary>
    /// Moves the unit towards its target. Can be overridden by derived classes for custom movement behavior.
    /// </summary>
    protected virtual void MoveToTarget()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _movementSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Checks if the unit can attack based on its attack cooldown, and attacks if possible.
    /// </summary>
    protected void CheckAttack()
    {
        if (_currentAttackCooldown <= 0f)
        {
            Attack();
            _currentAttackCooldown = _attackCooldown;
        }
        else
        {
            _currentAttackCooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Attacks the current target, dealing damage. Can be overridden by derived classes for custom attack behavior.
    /// </summary>
    protected virtual void Attack()
    {
        if (_target != null)
        {
            _target.TakeDamage(_damage);
        }
    }

    /// <summary>
    /// Finds a target based on the unit's targeting priority. To be implemented.
    /// </summary>
    protected abstract void FindTarget();
}
