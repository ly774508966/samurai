using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Queue<AgentAction> _inputOrders = new Queue<AgentAction>();
    //int _maxCount = 1;
    Agent _owner;

    void Awake()
    {
        _owner = GetComponent<Agent>();
    }

    void AddOrder(AgentAction action)
    {
        /*if (_inputOrders.Count >= _maxCount)
        {
            return;
        }*/
        _inputOrders.Enqueue(action);
    }

    public AgentAction GetInputAction()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // phenix 添加
        if (moveDir != Vector3.zero)
        {
            AgentActionMove moveAction = AgentActionFactory.Get(AgentActionType.MOVE, _owner) as AgentActionMove;
            moveAction.moveDir = moveDir;
            AddOrder(moveAction);            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AgentActionRoll rollAction = AgentActionFactory.Get(AgentActionType.ROLL, _owner) as AgentActionRoll;
            rollAction.direction = transform.forward;
            rollAction.toTarget = null;
            AddOrder(rollAction);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            AgentActionAttackMelee attackMeleeAction = AgentActionFactory.Get(AgentActionType.ATTACK_MELEE, _owner) as AgentActionAttackMelee;
            attackMeleeAction.attackType = AttackType.X;            
            AddOrder(attackMeleeAction);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            AgentActionAttackMelee attackMeleeAction = AgentActionFactory.Get(AgentActionType.ATTACK_MELEE, _owner) as AgentActionAttackMelee;
            attackMeleeAction.attackType = AttackType.O;            
            AddOrder(attackMeleeAction);
        }

        if (_inputOrders.Count > 0)
        {
            return _inputOrders.Dequeue();
        }
        return null;
    }
}
