using System;
using UnityEngine;

class GOAPActionKnockdown : GOAPAction
{        
    public GOAPActionKnockdown(Agent owner) : base(GOAPActionType.KNOCKDOWN, owner) { }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.EVENT);
        return prop.GetEvent() == EventTypes.KNOCKDOWN;
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.NONE);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionKnockdown agentAction = AgentActionFactory.Get(AgentActionType.KNOCKDOWN, Owner) as AgentActionKnockdown;
        agentAction.fromWeapon = Owner.BlackBoard.attackerWeapon;
        agentAction.attacker = Owner.BlackBoard.attacker;
        agentAction.impuls = Owner.BlackBoard.impuls;
        agentAction.time = Owner.BlackBoard.maxKnockdownTime * UnityEngine.Random.Range(0.7f, 1);
        return agentAction;
    }
}
