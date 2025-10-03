using System.Collections.Generic;
using UnityEngine;

public class BarrierGenerator : BaseTower
{
    [SerializeField] int _barrierAmount = 5;
    [SerializeField] int numberBarriers = 10;
    protected override void OnPrepare()
    {
        int barriersGenerated = 0;
        GameManager manager = GameManager.Instance;

        List<BaseUnit> allies = new List<BaseUnit>(manager.alliedUnits);

        foreach (BaseUnit ally in allies)
        {
            if (ally != null && !ally.Dead)
            {
                if (ally.HasBarrier)
                {
                    continue;
                }
                
                ally.GetBarrier(_barrierAmount);
                barriersGenerated++;

                if (barriersGenerated >= numberBarriers)
                {
                    break;
                }
            }
        }
    }
}
