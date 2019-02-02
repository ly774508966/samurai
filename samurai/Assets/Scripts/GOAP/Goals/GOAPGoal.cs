using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class GOAPGoal
{
    Agent _owner;
    public GOAPGoalType GoalType { get; private set; }
    public float Weight { get; set; }
    public GOAPPlan Plan { get; set; }
    
    public abstract bool IsSatisfied(WorldState worldState);
    //public abstract void MakeSatisfied(WorldState worldState);

    protected GOAPGoal(GOAPGoalType type, Agent owner)
    {
        GoalType = type;
        _owner = owner;
        Plan = new GOAPPlan(_owner);        
    }

    public void Reset(Agent owner)
    {
        _owner = owner;
        Plan.Reset(owner);
    }

    public virtual bool IsInterruptible()
    {
        return true;
    }

    public void BuildPlan(GOAPActionType[] candidates, WorldState curWS)
    {        
        Plan.Build(candidates, curWS, this);
    }

    public bool UpdateGoal(WorldState curWS)
    {          
        return Plan.UpdatePlan(curWS);        
    }
    
    public virtual void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
    
    }
}

