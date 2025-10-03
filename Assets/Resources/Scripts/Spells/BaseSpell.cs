using FMODUnity;
using UnityEngine;

public abstract class BaseSpell : MonoBehaviour
{
    public Price Cost => _castingCost;
    [SerializeField] Price _castingCost;
    [SerializeField] float _cooldown = 20f; // Cooldown time in seconds
    [SerializeField] EventReference _spellSound;
    float _currentCooldown; // Time left until the spell can be cast again
    void Update()
    {
        if (_currentCooldown > 0f)
            _currentCooldown -= Time.deltaTime;
    }
    public void CastSpell(Vector3 targetPosition)
    {
        if (_currentCooldown > 0f) return;
        if( GameManager.Instance.GetBlood() < _castingCost.bloodPrice || GameManager.Instance.GetBodies() < _castingCost.bodyPrice)
            return;

        GameManager.Instance.RemoveBodies(_castingCost.bodyPrice);
        GameManager.Instance.AddBlood(-_castingCost.bloodPrice);

        Effect(targetPosition);
        _currentCooldown = _cooldown;
        AudioManager.instance.PlayOneShot(_spellSound, targetPosition);
    }
    protected abstract void Effect(Vector3 targetPosition);
}