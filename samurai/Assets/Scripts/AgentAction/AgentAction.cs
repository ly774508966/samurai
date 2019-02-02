using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAction
{    
    AgentActionType  _type = AgentActionType.NONE;
    public Agent Owner { get; private set; }

    public GOAPAction GoapAction { get; set; }
    public AgentAction BuddyAction { get; set; }    
    public AgentActionType Type { get { return _type; } }    

    public AgentAction(AgentActionType actionType, Agent owner)
    {
        _type = actionType;
        Owner = owner;
    }

    public void Release()
    {
        if (GoapAction != null)
        {
            GoapAction.IsFinished = true;
            GoapAction.AgentAction = null;
        }

        if (BuddyAction != null)
        {
            BuddyAction.Release();
        }
        AgentActionFactory.Collect(this);
    }

    public virtual void Reset(Agent agent)
    {
        Owner = agent;
        BuddyAction = null;
        GoapAction = null;
    }
}
