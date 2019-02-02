using System;
using UnityEngine;

public abstract class GOAPAction : System.Object
{
    protected Agent Owner { get; private set; }

    public abstract bool IsPreConditionSatisfied(WorldState ws);
    public abstract void ApplyEffect(WorldState ws);
    protected abstract AgentAction MakeAgentAction();

    public float Cost { get; set; }

    public GOAPActionType Type { get; set; }
    public AgentAction AgentAction { get; set; }

    public bool IsFinished { get; set; }    

    protected GOAPAction(GOAPActionType type, Agent owner)
    {
        Type = type;
        IsFinished = false;
        Reset(owner);
    }

    public virtual void Activate()
    {
        /*AgentAction*/ AgentAction = MakeAgentAction();
        if (AgentAction != null)
        {
            AgentAction.GoapAction = this;
            Owner.AddAction(AgentAction);
        }
    }
    
    public virtual bool IsValid()
    {
        return true;
    }

    public virtual void Reset(Agent owner)
    {
        IsFinished = false;
        Owner = owner;
        AgentAction = null;        
    }

    public virtual void UpdateGOAPAction()
    {

    }

    public void Release()
    {
        if (AgentAction != null)
        {            
            AgentAction.GoapAction = null;
        }
        GOAPActionFactory.Collect(this);
    }
}
