using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionMove : AgentAction
{
    public Vector3 moveDir;

    public AgentActionMove(Agent owner) : base(AgentActionType.MOVE, owner)
    {
        Reset(owner);
    }    

    public override void Reset(Agent agent)
    {
        base.Reset(agent);
        moveDir = Vector3.zero;
    }
}
