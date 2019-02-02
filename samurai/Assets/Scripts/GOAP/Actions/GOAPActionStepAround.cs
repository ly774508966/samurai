using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionStepAround : GOAPAction
{
    public GOAPActionStepAround(Agent owner)
        : base(GOAPActionType.STEP_AROUND, owner)
    {

    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS);
        return prop.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.MOVE_AROUND, true);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionCombatMove agentAction = AgentActionFactory.Get(AgentActionType.COMBAT_MOVE, Owner) as AgentActionCombatMove;
        agentAction.moveType = UnityEngine.Random.Range(0, 2) == 0 ? MoveType.LEFTWARD : MoveType.RIGHTWARD;
        agentAction.target = Owner.BlackBoard.desiredTarget;
        agentAction.totalMoveDistance = UnityEngine.Random.Range(2.0f, 4.0f);
        agentAction.minDistanceToTarget = Owner.BlackBoard.DistanceToTarget * 0.7f;
        return agentAction;
    }

    public override bool IsValid()
    {
        return Owner.BlackBoard.desiredTarget != null;
    }
}
