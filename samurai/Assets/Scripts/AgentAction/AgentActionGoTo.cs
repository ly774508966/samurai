using UnityEngine;

public class AgentActionGoTo : AgentAction  
{
    public Vector3 finalPosition;
    public MoveType moveType;
    public MotionType motionType;

    public AgentActionGoTo(Agent owner) : base(AgentActionType.GOTO_POS, owner)
    {
    }

    public override void Reset(Agent agent)
    {
        base.Reset(agent);
        finalPosition = Vector3.zero;
        moveType = MoveType.NONE;
        motionType = MotionType.NONE;
    }
}
