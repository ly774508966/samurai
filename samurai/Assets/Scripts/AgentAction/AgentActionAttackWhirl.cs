using UnityEngine;

public class AgentActionAttackWhirl : AgentAction
{
    public AnimAttackData data;

    public AgentActionAttackWhirl(Agent owner) : base(AgentActionType.ATTACK_WHIRL, owner)
    {
    }    
}
