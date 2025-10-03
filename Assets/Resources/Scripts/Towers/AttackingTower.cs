using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AttackingTower : BaseTower
{
    [SerializeField] protected float _cooldownBetweenAttacks; // Time between attacks
    float _currentAttackCooldown; // Time left until next attack
    readonly List<BaseAttack> _attacks = new List<BaseAttack>();
    protected override void Start()
    {
        base.Start();
        _attacks.AddRange(GetComponents<BaseAttack>());
        foreach (BaseAttack attack in _attacks)
        {
            attack.isAlly = true;
        }
        _currentAttackCooldown = _cooldownBetweenAttacks;
    }

    void Update()
    {
        if (_paused) return;

        if (_currentAttackCooldown > 0f)
        {
            _currentAttackCooldown -= Time.deltaTime;
        }
        else
        {
            foreach (BaseAttack attack in _attacks)
            {
                CheckAttack(attack);
                _currentAttackCooldown = _cooldownBetweenAttacks;
                break;
            }
        }
    }

    protected void CheckAttack(BaseAttack attack)
    {
        if (attack == null) return;
        if (attack.CanAttack)
        {
            attack.Attack(attack.GetTarget(transform.position, null));
        }
    }
}