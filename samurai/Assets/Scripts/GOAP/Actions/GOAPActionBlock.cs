using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionBlock : GOAPAction
{
    public GOAPActionBlock(Agent owner) : base(GOAPActionType.BLOCK, owner) { }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS);
        return prop.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.IN_BLOCK, true);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionBlock agentAction = AgentActionFactory.Get(AgentActionType.BLOCK, Owner) as AgentActionBlock;        
        agentAction.attacker = Owner.BlackBoard.desiredTarget;
        agentAction.time = 3.0f;
        return agentAction;
    }

    public override bool IsValid()
    {
        return (Owner.BlackBoard.desiredTarget != null && Owner.BlackBoard.DistanceToTarget <= 4);
    }
}
