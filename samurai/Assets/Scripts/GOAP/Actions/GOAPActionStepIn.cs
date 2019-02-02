using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionStepIn : GOAPAction
{
    public GOAPActionStepIn(Agent owner)
        :base(GOAPActionType.STEP_IN, owner)
    {
        
    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS);
        return prop.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE, true);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionCombatMove agentAction = AgentActionFactory.Get(AgentActionType.COMBAT_MOVE, Owner) as AgentActionCombatMove;
        agentAction.target = Owner.BlackBoard.desiredTarget;
        agentAction.minDistanceToTarget = 3;
        agentAction.moveType = MoveType.FORWARD;
        if (Owner.agentType == AgentType.DOUBLE_SWORDS_MAN || Owner.agentType == AgentType.SWORD_MAN) // 这里暂时偷个懒，另外扩展一个不同参数的goapaction才是更灵活的做法
        {
            agentAction.motionType =MotionType.RUN;
            agentAction.totalMoveDistance = Owner.BlackBoard.DistanceToTarget - (Owner.BlackBoard.combatRange * 0.8f);
        }
        else
        {            
            agentAction.totalMoveDistance = UnityEngine.Random.Range((Owner.BlackBoard.DistanceToTarget - (Owner.BlackBoard.combatRange * 0.5f)) * 0.5f, 
                Owner.BlackBoard.DistanceToTarget - (Owner.BlackBoard.combatRange * 0.5f));
        }        
        
        return agentAction;
    }

    public override bool IsValid()
    {
        return (Owner.BlackBoard.desiredTarget != null && Owner.BlackBoard.DistanceToTarget >= 2);
    }
}
