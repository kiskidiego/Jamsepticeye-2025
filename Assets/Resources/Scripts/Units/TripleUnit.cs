using UnityEngine;

public class TripleGoblin : EnemyUnit
{
    [SerializeField] EnemyUnit _secondaryUnitPrefab1;
    [SerializeField] EnemyUnit _secondaryUnitPrefab2;
    [SerializeField] EnemyUnit _secondaryUnitPrefab3;
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
            EnemyUnit secondaryUnit1 = Instantiate(_secondaryUnitPrefab1, _secondaryUnitSpawnPoint1.position, _secondaryUnitSpawnPoint1.rotation);
            GameManager.Instance.AddEnemyUnit(secondaryUnit1);
        }
        if (_secondaryUnitPrefab2 != null)
        {
            EnemyUnit secondaryUnit2 = Instantiate(_secondaryUnitPrefab2, _secondaryUnitSpawnPoint2.position, _secondaryUnitSpawnPoint2.rotation);
            GameManager.Instance.AddEnemyUnit(secondaryUnit2);
        }
        if (_secondaryUnitPrefab3 != null)
        {
            EnemyUnit secondaryUnit3 = Instantiate(_secondaryUnitPrefab3, _secondaryUnitSpawnPoint3.position, _secondaryUnitSpawnPoint3.rotation);
            GameManager.Instance.AddEnemyUnit(secondaryUnit3);
        }
    }
}