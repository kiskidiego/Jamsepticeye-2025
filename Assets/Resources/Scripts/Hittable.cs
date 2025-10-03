using UnityEngine;

// Base class for all objects that can be hit and take damage
public class Hittable : MonoBehaviour
{
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    [SerializeField] protected float _maxHealth = 10f;
    [SerializeField] protected float _size = .5f; //Radius of the object for range calculations
    public bool HasBarrier => _hasBarrier;
    public float BarrierHealth => _barrierHealth;
    protected float _currentHealth;
    protected bool _hasBarrier = false;
    protected float _barrierHealth;
    protected float _sizeSquared;
    

    /// <summary>
    /// Initializes the object's current health to its maximum health, and its squared size. Can be overriden by derived classes.
    /// </summary>
    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _sizeSquared = _size * _size;
    }

    public void GetBarrier(int extraHP)
    {
        _hasBarrier = true;
        _barrierHealth = extraHP;
    }

    /// <summary>
    /// Subtracts damage from the object's current health. If health drops to 0 or below, the object dies.
    /// Can also heal if damage is negative. Healing cannot exceed max health.
    /// If the object has a barrier, damage is subtracted from the barrier health first.
    /// </summary>
    /// <param name="damage">Amount to subtract from current health.</param>
    public virtual void TakeDamage(float damage)
    {
        if (_hasBarrier)
        {
            _barrierHealth -= damage;
            if (_barrierHealth <= 0)
            {
                _hasBarrier = false;
            }
            return;
        }

        _currentHealth -= damage;
        Debug.Log($"{gameObject.name}: Taking damage: {damage} Current Health: {_currentHealth}");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles the death of the object. By default, it destroys the game object. Can be overridden by derived classes for custom death behavior.
    /// </summary>
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Returns the size of the unit.
    /// </summary>
    public float GetSizeSquared()
    {
        return _sizeSquared;
    }
}