using UnityEngine;

public class AnimFsmStateKnockdown : AnimFsmState
{
    enum KnockdownStatus
    {
        START,
        LOOP,
        //FATALITY,
        DEATH,
        END,
    }

    AgentActionKnockdown _agentAction;

    Quaternion FinalRotation;
    Quaternion StartRotation;
    Vector3 StartPosition;
    Vector3 FinalPosition;
    float CurrentRotationTime;
    float RotationTime;
    float CurrentMoveTime;
    float MoveTime;
    float EndOfStateTime;
    float KnockdownEndTime;

    bool RotationOk = false;
    bool PositionOK = false;

    KnockdownStatus _status;

    public AnimFsmStateKnockdown(Agent owner) : base(owner)
    {
    }

    public override void Enter(AgentAction action)
    {        
        base.Enter(action);
        Owner.BlackBoard.motionType = MotionType.KNOCKDOWN;
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
        if (_status == KnockdownStatus.DEATH)
            return;

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

        if (_status != KnockdownStatus.DEATH && Owner.BlackBoard.damageType == DamageType.InKnockdown)
        {
            InitializeDeath();
        }

        switch (_status)
        {
            case KnockdownStatus.START:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeKnockdownLoop();
                break;
            case KnockdownStatus.LOOP:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    InitializeKnockdownUp();
                break;
            /*case KnockdownState.FATALITY:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                {                    
                    InitializeDeath();
                }
                break;*/
            case KnockdownStatus.END:
                if (EndOfStateTime <= Time.timeSinceLevelLoad)
                    IsFinished = true;
                break;
            case KnockdownStatus.DEATH:
                break;
        }
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);
        _agentAction = Owner.BlackBoard.curAction as AgentActionKnockdown;        

        string animName = Owner.AnimSet.GetKnockdowAnim(KnockdownState.Down, Owner.BlackBoard.weaponSelected);

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
        FinalPosition = StartPosition + _agentAction.impuls;
        MoveTime = Owner.AnimEngine[animName].length * 0.4f;

        RotationOk = RotationTime == 0;
        PositionOK = MoveTime == 0;

        CurrentRotationTime = 0;
        CurrentMoveTime = 0;

        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;
        KnockdownEndTime = EndOfStateTime + _agentAction.time;

        _status = KnockdownStatus.START;
    }

    private void InitializeKnockdownLoop()
    {
        string animName = Owner.AnimSet.GetKnockdowAnim(KnockdownState.Loop, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.05f); ;
        EndOfStateTime = KnockdownEndTime;
        _status = KnockdownStatus.LOOP;    
    }


    private void InitializeDeath()
    {
        string animName = Owner.AnimSet.GetKnockdowAnim(KnockdownState.Fatality, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.1f);

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;        
        _status = KnockdownStatus.DEATH;
        Owner.BlackBoard.motionType = MotionType.DEATH;
    }

    private void InitializeKnockdownUp()
    {
        string animName = Owner.AnimSet.GetKnockdowAnim(KnockdownState.Up, Owner.BlackBoard.weaponSelected);
        CrossFade(animName, 0.05f);

        EndOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[animName].length * 0.9f;

        _status = KnockdownStatus.END;        
    }

    void UpdateFinalRotation()
    {
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
