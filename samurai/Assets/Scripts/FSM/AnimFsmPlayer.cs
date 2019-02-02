using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmPlayer : Fsm
{
    enum FsmStateType
    {
        IDLE = 0,
        MOVE,
        ROLL,
        ATTACK_MELEE,        
        INJURY,
        DEATH,
        MAX
    }

    BlackBoard _blackBoard;
        
    void Awake ()
    {
        _blackBoard = transform.GetComponent<BlackBoard>();

        // 一定要按枚举顺序加入, 以便于按枚举值索引
        AnimFsmStateIdle idleState = new AnimFsmStateIdle(GetComponent<Agent>());
        AddState(idleState);
        AnimFsmStateMove moveState = new AnimFsmStateMove(GetComponent<Agent>());
        AddState(moveState);
        AnimFsmStateRoll rollState = new AnimFsmStateRoll(GetComponent<Agent>());
        AddState(rollState);
        AnimFsmStateAttackMelee attackMeleeState = new AnimFsmStateAttackMelee(GetComponent<Agent>());
        AddState(attackMeleeState);
        AnimFsmStateInjury injuryState = new AnimFsmStateInjury(GetComponent<Agent>());
        AddState(injuryState);
        AnimFsmStateDeath deathState = new AnimFsmStateDeath(GetComponent<Agent>());
        AddState(deathState);

        CurState = DefState = idleState;        
    }

    void Start()
    {
        CurState.Enter(null);
    }    

    protected override void DoAction(AgentAction action)
    {
        FsmState preState = CurState;
        // ------------------------------ 无要执行的action ------------------------------
        if (action == null)
        {
            //if (_blackBoard.curAction != null && _blackBoard.curAction.Type == AgentAction.ActionType.MOVE)
            if ((_blackBoard.curAction as AgentActionMove) != null)
            {
                CurState = GetState((int)FsmStateType.IDLE); // 转入idle状态
                preState.Exit();
                CurState.Enter(action);                
            }
            return;
        }

        // ------------------------------ 有要执行的action ------------------------------

        // 执行同样行为
        /*if (_blackBoard.curAction != null && _blackBoard.curAction.Type == action.Type)
        {
            CurState.HandleSameAction(action);
            return;
        }*/
        if (CurState.HandleSameAction(action))
        {
            return;
        }


        //if (action.Type == AgentAction.ActionType.DEATH)
        if (action as AgentActionDeath != null)
        {
            CurState = GetState((int)FsmStateType.DEATH);
        }
        //else if (action.Type == AgentAction.ActionType.INJURY && _blackBoard.curAction.Type != AgentAction.ActionType.DEATH)
        else if (action as AgentActionInjury != null && _blackBoard.curAction as AgentActionDeath == null)
        {
            CurState = GetState((int)FsmStateType.INJURY);
        }
        else if (_blackBoard.curAction == null) // 当前是idle状态
        {
            switch (action.Type)
            {
                case AgentActionType.MOVE:
                    CurState = GetState((int)FsmStateType.MOVE);
                    break;
                case AgentActionType.ROLL:
                    CurState = GetState((int)FsmStateType.ROLL);
                    break;
                case AgentActionType.ATTACK_MELEE:
                    CurState = GetState((int)FsmStateType.ATTACK_MELEE);
                    break;
                default:
                    return;
            }
        }
        //else if (_blackBoard.curAction.Type == AgentAction.ActionType.MOVE)
        else if (_blackBoard.curAction as AgentActionMove != null) // 当前是move状态
        {
            switch (action.Type)
            {                
                case AgentActionType.ROLL:
                    CurState = GetState((int)FsmStateType.ROLL);
                    break;
                case AgentActionType.ATTACK_MELEE:
                    CurState = GetState((int)FsmStateType.ATTACK_MELEE);
                    break;
                default:
                    return;
            }
        }
        else if (_blackBoard.curAction.Type == AgentActionType.ATTACK_MELEE) // 当前是attack状态
        {
            switch (action.Type)
            {
                case AgentActionType.ROLL:
                    CurState = GetState((int)FsmStateType.ROLL);
                    break;                
                default:
                    return;
            }
        }
        else
        {
            return;
        }

        preState.Exit();
        CurState.Enter(action);

        /*FsmState preState = CurState;
        if (action == null && _blackBoard.curAction != null && _blackBoard.curAction.Type == AgentAction.ActionType.MOVE)        
        {
            CurState = GetState((int)FsmStateType.IDLE);
            return;
        }
        else if (action == null)
        {
            return;
        }
        else if (_blackBoard.curAction != null && _blackBoard.curAction.Type == action.Type)
        {
            CurState.HandleSameAction(action);
            return;
        }            
        else if (action.Type == AgentAction.ActionType.DEATH)
        {
            CurState = GetState((int)FsmStateType.DEATH);
        }
        else if (action.Type == AgentAction.ActionType.INJURY && _blackBoard.curAction.Type != AgentAction.ActionType.DEATH)
        {
            CurState = GetState((int)FsmStateType.INJURY);
        }
        else if (CurState == DefState) // 当前是idle状态
        {
            switch (action.Type)
            {
                case AgentAction.ActionType.MOVE:
                    CurState = GetState((int)FsmStateType.MOVE);
                    break;
                case AgentAction.ActionType.ROLL:
                    CurState = GetState((int)FsmStateType.ROLL);
                    break;
                case AgentAction.ActionType.ATTACK_MELEE:
                    CurState = GetState((int)FsmStateType.ATTACK_MELEE);
                    break;
                default:
                    return;
            }            
        }
        else
        {
            return;
        }

        preState.Exit();
        CurState.Enter(action);*/
    }

}
