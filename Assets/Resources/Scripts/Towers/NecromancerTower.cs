using UnityEngine;

public class NecromancerTower : BaseTower
{
    protected override void OnSell()
    {
        Debug.Log("You can't sell the main tower");
    }

    protected override void WhenDestroyed()
    {
        GameManager.Instance.EndGame();
    }
}
