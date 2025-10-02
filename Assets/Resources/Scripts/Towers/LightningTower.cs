using UnityEngine;

public class LightningTower : BaseTower
{
    void Update()
    {
        if (_paused) return;

        if (_target == null || (_target is BaseUnit && (_target as BaseUnit).Dead))
        {
            FindTarget();
            return;
        }

        CheckAttack();
    }
}
