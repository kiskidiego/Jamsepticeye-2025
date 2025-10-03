using FMODUnity;
using UnityEngine;

// Base class for all towers
public abstract class BaseTower : Hittable
{
    const float SELL_CASHBACK = 0.7f; // Percentage of bodies returned when sold.
    
    [SerializeField] protected TargetingPriorities _targetingPriority;
    [SerializeField] protected int _bodiesPrice;
    [SerializeField] EventReference _placeSound;
    [SerializeField] EventReference _interactSound;
    [Header("Damage properties")]
    protected bool _paused;

    /// <summary>
    /// Tower behaviour when bought.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        AudioManager.instance.PlayOneShot(_placeSound, transform.position);
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

    protected override void Die()
    {
        AudioManager.instance.PlayOneShot(_deathSound, transform.position);
        GameManager.Instance.RemoveTower(this);
    }

    /// <summary>
    /// Tower behaviour when clicked.
    /// </summary>
    protected virtual void OnInteract()
    {
        if (_paused) return;
        AudioManager.instance.PlayOneShot(_interactSound, transform.position);
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
}