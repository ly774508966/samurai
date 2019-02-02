using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateMove : AnimFsmState
{
    Quaternion finalRotation = new Quaternion();
    Quaternion startRotation = new Quaternion();
    float rotationProgress;

    /*public AnimFsmStateMove(BlackBoard blackBoard, AnimSetPlayer animSetPlayer, Animation animEngine, Transform trans, CharacterController characterController)
        : base(blackBoard, animSetPlayer, animEngine, trans, characterController)
    {

    }*/
    public AnimFsmStateMove(Agent owner) : base(owner)
    {

    }

    public override void Enter(AgentAction action)
    {
        base.Enter(action);
        PlayAnim(GetMoveMotionType(), GetWeaponState());
    }    

    public override void Exit()
    {
        Owner.BlackBoard.speed = 0;
        base.Exit();
    }

    public override bool HandleSameAction(AgentAction action)
    {
        //if (action == null || BlackBoard.curAction == null || action.Type != BlackBoard.curAction.Type)        
        //  return;

        if (action as AgentActionMove == null)
            return false;

        Owner.BlackBoard.curAction.Release();
        IsFinished = false;
        Initialize(action);
        return true;
    }

    protected override void Initialize(AgentAction action)
    {
       // Debug.Log("move");
        base.Initialize(action);
        AgentActionMove moveAction = Owner.BlackBoard.curAction as AgentActionMove;
        Owner.BlackBoard.desiredDirection = moveAction.moveDir;
        finalRotation.SetLookRotation(Owner.BlackBoard.desiredDirection);
        startRotation = Owner.Transform.rotation;
        Owner.BlackBoard.motionType = GetMoveMotionType();
        rotationProgress = 0;
    }

    
    private void PlayAnim(MotionType motion, WeaponState weaponState)
    {
        Owner.BlackBoard.motionType = motion;
        Owner.BlackBoard.weaponState = weaponState;
        CrossFade(Owner.AnimSet.GetMoveAnim(Owner.BlackBoard.motionType, MoveType.FORWARD, Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState), 0.2f);
    }

    private MotionType GetMoveMotionType()
    {
        if (Owner.BlackBoard.speed > Owner.BlackBoard.maxRunSpeed * 1.5f)
            return MotionType.SPRINT;
        else if (Owner.BlackBoard.speed > Owner.BlackBoard.maxWalkSpeed * 1.5f)
            return MotionType.RUN;

        return MotionType.WALK;
    }

    private WeaponState GetWeaponState()
    {
        if (Owner.BlackBoard.desiredTarget != null)
            return WeaponState.Ready;
        else
            return WeaponState.NotInHands;
    }

    public override void Loop()
    {   
        rotationProgress += Time.deltaTime * Owner.BlackBoard.rotationSmooth;
        rotationProgress = Mathf.Min(rotationProgress, 1);
        Quaternion q = Quaternion.Slerp(startRotation, finalRotation, rotationProgress);
        Owner.Transform.rotation = q;

        if (Quaternion.Angle(q, finalRotation) > 40.0f)
            return;

        float maxSpeed = Mathf.Max(Owner.BlackBoard.maxWalkSpeed, Owner.BlackBoard.maxRunSpeed * Owner.BlackBoard.moveSpeedModifier);        
        float curSmooth = Owner.BlackBoard.speedSmooth * Time.deltaTime;
        Owner.BlackBoard.speed = Mathf.Lerp(Owner.BlackBoard.speed, maxSpeed, curSmooth);
        Owner.BlackBoard.moveDir = Owner.BlackBoard.desiredDirection;

        // MOVE
        if (Move(Owner.BlackBoard.moveDir * Owner.BlackBoard.speed * Time.deltaTime, true) == false)
            IsFinished = true;

        MotionType motion = GetMoveMotionType();
        WeaponState weaponState = GetWeaponState();
        if (motion != Owner.BlackBoard.motionType || weaponState != Owner.BlackBoard.weaponState)
            PlayAnim(motion, weaponState);        
    }
    
}
