using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionStepOut : GOAPAction
{
    public GOAPActionStepOut(Agent owner)
        : base(GOAPActionType.STEP_OUT, owner)
    {

    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS);
        return prop.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE, false);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionCombatMove agentAction = AgentActionFactory.Get(AgentActionType.COMBAT_MOVE, Owner) as AgentActionCombatMove;
        agentAction.moveType = MoveType.BACKWARD;
        agentAction.target = Owner.BlackBoard.desiredTarget;
        agentAction.minDistanceToTarget = 0;
        if (Owner.agentType == AgentType.DOUBLE_SWORDS_MAN) // 这里暂时偷个懒，另外扩展一个不同参数的goapaction才是更灵活的做法
        {
            agentAction.motionType = MotionType.RUN;
            agentAction.totalMoveDistance = UnityEngine.Random.Range(4f, 6f);
        }
        else
        {            
            agentAction.totalMoveDistance = UnityEngine.Random.Range(2f, 5f);
        }            
        
        return agentAction;
    }

    public override bool IsValid()
    {
        return Owner.BlackBoard.desiredTarget != null;
    }
}
