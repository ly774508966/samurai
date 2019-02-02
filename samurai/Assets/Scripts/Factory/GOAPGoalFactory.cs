using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GOAPGoalType
{
	NONE = -1,        
    STEP_IN,
    STEP_OUT,
    STEP_AROUND,
    ATTACK_TARGET,
    REACT_TO_DAMAGE,
    BLOCK,
    /*LOOK_AT_TARGET,
    ORDER_ATTACK,
    ORDER_DODGE,
    GOTO,
    DODGE,
    ALERT,
    CALM,    
    PLAY_ANIM,
    IDLE_ANIM,    
    BOSS_ATTACK,    */
    MAX,
}

class GOAPGoalFactory
{
    static Queue<GOAPGoal>[] _buffers = new Queue<GOAPGoal>[(int)GOAPGoalType.MAX];

    static GOAPGoalFactory()
    {
        int length = (int)GOAPGoalType.MAX;
        for (int i = 0; i < length; i++)
        {
            _buffers[i] = new Queue<GOAPGoal>();
        }
    }
    public static GOAPGoal Get(GOAPGoalType type, Agent owner)
	{
        if (_buffers[(int)type].Count > 0)
        {
            _buffers[(int)type].Peek().Reset(owner);
            return _buffers[(int)type].Dequeue();
        }
        switch (type)
		{
            case GOAPGoalType.STEP_IN:
                return new GOAPGoalStepIn(owner);
            case GOAPGoalType.STEP_OUT:
                return new GOAPGoalStepOut(owner);
            case GOAPGoalType.STEP_AROUND:
                return new GOAPGoalStepAround(owner);
            case GOAPGoalType.ATTACK_TARGET:
                return new GOAPGoalAttackTarget(owner);
            case GOAPGoalType.REACT_TO_DAMAGE:
                return new GOAPGoalReactToDamage(owner);
            case GOAPGoalType.BLOCK:
                return new GOAPGoalBlock(owner);
            /*case GOAPGoalType.ORDER_ATTACK:
                return new GOAPGoalOrderAttack(owner);                
            case GOAPGoalType.ORDER_DODGE:
                return new GOAPGoalOrderDodge(owner);                            
            case GOAPGoalType.LOOK_AT_TARGET:
                return new GOAPGoalLookAtTarget(owner);                
            case GOAPGoalType.KILL_TARGET:
                return new GOAPGoalKillTarget(owner);			    
            case GOAPGoalType.DODGE:
                return new GOAPGoalDodge(owner);                            
            case GOAPGoalType.ALERT:
                return new GOAPGoalAlert(owner);                
            case GOAPGoalType.CALM:
                return new GOAPGoalCalm(owner);                            
            case GOAPGoalType.PLAY_ANIM:
                return new GOAPGoalPlayAnim(owner);                
            case GOAPGoalType.IDLE_ANIM:
                return new GOAPGoalIdleAction(owner);                
            */
            default:
			    return null;
		}
	}
    public static void Collect(GOAPGoal goal)
    {
        if (goal == null)
        {
            return;
        }
        _buffers[(int)goal.GoalType].Enqueue(goal);
    }
}
