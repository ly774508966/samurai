using UnityEngine;

class GOAPActionAttackWhirl : GOAPAction
{
    public GOAPActionAttackWhirl(Agent owner) : base(GOAPActionType.ATTACK_WHIRL, owner) { }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS);        
        return prop.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.TARGET_ATTACKED, true);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionAttackWhirl agentAction = AgentActionFactory.Get(AgentActionType.ATTACK_WHIRL, Owner) as AgentActionAttackWhirl;
        agentAction.data = Owner.AnimSet.GetWhirlAttackAnim();
        return agentAction;
    }

}