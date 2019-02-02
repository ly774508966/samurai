using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateDeath : AnimFsmState
{
    Vector3 StartPosition;
    Vector3 FinalPosition;
    Quaternion FinalRotation;
    Quaternion StartRotation;

    float RotationProgress;
    float MoveTime;
    float CurrentMoveTime;
    bool PositionOK = false;
    bool RotationOk = false;

    public AnimFsmStateDeath(Agent owner) : base(owner)
    {

    }

    public override void Enter(AgentAction action)
    {
        Debug.Log("dead");
        base.Enter(action);
        Owner.BlackBoard.motionType = MotionType.NONE;
        Owner.BlackBoard.moveDir = Vector3.zero;
        Owner.BlackBoard.speed = 0;        
    }

    public override void Loop()
    {        
        if (RotationOk == false)
        {            
            RotationProgress += Time.deltaTime * Owner.BlackBoard.rotationSmooth;

            if (RotationProgress >= 1)
            {
                RotationProgress = 1;
                RotationOk = true;
            }

            RotationProgress = Mathf.Min(RotationProgress, 1);
            Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, RotationProgress);
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

            float progress = Mathf.Min(1.0f, CurrentMoveTime / MoveTime);
            Vector3 finalPos = Mathfx.Sinerp(StartPosition, FinalPosition, progress);            
            if (Move(finalPos - Owner.Transform.position, true) == false)
            {
                PositionOK = true;
            }
        }

    }
    
    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);
        AgentActionDeath deathAction = Owner.BlackBoard.curAction as AgentActionDeath;       
        
        string animName = Owner.AnimSet.GetDeathAnim(deathAction.fromWeapon, deathAction.damageType);
        CrossFade(animName, 0.1f);
        
        Owner.BlackBoard.motionType = MotionType.NONE;
        StartPosition = Owner.Transform.position;

        if (deathAction.attacker != null)
        {
            FinalPosition = StartPosition + deathAction.attacker.Forward;

            StartRotation = Owner.Transform.rotation;
            FinalRotation.SetLookRotation(-deathAction.attacker.Forward);

            PositionOK = false;
            RotationOk = false;

            RotationProgress = 0;
        }
        else
        {
            StartPosition = Owner.Transform.position;
            FinalPosition = StartPosition + deathAction.impuls;

            PositionOK = false;
            RotationOk = true;
        }

        CurrentMoveTime = 0;
        MoveTime = Owner.AnimEngine[animName].length * 0.6f;

        //Owner.Invoke("SpawnBlood", AnimEngine[animName].length);
        Owner.BlackBoard.motionType = MotionType.DEATH;        
    }
}
