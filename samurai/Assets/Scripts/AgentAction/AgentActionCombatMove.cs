using UnityEngine;

public class AgentActionCombatMove: AgentAction  
{
    public Agent target;
    public float totalMoveDistance;
    public float minDistanceToTarget;

    public MoveType moveType;
    public MotionType motionType = MotionType.WALK; 

    public AgentActionCombatMove(Agent owner) : base(AgentActionType.COMBAT_MOVE, owner) { }

    public override void Reset(Agent agent)
    {
        base.Reset(agent);
        totalMoveDistance = 0;
        minDistanceToTarget = 0;
        moveType = MoveType.NONE;
        motionType = MotionType.WALK;
    }    
}
