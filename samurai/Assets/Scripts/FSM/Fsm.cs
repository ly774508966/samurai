using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fsm : MonoBehaviour {

    List<FsmState>  _states = new List<FsmState>();        // 状态集
    FsmState        _curState;      // 当前状态
    FsmState        _defState;      // 默认状态

    protected FsmState CurState { get { return _curState; } set { _curState = value; } }
    protected FsmState DefState { get { return _defState; } set { _defState = value; } }
    public bool InDefState { get { return CurState == DefState; } }

    protected void AddState(FsmState state)
    {
        _states.Add(state);
    }

    protected FsmState GetState(int idx)
    {
        if (idx < _states.Count && idx >= 0)
        {
            return _states[idx];
        }
        return null;
    }

    public virtual void Loop(AgentAction action)
    {         
        _curState.Loop();
        if (_curState.IsFinished)
        {
            _curState.Exit();
            _curState = _defState;
            _curState.Enter(null);
        }
        DoAction(action);
    }

   protected virtual void DoAction(AgentAction action) {}
}
