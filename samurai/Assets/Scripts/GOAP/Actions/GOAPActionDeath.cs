using System;
using UnityEngine;

class GOAPActionDeath : GOAPAction
{    
    public GOAPActionDeath(Agent owner) : base(GOAPActionType.DEATH, owner)
    {
    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.EVENT);
        return prop.GetEvent() == EventTypes.DEAD;
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.NONE);
    }

    protected override AgentAction MakeAgentAction()
    {
        Debug.Log("make dead action");
        AgentActionDeath agentAction = AgentActionFactory.Get(AgentActionType.DEATH, Owner) as AgentActionDeath;
        agentAction.damageType = Owner.BlackBoard.damageType;
        agentAction.fromWeapon = Owner.BlackBoard.attackerWeapon;
        agentAction.attacker = Owner.BlackBoard.attacker;
        agentAction.impuls = Owner.BlackBoard.impuls;
        return agentAction;
    }
}
