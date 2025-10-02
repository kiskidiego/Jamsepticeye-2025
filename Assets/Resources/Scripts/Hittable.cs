using UnityEngine;

// Base class for all objects that can be hit and take damage
public abstract class Hittable : MonoBehaviour
{
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected float _size; //Radius of the object for range calculations
    protected float _sizeSquared;

    /// <summary>
    /// Initializes the object's current health to its maximum health, and its squared size. Can be overriden by derived classes.
    /// </summary>
    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _sizeSquared = _size * _size;
    }

    /// <summary>
    /// Subtracts damage from the object's current health. If health drops to 0 or below, the object dies.
    /// Can also heal if damage is negative. Healing cannot exceed max health.
    /// </summary>
    /// <param name="damage">Amount to subtract from current health.</param>
    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
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