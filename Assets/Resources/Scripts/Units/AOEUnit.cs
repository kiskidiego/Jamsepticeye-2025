using System.Collections.Generic;
using UnityEngine;

public class AOEUnit : BaseUnit
{
    [SerializeField] float _aoeRadius = 5f; // Area of effect radius
    protected float _aoeRadiusSquared;

    protected override void Start()
    {
        base.Start();
        _aoeRadiusSquared = _aoeRadius * _aoeRadius;
    }
    protected override void Attack()
    {
        if (_target == null) return;

        if (_isAlly)
        {
            Hittable[] hittables = GameManager.Instance.GetAllEnemiesInRange(_target.transform.position, _aoeRadius);
            foreach (var hittable in hittables)
            {
                hittable.TakeDamage(_damage);
            }
        }
        else
        {
            Hittable[] hittables = GameManager.Instance.GetAllAlliesInRange(_target.transform.position, _aoeRadius);
            foreach (var hittable in hittables)
            {
                hittable.TakeDamage(_damage);
            }
        }
    }
}