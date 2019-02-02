using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPGoalStepAround : GOAPGoal
{
    public GOAPGoalStepAround(Agent owner)
        : base(GOAPGoalType.STEP_AROUND, owner)
    {

    }
    public override bool IsSatisfied(WorldState worldState)
    {
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.MOVE_AROUND);
        return prop.GetBool();
    }

    /*public override void MakeSatisfied(WorldState worldState)
    {        
        worldState.SetWSProperty(WorldStatePropKey.MOVE_AROUND, true);
    }*/

    public override void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
        Weight = 0;

        if (blackBoard.desiredTarget == null)
        {
            return;
        }

        if (blackBoard.InCombatRange == false || blackBoard.InWeaponRange)
        {
            return;
        }

        Weight = blackBoard.GOAP_StepAroundRelevancy;
    }
}