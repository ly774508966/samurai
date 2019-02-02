using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FsmState
{    
    Agent       _owner;
    //BlackBoard  _blackBoard;    
    bool        _isFinished = false;     // 状态是否完成

    public bool IsFinished { get { return _isFinished; } set { _isFinished = value; } }    
    public Agent Owner { get { return _owner; } }
    //protected BlackBoard BlackBoard { get { return _blackBoard; } }
    

    public FsmState(Agent owner)
    {
        _owner = owner;
        //_blackBoard = owner.GetComponent<BlackBoard>();
    }

    public virtual void Enter(AgentAction action)
    {
        _isFinished = false;        
        Initialize(action);
    }
    protected virtual void Initialize(AgentAction action)
    {
        Owner.BlackBoard.curAction = action;
    }

    public virtual bool HandleSameAction(AgentAction action) { return false; }

    // Update is called once per frame
    public virtual void Loop () {}

    public virtual void Exit()
    {
        _isFinished = true;
        if (Owner.BlackBoard.curAction != null)
        {
           /* if (Owner.BlackBoard.curAction.GoapAction != null)
            {
                Owner.BlackBoard.curAction.GoapAction.IsFinished = true;
            }*/
            Owner.BlackBoard.curAction.Release();
            Owner.BlackBoard.curAction = null;
        }
    }
}
