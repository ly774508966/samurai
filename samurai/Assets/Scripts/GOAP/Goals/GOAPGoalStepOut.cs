using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPGoalStepOut : GOAPGoal
{
    public GOAPGoalStepOut(Agent owner)
        : base(GOAPGoalType.STEP_OUT, owner)
    {

    }
    public override bool IsSatisfied(WorldState worldState)
    {
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE);        
        return prop.GetBool() == false;
    }

    /* public override void MakeSatisfied(WorldState worldState)
     {
         worldState.SetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE, false);        
     }*/

    public override void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
        Weight = 0;

        if (blackBoard.desiredTarget == null)
        {
            return;
        }

        if (blackBoard.Fear < blackBoard.maxFear * 0.25f)
            return;

        if (blackBoard.InCombatRange == false)
        {
            return;
        }

        Weight = blackBoard.GOAP_StepOutRelevancy * (blackBoard.Fear * 0.01f);

        if (blackBoard.DistanceToTarget < 2.0f)
            Weight += 0.2f;

        if (blackBoard.AheadOfTarget == false)
        {
            Weight *= 0.5f;
        }                

        if (Weight > blackBoard.GOAP_StepOutRelevancy)
            Weight = blackBoard.GOAP_StepOutRelevancy;       
    }
}