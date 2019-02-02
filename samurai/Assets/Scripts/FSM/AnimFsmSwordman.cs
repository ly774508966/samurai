using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmSwordman : Fsm
{
    enum FsmStateType
    {
        IDLE = 0,
        GOTO_POS,
        COMBAT_MOVE,
        ATTACK_MELEE,
        INJURY,
        DEATH,
        KNOCKDOWN,
        BLOCK,
        MAX
    }

    BlackBoard _blackBoard;

    void Awake()
    {
        _blackBoard = transform.GetComponent<BlackBoard>();

        // 一定要按枚举顺序加入, 以便于按枚举值索引
        AnimFsmStateIdle idleState = new AnimFsmStateIdle(GetComponent<Agent>());
        AddState(idleState);
        AnimFsmStateGoTo gotoState = new AnimFsmStateGoTo(GetComponent<Agent>());
        AddState(gotoState);
        AnimFsmStateCombatMove combatMoveState = new AnimFsmStateCombatMove(GetComponent<Agent>());
        AddState(combatMoveState);
        AnimFsmStateAttackMelee attackMeleeState = new AnimFsmStateAttackMelee(GetComponent<Agent>());
        AddState(attackMeleeState);
        AnimFsmStateInjury injuryState = new AnimFsmStateInjury(GetComponent<Agent>());
        AddState(injuryState);
        AnimFsmStateDeath deathState = new AnimFsmStateDeath(GetComponent<Agent>());
        AddState(deathState);
        AnimFsmStateKnockdown knowdownState = new AnimFsmStateKnockdown(GetComponent<Agent>());
        AddState(knowdownState);
        AnimFsmStateBlock blockState = new AnimFsmStateBlock(GetComponent<Agent>());
        AddState(blockState);
        CurState = DefState = idleState;
    }

    void Start()
    {
        CurState.Enter(null);
    }

    protected override void DoAction(AgentAction action)
    {
        if (action == null)
        {
            return;
        }

        if (CurState.HandleSameAction(action) == true)
        {
            return;
        }

        FsmState preState = CurState;

        if (action is AgentActionGoTo)
            CurState = GetState((int)FsmStateType.GOTO_POS);
        else if (action is AgentActionCombatMove)
            CurState = GetState((int)FsmStateType.COMBAT_MOVE);
        else if (action is AgentActionAttackMelee)
            CurState = GetState((int)FsmStateType.ATTACK_MELEE);
        else if (action is AgentActionInjury)
            CurState = GetState((int)FsmStateType.INJURY);
        else if (action is AgentActionDeath)
            CurState = GetState((int)FsmStateType.DEATH);
        else if (action is AgentActionKnockdown)
            CurState = GetState((int)FsmStateType.KNOCKDOWN);
        else if (action is AgentActionBlock)
            CurState = GetState((int)FsmStateType.BLOCK);

        preState.Exit();
        CurState.Enter(action);
    }

}
