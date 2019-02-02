using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionAttackMeleeOnce : GOAPAction
{
    public GOAPActionAttackMeleeOnce(Agent owner)
        : base(GOAPActionType.ATTACK_MELEE_ONCE, owner)
    {

    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop1 = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS); 
        WorldStateProp prop2 = ws.GetWSProperty(WorldStatePropKey.IN_WEAPONS_RANGE);
        return prop1.GetBool() && prop2.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.TARGET_ATTACKED, true);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionAttackMelee agentAction = AgentActionFactory.Get(AgentActionType.ATTACK_MELEE, Owner) as AgentActionAttackMelee;
        if (Owner.BlackBoard.IsBlocking)
        {
            agentAction.attackType = AttackType.Counter;  // 反击
        }
        else
        {
            agentAction.attackType = AttackType.X;
        }        
        agentAction.hit = false;
        agentAction.attackPhaseDone = false;
        return agentAction;
    }

    public override bool IsValid()
    {
        return Owner.BlackBoard.desiredTarget != null;
    }

  /*  Vector3 GetBestAttackStart(Agent target)
    {
        Vector3 dirToTarget = target.Position - Owner.Position;
        dirToTarget.Normalize();

        return target.Position - dirToTarget * Owner.BlackBoard.weaponRange;
    }*/
}
