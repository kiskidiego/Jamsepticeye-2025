using UnityEngine;

// Base class for all objects that can be hit and take damage
public abstract class Hittable : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _currentHealth;

    /// <summary>
    /// Initializes the object's current health to its maximum health. Can be overriden by derived classes.
    /// </summary>
    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
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
    /// Handles the death of the object. To be implemented by derived classes.
    /// </summary>
    protected abstract void Die();
}