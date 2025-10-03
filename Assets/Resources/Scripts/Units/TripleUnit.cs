using UnityEngine;

public class TripleUnit : BaseUnit
{
    [SerializeField] BaseUnit _secondaryUnitPrefab1;
    [SerializeField] BaseUnit _secondaryUnitPrefab2;
    [SerializeField] BaseUnit _secondaryUnitPrefab3;
    [SerializeField] Transform _secondaryUnitSpawnPoint1;
    [SerializeField] Transform _secondaryUnitSpawnPoint2;
    [SerializeField] Transform _secondaryUnitSpawnPoint3;

    protected override void Die()
    {
        if (_dead)
        {
            Debug.LogWarning($"{gameObject.name} is already dead. Die() called again.");
            return;
        }
        base.Die();
        Debug.Log("Spawning secondary units");
        if (_secondaryUnitPrefab1 != null)
        {
            BaseUnit secondaryUnit1 = Instantiate(_secondaryUnitPrefab1, _secondaryUnitSpawnPoint1.position, _secondaryUnitSpawnPoint1.rotation);
            if (_isAlly)
            {
                GameManager.Instance.AddAllyUnit(secondaryUnit1);
            }
            else
            {
                GameManager.Instance.AddEnemyUnit(secondaryUnit1);
            }
        }
        if (_secondaryUnitPrefab2 != null)
            {
                BaseUnit secondaryUnit2 = Instantiate(_secondaryUnitPrefab2, _secondaryUnitSpawnPoint2.position, _secondaryUnitSpawnPoint2.rotation);
                if (_isAlly)
                {
                    GameManager.Instance.AddAllyUnit(secondaryUnit2);
                }
                else
                {
                    GameManager.Instance.AddEnemyUnit(secondaryUnit2);
                }
            }
            if (_secondaryUnitPrefab3 != null)
            {
                BaseUnit secondaryUnit3 = Instantiate(_secondaryUnitPrefab3, _secondaryUnitSpawnPoint3.position, _secondaryUnitSpawnPoint3.rotation);
                if (_isAlly)
                {
                    GameManager.Instance.AddAllyUnit(secondaryUnit3);
                }
                else
                {
                    GameManager.Instance.AddEnemyUnit(secondaryUnit3);
                }
            }
    }
}