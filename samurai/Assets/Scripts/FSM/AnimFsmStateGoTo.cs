using UnityEngine;

public class AnimFsmStateGoTo : AnimFsmState
{
    AgentActionGoTo _agentActionGoTo;
    float           _maxSpeed;
    string          _animName;

	Quaternion FinalRotation = new Quaternion();
	Quaternion StartRotation = new Quaternion();
	float RotationProgress;


	public AnimFsmStateGoTo(Agent owner) : base(owner)
    {

    }


    public override void Enter(AgentAction action)
	{       
        base.Enter(action);
        _animName = null;
		PlayAnim(_agentActionGoTo.motionType);
	}

    public override void Exit()
	{
        Owner.AnimEngine[_animName].speed = 1;
        Owner.BlackBoard.speed = 0;
        base.Exit();
	}

    public override void Loop()
	{		
        float dist = (_agentActionGoTo.finalPosition - Owner.Transform.position).sqrMagnitude;
        Vector3 dir;        

        if (_agentActionGoTo.motionType == MotionType.SPRINT)
        {
            if (dist < 0.5f * 0.5f)
                _maxSpeed = Owner.BlackBoard.maxWalkSpeed;
        }
        else
        {
            if (dist < 1.5f * 1.5f)
                _maxSpeed = Owner.BlackBoard.maxWalkSpeed;
        }

		RotationProgress += Time.deltaTime * Owner.BlackBoard.rotationSmooth;
		RotationProgress = Mathf.Min(RotationProgress, 1);
		Quaternion q = Quaternion.Slerp(StartRotation, FinalRotation, RotationProgress);
        Owner.Transform.rotation = q;
		
		float curSmooth = Owner.BlackBoard.speedSmooth * Time.deltaTime;
        Owner.BlackBoard.speed = Mathf.Lerp(Owner.BlackBoard.speed, _maxSpeed, curSmooth);

		dir = _agentActionGoTo.finalPosition - Owner.Transform.position;
		dir.y = 0;
		dir.Normalize();
        Owner.BlackBoard.moveDir = dir;

		// MOVE
		if (Move(Owner.BlackBoard.moveDir * Owner.BlackBoard.speed * Time.deltaTime, true) == false)
		{
            IsFinished = true;
		}
		else if ((_agentActionGoTo.finalPosition - Owner.Transform.position).sqrMagnitude < 0.3f * 0.3f)
		{
            IsFinished = true;
        }
		else 
		{
            MotionType motion = GetMotionType();
            if (motion != Owner.BlackBoard.motionType)
                PlayAnim(motion);
		}

	}

    public override bool HandleSameAction(AgentAction action)
	{
		if (action as AgentActionGoTo == null)
		{
            return false;			
		}

        Owner.BlackBoard.curAction.Release();
        IsFinished = false;
        Initialize(action);
        return true;
    }

	private void PlayAnim(MotionType motion)
	{
        Owner.BlackBoard.motionType = motion;
        _animName = Owner.AnimSet.GetMoveAnim(Owner.BlackBoard.motionType, MoveType.FORWARD, Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState);
        CrossFade(_animName, 0.2f);
	}

    MotionType GetMotionType()
    {
        if (Owner.BlackBoard.speed > Owner.BlackBoard.maxRunSpeed * 1.5f)
            return MotionType.SPRINT;
        else if (Owner.BlackBoard.speed > Owner.BlackBoard.maxWalkSpeed * 1.5f)
            return MotionType.RUN;
        
        return MotionType.WALK;
    }

	protected override void Initialize(AgentAction action)
	{
        base.Initialize(action);
        _agentActionGoTo = Owner.BlackBoard.curAction as AgentActionGoTo;        
        
        Vector3 dir = _agentActionGoTo.finalPosition - Owner.Transform.position;
        dir.y = 0;
        dir.Normalize();
        if (dir != Vector3.zero)
            FinalRotation.SetLookRotation(dir);

        StartRotation = Owner.Transform.rotation;

        Owner.BlackBoard.motionType = GetMotionType();

        if (_agentActionGoTo.motionType == MotionType.SPRINT)
        {
            _maxSpeed = Owner.BlackBoard.maxSprintSpeed;
        }
        else if (_agentActionGoTo.motionType == MotionType.RUN)
            _maxSpeed = Owner.BlackBoard.maxRunSpeed;
        else
            _maxSpeed = Owner.BlackBoard.maxWalkSpeed;

		RotationProgress = 0;

        if (Owner.isPlayer == false)
        {
            //Owner.BlackBoard.Vigor -= 20;
        }
    }
}
