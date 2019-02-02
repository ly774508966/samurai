using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GOAPActionType
{
    NONE = -1,    
    GOTO_MELEE_RANGE,    
    STEP_IN, 
    STEP_OUT,
    STEP_AROUND,    
    ATTACK_MELEE_ONCE,
    INJURY,
    DEATH,
    KNOCKDOWN,
    TOUNT,
    BLOCK,
    ATTACK_WHIRL,
    ATTACK_MELEE_TWO_SWORDS,
    ATTACK_BOW,
    //MOVE,
    //GOTO_POS,
    //ATTACKBERSERK,         
    //ATTACKBOSS,         
    //ATTACKROLL,         
    //ATTACKCOUNTER,      
    //COMBAT_RUN_FORWARD,
    //COMBAT_RUN_BACKWARD,
    //LOOK_AT_TARGET,
    ORDER_ATTACK,        
    ORDER_DODGE,         
    ROLL_TO_TARGET,           
    //PLAYANIM,               
    INJURY_OROCHI,
    MAX
}
public class GOAPActionFactory
{
    static Queue<GOAPAction>[] _buffers = new Queue<GOAPAction>[(int)GOAPActionType.MAX];

    static GOAPActionFactory()
    {
        int length = (int)GOAPActionType.MAX;
        for (int i = 0; i < length; i++)
        {
            _buffers[i] = new Queue<GOAPAction>();
        }
    }

    public static GOAPAction Get(GOAPActionType actionType, Agent owner)
    {
        if (_buffers[(int)actionType].Count > 0)
        {
            _buffers[(int)actionType].Peek().Reset(owner);
            return _buffers[(int)actionType].Dequeue();
        }
        switch (actionType)
        {
            case GOAPActionType.STEP_IN:
                return new GOAPActionStepIn(owner);
            case GOAPActionType.STEP_OUT:
                return new GOAPActionStepOut(owner);
            case GOAPActionType.STEP_AROUND:
                return new GOAPActionStepAround(owner);
            case GOAPActionType.GOTO_MELEE_RANGE:
                return new GOAPActionGoToMeleeRange(owner);            
            case GOAPActionType.INJURY:
                return new GOAPActionInjury(owner);
            case GOAPActionType.DEATH:
                return new GOAPActionDeath(owner);
            case GOAPActionType.KNOCKDOWN:
                return new GOAPActionKnockdown(owner);
            case GOAPActionType.ATTACK_MELEE_ONCE:
                return new GOAPActionAttackMeleeOnce(owner);
            case GOAPActionType.ATTACK_MELEE_TWO_SWORDS:
                return new GOAPActionAttackTwoSwords(owner);
            case GOAPActionType.ATTACK_WHIRL:
                return new GOAPActionAttackWhirl(owner);
            case GOAPActionType.BLOCK:
                return new GOAPActionBlock(owner);
            default:
                return null;
        }
    }

    public static void Collect(GOAPAction action)
    {
        if (action == null)
        {
            return;
        }
        _buffers[(int)action.Type].Enqueue(action);
    }
}
