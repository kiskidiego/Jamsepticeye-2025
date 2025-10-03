using UnityEngine;

public class VampireAttack : BaseAttack
{
    [SerializeField] float _healingPerHit = 2.5f; // Percentage of damage dealt that is converted to health
    [SerializeField] int _bloodPerHit = 1; // Amount of blood gained per hit

    /// <summary>
    /// Executes a vampire attack, dealing damage to the target, healing the player's castle and harvesting blood.
    /// </summary>
    protected override void AttackEffect(Hittable target)
    {
        if (target == null) return;
        
        target.TakeDamage(_damage);
        GameManager.Instance.Castle.TakeDamage(-_healingPerHit); // Heal the castle (negative damage)
        GameManager.Instance.AddBlood(_bloodPerHit);
    }
}