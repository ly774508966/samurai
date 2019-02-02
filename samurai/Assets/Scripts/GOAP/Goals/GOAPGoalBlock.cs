using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPGoalBlock : GOAPGoal
{
    public GOAPGoalBlock(Agent owner)
        : base(GOAPGoalType.BLOCK, owner)
    {

    }
    public override bool IsSatisfied(WorldState worldState)
    {
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.IN_BLOCK);
        return prop.GetBool();
    }

    /* public override void MakeSatisfied(WorldState worldState)
     {
         worldState.SetWSProperty(WorldStatePropKey.IN_BLOCK, true);
     }*/

    public override void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
        Weight = 0;        
        if (blackBoard.desiredTarget == null)
            return;

        //if (blackBoard.dodge < blackBoard.dodgeMax * 0.25f)
          //  return;

        if (blackBoard.desiredTarget != null && blackBoard.DistanceToTarget > 3.5f)
            return;
        
        WorldStateProp prop3 = worldState.GetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE);
        WorldStateProp prop4 = worldState.GetWSProperty(WorldStatePropKey.AHEAD_OF_ENEMY);        

        if (blackBoard.InCombatRange && blackBoard.AheadOfTarget)
            Weight = blackBoard.GOAP_BlockRelevancy /** blackBoard.dodge * 0.01f*/;
    }
}
