using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionGoToMeleeRange : GOAPAction
{
    public GOAPActionGoToMeleeRange(Agent owner)
        : base(GOAPActionType.GOTO_MELEE_RANGE, owner)
    {

    }

    public override bool IsPreConditionSatisfied(WorldState ws)
    {
        WorldStateProp prop = ws.GetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS);
        return prop.GetBool();
    }

    public override void ApplyEffect(WorldState ws)
    {
        ws.SetWSProperty(WorldStatePropKey.IN_WEAPONS_RANGE, true);
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionGoTo agentAction = AgentActionFactory.Get(AgentActionType.GOTO_POS, Owner) as AgentActionGoTo;
        agentAction.moveType = MoveType.FORWARD;
        agentAction.motionType = MotionType.SPRINT;
        agentAction.finalPosition = GetBestAttackStart(Owner.BlackBoard.desiredTarget);
        return agentAction;        
    }

    public override bool IsValid()
    {
        return Owner.BlackBoard.desiredTarget != null;
    }

    Vector3 GetBestAttackStart(Agent target)
    {
        Vector3 dirToTarget = target.Position - Owner.Position;
        dirToTarget.Normalize();

        return target.Position - dirToTarget * Owner.BlackBoard.weaponRange;
    }
}
