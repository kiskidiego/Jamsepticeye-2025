using UnityEngine;

// Base class for all units, enemy and ally
public class BaseUnit : Hittable
{
    public int BodyReward => _bodyReward;
    public int BloodReward => _bloodReward;
    public bool Dead => _dead;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed; // Attacks per second
    [SerializeField] protected float _movementSpeed;
    [SerializeField] protected float _range;
    [SerializeField] protected TargetingPriorities _targetingPriority;
    [SerializeField] int _bodyReward;
    [SerializeField] int _bloodReward;
    [SerializeField] bool _isAlly; // True if the unit is an ally, false if it's an enemy
    protected Hittable _target;
    protected float _rangeSquared;
    protected float _attackCooldown; // Time between attacks
    protected float _currentAttackCooldown; // Time left until next attack
    bool _paused = true;
    bool _dead = false;

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
        if (_paused) return;

        if (_dead) return;

        if (_target == null || (_target is BaseUnit && (_target as BaseUnit).Dead))
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
            transform.LookAt(_target.transform);
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
    /// Finds a target based on the unit's targeting priority.
    /// </summary>
    protected virtual void FindTarget()
    {
        if (GameManager.Instance == null)
        {
            throw new System.Exception("GameManager instance is null. Cannot find target.");
        }
        if (_isAlly)
        {
            switch (_targetingPriority)
            {
                case TargetingPriorities.Units:
                    _target = GameManager.Instance.GetClosestEnemy(transform.position);
                    break;
                case TargetingPriorities.Towers:
                    throw new System.Exception("Allies cannot target towers.");
                case TargetingPriorities.Castle:
                    throw new System.Exception("Allies cannot target the castle.");
                case TargetingPriorities.HighestHealth:
                    _target = GameManager.Instance.GetHighestHealthEnemy();
                    break;
                default:
                    throw new System.Exception("Invalid targeting priority.");
            }
        }
        else
        {
            switch (_targetingPriority)
            {
                case TargetingPriorities.Units:
                    _target = GameManager.Instance.GetClosestAllyUnit(transform.position);
                    break;
                case TargetingPriorities.Towers:
                    _target = GameManager.Instance.GetClosestTower(transform.position);
                    break;
                case TargetingPriorities.Castle:
                    _target = GameManager.Instance.Castle;
                    break;
                case TargetingPriorities.HighestHealth:
                    _target = GameManager.Instance.GetHighestHealthAllyUnit(transform.position);
                    break;
                default:
                    throw new System.Exception("Invalid targeting priority.");
            }
        }
    }

    protected override void Die()
    {
        _dead = true;
    }

    /// <summary>
    /// Pauses the unit's behavior between rounds.
    /// </summary>
    public void Pause()
    {
        _paused = true;
    }

    /// <summary>
    /// Unpauses the unit's behavior.
    /// </summary>
    public void Unpause()
    {
        _paused = false;
    }

    /// <summary>
    /// Resets the unit's state for reuse.
    /// </summary>
    public void Reset()
    {
        _currentHealth = _maxHealth;
        _target = null;
        _currentAttackCooldown = 0f;
    }
}
