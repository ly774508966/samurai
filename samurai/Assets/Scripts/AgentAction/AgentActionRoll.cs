using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionRoll : AgentAction
{
    public Vector3 direction;
    public Agent toTarget;    

    public AgentActionRoll(Agent owner) : base(AgentActionType.ROLL, owner)
    {
        Reset(owner);
    }
    public override void Reset(Agent agent)
    {
        base.Reset(agent);
        direction = Vector3.zero;
        toTarget = null;
    }
}
