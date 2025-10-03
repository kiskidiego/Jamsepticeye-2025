using UnityEngine;

// Base class for all towers
public abstract class BaseTower : Hittable
{
    const float SELL_CASHBACK = 0.7f; // Percentage of bodies returned when sold.
    
    [SerializeField] protected TargetingPriorities _targetingPriority;
    [SerializeField] protected int _bodiesPrice;
    [Header("Damage properties")]
    [SerializeField] protected bool _canAttack;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _attackCooldown; // Time between attacks
    protected Hittable _target;
    protected float _currentAttackCooldown; // Time left until next attack
    protected bool _paused;

    /// <summary>
    /// Tower behaviour when bought.
    /// </summary>
    protected virtual void OnBuy()
    {
        Pause();
    }
    
    /// <summary>
    /// Tower behaviour when sold.
    /// </summary>
    protected virtual void OnSell()
    {
        for(int i = 0; i < Mathf.RoundToInt(_bodiesPrice * SELL_CASHBACK * CurrentHealth / MaxHealth); i++)
        {
            GameManager.Instance.AddBody();
        }
        GameManager.Instance.RemoveTower(this);
    }

    protected virtual void WhenDestroyed()
    {
        GameManager.Instance.RemoveTower(this);
    }

    /// <summary>
    /// Tower behaviour when clicked.
    /// </summary>
    protected virtual void OnInteract()
    {
    }
    /// <summary>
    /// Pauses the tower's behavior between rounds.
    /// </summary>
    public void Pause()
    {
        _paused = true;
    }

    /// <summary>
    /// Unpauses the tower's behavior.
    /// </summary>
    public void Unpause()
    {
        _paused = false;
    }
    
    /// <summary>
    /// Checks if the tower can attack based on its attack cooldown, and attacks if possible.
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
    /// Finds a target based on the tower's targeting priority.
    /// </summary>
    
    protected virtual void FindTarget()
    {
        if (GameManager.Instance == null)
        {
            throw new System.Exception("GameManager instance is null. Cannot find target.");
        }
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
}