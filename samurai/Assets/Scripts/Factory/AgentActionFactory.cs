using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentActionType
{
    NONE = -1,
    MOVE,
    GOTO_POS,
    COMBAT_MOVE,
    ROLL,
    ATTACK_MELEE,
    INJURY,
    DEATH,
    KNOCKDOWN,
    ATTACK_WHIRL,
    BLOCK,
    MAX
}

public class AgentActionFactory
{
    static Queue<AgentAction>[] _buffers = new Queue<AgentAction>[(int)AgentActionType.MAX];

    static AgentActionFactory()
    {
        int length = (int)AgentActionType.MAX;
        for (int i = 0; i < length; i++)
        {
            _buffers[i] = new Queue<AgentAction>();
        }
    }

    public static AgentAction Get(AgentActionType actionType, Agent owner)
    {
        if (_buffers[(int)actionType].Count > 0)
        {
            _buffers[(int)actionType].Peek().Reset(owner);
            return _buffers[(int)actionType].Dequeue();
        }
        switch (actionType)
        {
            case AgentActionType.ROLL:
                return new AgentActionRoll(owner);                
            case AgentActionType.MOVE:
                return new AgentActionMove(owner);
            case AgentActionType.ATTACK_MELEE:
                return new AgentActionAttackMelee(owner);
            case AgentActionType.COMBAT_MOVE:
                return new AgentActionCombatMove(owner);
            case AgentActionType.GOTO_POS:
                return new AgentActionGoTo(owner);
            case AgentActionType.INJURY:
                return new AgentActionInjury(owner);
            case AgentActionType.DEATH:
                return new AgentActionDeath(owner);
            case AgentActionType.KNOCKDOWN:
                return new AgentActionKnockdown(owner); 
            case AgentActionType.ATTACK_WHIRL:
                return new AgentActionAttackWhirl(owner);
            case AgentActionType.BLOCK:
                return new AgentActionBlock(owner);
            default:
                return null;
        }        
    }

    public static void Collect(AgentAction action)
    {
        if (action == null)
        {
            return;
        }
        _buffers[(int)action.Type].Enqueue(action);
    }
}
