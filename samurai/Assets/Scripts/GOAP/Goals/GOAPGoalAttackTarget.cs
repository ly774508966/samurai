using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPGoalAttackTarget : GOAPGoal
{
    public GOAPGoalAttackTarget(Agent owner)
        : base(GOAPGoalType.ATTACK_TARGET, owner)
    {

    }
    public override bool IsSatisfied(WorldState worldState)
    {
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.TARGET_ATTACKED);
        return prop.GetBool();
    }

   /* public override void MakeSatisfied(WorldState worldState)
    {
        worldState.SetWSProperty(WorldStatePropKey.TARGET_ATTACKED, true);
    }*/

    public override void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
        Weight = 0;

        if (blackBoard.desiredTarget == null)
        {
            return;
        }

        if (blackBoard.Vigor < blackBoard.vigorAttackCost)
        {
            return;
        }

        if (blackBoard.InCombatRange == false)
        {
            return;
        }

        if (blackBoard.maxRage == 0)
        {
            return;
        }

        float attackValue = blackBoard.Rage / blackBoard.maxRage; 
        if (attackValue < 0.25f)
            return;        

        Weight = blackBoard.GOAP_AttackTargetRelevancy * attackValue;
    }
}