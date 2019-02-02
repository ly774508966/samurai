using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPActionAttackTwoSwords : GOAPAction
{	
    int NumberOfAttacks;
    AttackType CurrentAttacktype;

    public GOAPActionAttackTwoSwords(Agent owner)
        : base(GOAPActionType.ATTACK_MELEE_TWO_SWORDS, owner)
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

    public override void Activate()
    {        
        CurrentAttacktype = AttackType.X;
        NumberOfAttacks = UnityEngine.Random.Range(2, 4);
        base.Activate();
    }

    public override void UpdateGOAPAction()
    {
        if ((AgentAction as AgentActionAttackMelee).attackPhaseDone && NumberOfAttacks > 0)
        {
            if (CurrentAttacktype == AttackType.X)
                CurrentAttacktype = AttackType.O;
            else
                CurrentAttacktype = AttackType.X;

            //Owner.SoundPlayPrepareAttack();
            if (AgentAction != null)
            {
                AgentAction.GoapAction = null;
                AgentAction = MakeAgentAction();
                AgentAction.GoapAction = this;
                Owner.AddAction(AgentAction);
            }            
        }
    }

    protected override AgentAction MakeAgentAction()
    {
        AgentActionAttackMelee agentAction = AgentActionFactory.Get(AgentActionType.ATTACK_MELEE, Owner) as AgentActionAttackMelee;
        agentAction.attackType = CurrentAttacktype;
/*
        if (Owner.BlackBoard.desiredTarget)
        {
            Owner.BlackBoard.desiredDirection = Owner.BlackBoard.desiredTarget.Position - Owner.Position;
            Owner.BlackBoard.desiredDirection.Normalize();
            agentAction.attackDir = Owner.BlackBoard.desiredDirection;
        }
        else
            agentAction.attackDir = Owner.Forward;*/

        agentAction.hit = false;
        agentAction.attackPhaseDone = false;
        --NumberOfAttacks;
        
        return agentAction;
    }

    public override bool IsValid()
    {
        return Owner.BlackBoard.desiredTarget != null;
    }

    /*Vector3 GetBestAttackStart(Agent target)
    {
        Vector3 dirToTarget = target.Position - Owner.Position;
        dirToTarget.Normalize();

        return target.Position - dirToTarget * Owner.BlackBoard.weaponRange;
    }*/
}
