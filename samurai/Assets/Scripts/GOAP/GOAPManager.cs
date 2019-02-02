using System;
using System.Collections.Generic;
using UnityEngine;


class GOAPManager : MonoBehaviour
{
    public GOAPGoalType[] goals = new GOAPGoalType[0];
    public GOAPActionType[] actions = new GOAPActionType[0];

    GOAPGoal    _currGoal;
    WorldState  _ws = new WorldState();
    Agent       _owner;

    void Awake()
    {
        _owner = GetComponent<Agent>();
    }

    void Start()
    {
        _ws.SetWSProperty(WorldStatePropKey.ORDER, OrderType.NONE);
        _ws.SetWSProperty(WorldStatePropKey.IN_IDLE, true);
        _ws.SetWSProperty(WorldStatePropKey.AT_TARGET_POS, true);
        _ws.SetWSProperty(WorldStatePropKey.TARGET_ATTACKED, false);
        _ws.SetWSProperty(WorldStatePropKey.LOOKING_AT_TARGET, false);        
        _ws.SetWSProperty(WorldStatePropKey.PLAY_ANIM, false);
        _ws.SetWSProperty(WorldStatePropKey.IN_DODGE, false);
        _ws.SetWSProperty(WorldStatePropKey.IN_WEAPONS_RANGE, false);
        _ws.SetWSProperty(WorldStatePropKey.WEAPON_IN_HANDS, false);
        _ws.SetWSProperty(WorldStatePropKey.IN_BLOCK, false);
        _ws.SetWSProperty(WorldStatePropKey.ALERTED, false);
        _ws.SetWSProperty(WorldStatePropKey.IN_COMBAT_RANGE, false);
        _ws.SetWSProperty(WorldStatePropKey.AHEAD_OF_ENEMY, false);
        _ws.SetWSProperty(WorldStatePropKey.BEHIND_ENEMY, false);
        _ws.SetWSProperty(WorldStatePropKey.MOVE_TO_RIGHT, false);
        _ws.SetWSProperty(WorldStatePropKey.MOVE_TO_LEFT, false);
        _ws.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.NONE);
    }    

    public WorldState WorldState { get { return _ws; } }

    public void Loop()
	{
		if (_currGoal == null)
		{
            _currGoal = FindNewGoal();
            if (_currGoal == null)
            {
                return;
            }

            _currGoal.BuildPlan(actions, _ws);
		}
        else if (_currGoal.IsInterruptible()) 
        {
            GOAPGoal newGoal = FindNewGoal(); // 遇见更优的目标
            if (newGoal != null && /*newGoal.GoalType != _currGoal.GoalType &&*/ newGoal.Weight > _currGoal.Weight)
            {
                GOAPGoalFactory.Collect(_currGoal); 
                _currGoal = newGoal;
                _currGoal.BuildPlan(actions, _ws);
            }
        }

        if (!_currGoal.UpdateGoal(_ws))
        {
            GOAPGoalFactory.Collect(_currGoal);
            _currGoal = null;
        }
    }

    protected virtual GOAPGoal FindNewGoal()
    {
        if (goals.Length == 0)
        {
            return null;
        }

        //if (_owner.BlackBoard.desiredTarget == null)
        //{
          //  return null;
        //}

        float maxWeight = 0;
        GOAPGoal maxGoal = null;
        foreach (var goalType in goals)
        {
            GOAPGoal goal = GOAPGoalFactory.Get(goalType, _owner);
            goal.CalcWeight(_ws, _owner.BlackBoard);
            if (goal.Weight > maxWeight)
            {
                maxWeight = goal.Weight;
                maxGoal = goal;
            }             
        }

        return maxGoal;
        
        /*if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            return GOAPGoalFactory.Get(GOAPGoalType.ATTACK_TARGET, _owner);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            return GOAPGoalFactory.Get(GOAPGoalType.BLOCK, _owner);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            return GOAPGoalFactory.Get(GOAPGoalType.STEP_IN, _owner);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            return GOAPGoalFactory.Get(GOAPGoalType.STEP_OUT, _owner);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            return GOAPGoalFactory.Get(GOAPGoalType.STEP_AROUND, _owner);
        }
        
        return null;*/
    }
}