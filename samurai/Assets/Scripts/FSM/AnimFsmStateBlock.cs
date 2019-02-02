using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateBlock : AnimFsmState
{
    enum E_State
    {
        E_START,
        E_LOOP,
        E_BLOCK_HIT,
        E_END,
    }

    AgentActionBlock _agentAction;
    //AgentActionDamageBlocked ActionDamageBlocked;

    Quaternion FinalRotation;
    Quaternion StartRotation;
    Vector3 StartPosition;
    Vector3 FinalPosition;
    float CurrentRotationTime;
    float RotationTime;
    float CurrentMoveTime;
    float MoveTime;
    float EndOfStateTime;
    float BlockEndTime;

    bool RotationOk = false;
    bool PositionOK = false;
    
    E_State State;

    public AnimFsmStateBlock(Agent owner) : base(owner)
    {
    }

    public override void Enter(AgentAction action)
    {        
        base.Enter(action);
        Owner.BlackBoard.motionType = MotionType.BLOCK;
        Owner.BlackBoard.moveDir = Vector3.zero;
        Owner.BlackBoard.speed = 0;
    }

    public override void Exit()
    {             
        Owner.BlackBoard.motionType = MotionType.NONE;
        base.Exit();
    }

    public override void Loop()
    {        
        UpdateFinalRotation();

        if (RotationOk == false)
        {
            CurrentRotationTime += Time.deltaTime;
            if (CurrentRotationTime >= RotationTime)
            {
                CurrentRotationTime = RotationTime;
                RotationOk = true;
            }

            float progress = CurrentRotationTime / RotationTime;
            Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, progress);
            Owner.Transform.rotation = q;
        }

        if (PositionOK == false)
        {
            CurrentMoveTime += Time.deltaTime;
            if (CurrentMoveTime >= MoveTime)
            {
                CurrentMoveTime = MoveTime;
                PositionOK = true;
            }

            float progress = CurrentMoveTime / MoveTime;
            Vector3 finalPos = Mathfx.Sinerp(StartPosition, FinalPosition, progress);            
            if (Move(finalPos - Owner.Transform.position, true) == false)
                PositionOK = true;
        }        

        switch (State)
        {
            case E_State.E_START:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeBlockLoop();
                break;
            case E_State.E_LOOP:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                {
                    InitializeBlockEnd();
                }
                else if (Owner.BlackBoard.blockResult == BlockResult.FAIL)
                {
                    InitializeBlockFailed();
                    Owner.BlackBoard.blockResult = BlockResult.NONE;
                }
                else if (Owner.BlackBoard.blockResult == BlockResult.SUCCESS)
                {
                    InitializeBlockSuccess();
                    Owner.BlackBoard.blockResult = BlockResult.NONE;
                }
                break;
            case E_State.E_BLOCK_HIT:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                {
                    if (Time.timeSinceLevelLoad < BlockEndTime)
                        InitializeBlockLoop();
                    else
                        InitializeBlockEnd();

                    /*if (ActionDamageBlocked != null)
                    {
                        ActionDamageBlocked.SetSuccess();
                        ActionDamageBlocked = null;
                    }*/
                }
                break;
            case E_State.E_END:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    IsFinished = true;
                break;
        }
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);
        _agentAction = Owner.BlackBoard.curAction as AgentActionBlock;

        string animName = Owner.AnimSet.GetBlockAnim(BlockState.Start, Owner.BlackBoard.weaponSelected);

        StartRotation = Owner.Transform.rotation;
        StartPosition = Owner.Transform.position;

        Vector3 dir = _agentAction.attacker.Position - Owner.Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Owner.Transform.forward, dir);
        }
        else
            dir = Owner.Transform.forward;

        FinalRotation.SetLookRotation(dir);
        RotationTime = angle / 500.0f;
        MoveTime = 0;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;
        BlockEndTime = Time.timeSinceLevelLoad + _agentAction.time;

        State = E_State.E_START;
    }

    private void InitializeBlockLoop()
    {
        string animName = Owner.AnimSet.GetBlockAnim(BlockState.Loop, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.05f); ;

        EndOfStateTime = BlockEndTime;

        State = E_State.E_LOOP;
        Owner.BlackBoard.motionType = MotionType.BLOCK;
    }

    private void InitializeBlockSuccess()
    {
        string animName = Owner.AnimSet.GetBlockAnim(BlockState.HitBlocked, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.05f); ;

        StartRotation = Owner.Transform.rotation;
        StartPosition = Owner.Transform.position;

        Vector3 dir = _agentAction.attacker.Position - Owner.Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Owner.Transform.forward, dir);
        }
        else
            dir = Owner.Transform.forward;

        FinalRotation.SetLookRotation(dir);
        FinalPosition = StartPosition - dir * 0.75f;

        RotationTime = angle / 500.0f;
        MoveTime = 0.1f;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;        

        State = E_State.E_BLOCK_HIT;
        //Owner.BlackBoard.motionType = MotionType.BLOCKING_ATTACK;
        Owner.BlackBoard.motionType = MotionType.BLOCK;
    }


    private void InitializeBlockFailed()
    {
        string animName = Owner.AnimSet.GetBlockAnim(BlockState.Failed, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.05f); ;

        StartRotation = Owner.Transform.rotation;
        StartPosition = Owner.Transform.position;

        Vector3 dir = _agentAction.attacker.Position - Owner.Transform.position;
        float angle = 0;

        if (dir.sqrMagnitude > 0.1f * 0.1f)
        {
            dir.Normalize();
            angle = Vector3.Angle(Owner.Transform.forward, dir);
        }
        else
            dir = Owner.Transform.forward;

        FinalRotation.SetLookRotation(dir);
        FinalPosition = StartPosition - dir * 2;

        RotationTime = angle / 500.0f;
        MoveTime = Owner.AnimEngine[animName].length * 0.8f;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;

        State = E_State.E_END;
        Owner.BlackBoard.motionType = MotionType.INJURY;
    }

    private void InitializeBlockEnd()
    {
        //Debug.Log(Time.timeSinceLevelLoad + Owner.name + "Block end");

        string animName = Owner.AnimSet.GetBlockAnim(BlockState.End, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.05f); ;

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;
        Owner.BlackBoard.motionType = MotionType.NONE;

        State = E_State.E_END;
    }

    void UpdateFinalRotation()
    {
        if (_agentAction.attacker == null)
            return;

        Vector3 dir = _agentAction.attacker.Position - Owner.Position;
        dir.Normalize();

        FinalRotation.SetLookRotation(dir);
        StartRotation = Owner.Transform.rotation;
        float angle = Vector3.Angle(Owner.Transform.forward, dir);

        if (angle > 0)
        {
            RotationTime = angle / 100.0f;
            RotationOk = false;
            CurrentRotationTime = 0;
        }
    }
}
