using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEditor.UI;
using FMODUnity;

// Base class for all units, enemy and ally
public class AllyUnit : BaseUnit
{
    [HideInInspector] public Cemetery _cemetery;
    [HideInInspector] public float spawnRadius = 5f;
    [SerializeField] public readonly AllyUnitsEnum unitType; // Type of the unit, used for identifying it
    [SerializeField] EventReference _spawnSound;

    protected override void Start()
    {
        base.Start();

        Debug.Log($"AllyUnit spawned: {unitType}");

        AudioManager.instance.PlayOneShot(_spawnSound, transform.position);
        transform.DOMove(_cemetery.UnitSpawnPoint.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        ), _spawnTweenDuration).SetEase(Ease.InOutCubic);
    }
    void OnDestroy()
    {
        if (_cemetery == null)
        {
            return;
        }
        else
        {
            _cemetery.RemoveUnit(this);
        }
    }

    /// <summary>
    /// Resets the unit's state for reuse.
    /// </summary>
    public void Reset()
    {
        if (_dead) return;

        if (_cemetery == null)
        {
            _dead = true;
            return;
        }

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
        transform.DOMove(_cemetery.UnitSpawnPoint.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        ), _spawnTweenDuration).SetEase(Ease.InOutCubic);
    }
    public void ChangeCemetery(Cemetery newCemetery)
    {
        if (_cemetery != null)
        {
            _cemetery.RemoveUnit(this);
        }
        newCemetery.AddUnit(this);
        Reset();
    }
}
