using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.UI;

// Base class for all units, enemy and ally
public class BaseUnit : Hittable
{
    public int BodyReward => _bodyReward;
    public int BloodReward => _bloodReward;
    public bool Dead => _dead;
    [SerializeField] protected float _movementSpeed;
    [SerializeField] protected float _cooldownBetweenAttacks = 0.2f;
    [SerializeField] int _bodyReward;
    [SerializeField] int _bloodReward;
    [SerializeField] protected bool _isAlly; // True if the unit is an ally, false if it's an enemy
    protected float _slowMultiplier = 1f; // Multiplier for movement speed when slowed
    protected float _hastenMultiplier = 1f; // Multiplier for movement speed when hastened
    protected Hittable _target;
    protected bool _paused = true;
    protected bool _dead = false;
    protected float _currentAttackCooldown;
    protected int currentAttackIndex = 0;
    protected List<BaseAttack> _attacks;

    /// <summary>
    /// Initializes the unit's current health and its squared size, as well as its attacks. Can be overriden by derived classes.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        _attacks = new List<BaseAttack>(GetComponents<BaseAttack>());
        foreach (var attack in _attacks)
        {
            attack._isAlly = _isAlly;
        }
    }
    /// <summary>
    /// Default Unit behavior. Finds a target and tries to attack it if in range. Can be overriden by derived classes.
    /// </summary>
    void Update()
    {
        if (_paused) return;

        if (_dead) return;

        if (_target == null || (_target is BaseUnit && (_target as BaseUnit).Dead))
        {
            FindTarget();
            return;
        }
        if (_attacks.Count == 0)
        {
            return;
        }
        if (Vector3.SqrMagnitude(transform.position - _target.transform.position) - _sizeSquared - _target.GetSizeSquared() > _attacks[currentAttackIndex].RangeSqr)
        {
            MoveToTarget();
        }
        else if (_currentAttackCooldown <= 0f)
        {
            CheckAttack();
        }
        _currentAttackCooldown -= Time.deltaTime * _hastenMultiplier * _slowMultiplier;
    }

    /// <summary>
    /// Moves the unit towards its target. Can be overridden by derived classes for custom movement behavior.
    /// </summary>
    void MoveToTarget()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _movementSpeed * Time.deltaTime * _slowMultiplier * _hastenMultiplier);
            transform.LookAt(_target.transform);
        }
    }

    /// <summary>
    /// Checks if the unit can attack based on its attack cooldown, and attacks if possible.
    /// </summary>
    void CheckAttack()
    {
        if (_attacks[currentAttackIndex].CanAttack)
        {
            _attacks[currentAttackIndex].Attack(_target);
            _target = null; // Reset target after attack to find a new one next frame
            _currentAttackCooldown = _cooldownBetweenAttacks;
        }
    }

    /// <summary>
    /// Picks the attack with the lowest cooldown and assigns it as the current attack, then finds a target for it.
    /// </summary>
    void FindTarget()
    {
        float minAttackCooldown = float.MaxValue;
        for (int i = 0; i < _attacks.Count; i++)
        {
            var attack = _attacks[i];
            if (attack.CurrentAttackCooldown < minAttackCooldown)
            {
                minAttackCooldown = attack.CurrentAttackCooldown;
                _target = attack.GetTarget(transform.position, this);
                currentAttackIndex = i;
            }
        }
    }

    /// <summary>
    /// Handles the unit's death, marking it as dead. Can be overridden by derived classes for custom death behavior.
    /// </summary>
    protected override void Die()
    {
        if (_dead)
        {
            Debug.LogWarning($"{gameObject.name} is already dead. Die() called again.");
        }
        Debug.Log($"{gameObject.name} has died.");
        _dead = true;
    }

    /// <summary>
    /// Pauses the unit's behavior between rounds.
    /// </summary>
    public void Pause()
    {
        _paused = true;
        foreach (var attack in _attacks)
        {
            attack.paused = true;
        }
    }

    /// <summary>
    /// Unpauses the unit's behavior.
    /// </summary>
    public void Unpause()
    {
        _paused = false;
        foreach (var attack in _attacks)
        {
            attack.paused = false;
        }
    }

    /// <summary>
    /// Resets the unit's state for reuse.
    /// </summary>
    public void Reset()
    {
        _currentHealth = _maxHealth;
        _target = null;
        _slowMultiplier = 1f;
        _hastenMultiplier = 1f;
        foreach (var attack in _attacks)
        {
            attack.paused = true;
            attack._slowMultiplier = 1f;
            attack._hastenMultiplier = 1f;
        }
    }

    /// <summary>
    /// Applies a slow effect to the unit and its attacks for a specified duration. Multiplicative with other slows.
    /// </summary>
    /// <param name="slowAmount"></param>
    /// <param name="duration"></param>
    public void ApplySlow(float slowAmount, float duration)
    {
        _slowMultiplier *= (100f - slowAmount) / 100f;
        foreach (var attack in _attacks)
        {
            attack.ApplySlow(slowAmount);
        }
        StartCoroutine(RemoveSlowAfterDuration(duration, slowAmount));
    }

    /// <summary>
    /// Applies a hasten effect to the unit and its attacks for a specified duration. Additive with other hasten effects.
    /// </summary>
    /// <param name="hastenAmount"></param>
    /// <param name="duration"></param>
    public void ApplyHasten(float hastenAmount, float duration)
    {
        _hastenMultiplier += hastenAmount / 100f;
        foreach (var attack in _attacks)
        {
            attack.ApplyHasten(hastenAmount);
        }
        StartCoroutine(RemoveHastenAfterDuration(duration, hastenAmount));
    }
    IEnumerator RemoveSlowAfterDuration(float duration, float slowAmount)
    {
        yield return new WaitForSeconds(duration);
        _slowMultiplier /= (100f - slowAmount) / 100f;
        foreach (var attack in _attacks)
        {
            attack.RemoveSlow(slowAmount);
        }
    }
    IEnumerator RemoveHastenAfterDuration(float duration, float hastenAmount)
    {
        yield return new WaitForSeconds(duration);
        _hastenMultiplier -= hastenAmount / 100f;
        foreach (var attack in _attacks)
        {
            attack.RemoveHasten(hastenAmount);
        }
    }
}
