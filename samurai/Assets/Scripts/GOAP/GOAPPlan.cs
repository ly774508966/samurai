using System;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlan
{
	Queue<GOAPAction> _actions = new Queue<GOAPAction>();
    Agent _owner;

    public GOAPPlan(Agent owner)
    {
        _owner = owner;
    }

	public bool UpdatePlan(WorldState curWS)
	{
        if (_actions.Count == 0)
        {
            return false;
        }

        GOAPAction curGOAPAction = _actions.Peek();
        if (!curGOAPAction.IsValid())
        {
            Clear();
            return false;
        }        

        if (curGOAPAction.IsFinished)
        {
            //curGOAPAction.ApplyEffect(curWS); // 实施效果（其实没必要调用了，因为这时候没有必要设置ws效果了，因为ws的效果没有意义，接着执行下面的action就行）
            _actions.Dequeue().Release();            
            if (_actions.Count > 0)
            {
                curGOAPAction = _actions.Peek();
                curGOAPAction.Activate();
            }
        }
        else
            curGOAPAction.UpdateGOAPAction();

        return true;
    }

    public void Reset(Agent owner)
    {
        _owner = owner;
        Clear();
    }

    public void Clear()
    {
        while (_actions.Count > 0)
        {
            GOAPActionFactory.Collect(_actions.Dequeue());
        }
    }
    
    public virtual void Build(GOAPActionType[] candidates, WorldState curWS, GOAPGoal goal)
    {
        _actions.Clear();

        // 测试用，后续改成A*
        /*foreach (var actionType in candidates)
        {
            GOAPAction action = GOAPActionFactory.Get(actionType, _agent);
            if (action == null)
            {
                Debug.LogWarning("null for actiontype: " + actionType);
                continue;
            }
            if (actionType == GOAPActionType.GOTO_MELEE_RANGE)
            {
                _actions.Enqueue(action);
            }
        }*/
        GOAPAction action = null;
        switch (goal.GoalType)
        {            
            case GOAPGoalType.STEP_IN:
                action = GOAPActionFactory.Get(GOAPActionType.STEP_IN, _owner);
                _actions.Enqueue(action);
                break;
            case GOAPGoalType.STEP_OUT:
                action = GOAPActionFactory.Get(GOAPActionType.STEP_OUT, _owner);
                _actions.Enqueue(action);
                break;
            case GOAPGoalType.STEP_AROUND:
                action = GOAPActionFactory.Get(GOAPActionType.STEP_AROUND, _owner);
                _actions.Enqueue(action);
                break;
            case GOAPGoalType.ATTACK_TARGET:
                action = GOAPActionFactory.Get(GOAPActionType.GOTO_MELEE_RANGE, _owner);
                _actions.Enqueue(action);
                if (_owner.agentType == AgentType.PEASANT || _owner.agentType == AgentType.SWORD_MAN)
                {
                    action = GOAPActionFactory.Get(GOAPActionType.ATTACK_MELEE_ONCE, _owner);
                    _actions.Enqueue(action);
                }
                else if (_owner.agentType == AgentType.DOUBLE_SWORDS_MAN)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)                    
                        action = GOAPActionFactory.Get(GOAPActionType.ATTACK_MELEE_TWO_SWORDS, _owner);                    
                    else                    
                        action = GOAPActionFactory.Get(GOAPActionType.ATTACK_WHIRL, _owner);
                    _actions.Enqueue(action);
                }                                
                break;
            case GOAPGoalType.REACT_TO_DAMAGE:
                if (curWS.GetWSProperty(WorldStatePropKey.EVENT).GetEvent() == EventTypes.HIT)
                {
                    action = GOAPActionFactory.Get(GOAPActionType.INJURY, _owner);
                    _actions.Enqueue(action);
                }
                else if (curWS.GetWSProperty(WorldStatePropKey.EVENT).GetEvent() == EventTypes.DEAD)
                {
                    action = GOAPActionFactory.Get(GOAPActionType.DEATH, _owner);
                    _actions.Enqueue(action);
                }
                else if (curWS.GetWSProperty(WorldStatePropKey.EVENT).GetEvent() == EventTypes.KNOCKDOWN)
                {
                    action = GOAPActionFactory.Get(GOAPActionType.KNOCKDOWN, _owner);
                    _actions.Enqueue(action);
                }
                break;
            case GOAPGoalType.BLOCK:
                action = GOAPActionFactory.Get(GOAPActionType.BLOCK, _owner);
                _actions.Enqueue(action);
                break;
            default:
                break;
        }        

        if (_actions.Count > 0)
        {
            _actions.Peek().Activate();
        }
    }
}

