using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionAttackMelee : AgentAction
{
    public Agent target;
    public AnimAttackData data;
    public AttackType attackType;
    public Vector3 attackDir;
    public bool hit;
    public bool attackPhaseDone;    

    public override void Reset(Agent agent)
    {
        base.Reset(agent);
        target = null;
        hit = false;
        attackPhaseDone = false;
        data = null;
        attackType = AttackType.None;
    }
    public override string ToString() { return "HumanActionAttack " + (target != null ? target.name : "no target") + " " + attackType.ToString(); }

    public AgentActionAttackMelee(Agent owner) : base(AgentActionType.ATTACK_MELEE, owner)
    {
        Reset(owner);
    }

    public void Initialize(/*Agent owner*/)
    {
        if (attackType == AttackType.None)
        {
            return;
        }

        target = Owner.transform.GetComponent<BlackBoard>().desiredTarget;//Owner.transform.GetComponent<PlayerEnemyDecision>().GetBestTarget();
        if (target == null)
        {
            attackDir = Owner.transform.forward;
        }

        ComboMgr comboMgr = Owner.transform.GetComponent<ComboMgr>();
        if (comboMgr != null)
        {
            if (target != null && target.BlackBoard.IsKnockedDown)
            {
                data = Owner.AnimSet.GetFirstAttackAnim(Owner.BlackBoard.weaponSelected, AttackType.Fatality);
                attackType = AttackType.Fatality;
                comboMgr.Reset();
            }
            else
                data = comboMgr.ProcessCombo(attackType);
        }
        else
        {
            data = Owner.GetComponent<AnimSet>().GetFirstAttackAnim(Owner.GetComponent<BlackBoard>().weaponSelected, attackType);
        }        
    }
}
