using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionInjury : GOAPAction
{
    public GOAPActionInjury(Agent owner)
        : base(GOAPActionType.INJURY, owner)
    {

    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.EVENT);
        return prop.GetEvent() == EventTypes.HIT;
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.NONE);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionInjury agentAction = AgentActionFactory.Get(AgentActionType.INJURY, Owner) as AgentActionInjury;
        agentAction.damageType = Owner.BlackBoard.damageType;
        agentAction.fromWeapon = Owner.BlackBoard.attackerWeapon;
        agentAction.attacker = Owner.BlackBoard.attacker;
        agentAction.impuls = Owner.BlackBoard.impuls;
        return agentAction;
    }    
}
