using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPGoalStepIn : GOAPGoal
{
    public GOAPGoalStepIn(Agent owner)
        : base(GOAPGoalType.STEP_IN, owner)
    {

    }
    public override bool IsSatisfied(WorldState worldState)
    {
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE);        
        return prop.GetBool();
    }

    /*public override void MakeSatisfied(WorldState worldState)
    {
        worldState.SetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE, true);        
    }*/

    public override void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
        Weight = 0;

        if (blackBoard.desiredTarget == null)
        {
            return;
        }

        if (blackBoard.InCombatRange == true)
        {
            return;
        }

        Weight = Mathf.Min(blackBoard.GOAP_StepInRelevancy, (blackBoard.DistanceToTarget - blackBoard.combatRange) * 0.2f);
    }
}
