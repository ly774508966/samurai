using UnityEngine;

public class AnimFsmStateCombatMove : AnimFsmState
{
	AgentActionCombatMove _agentActionCombatMove;
	float _maxSpeed;
    float _movedDistance;

	Quaternion _finalRotation = new Quaternion();
	Quaternion _startRotation = new Quaternion();
       
	float _rotationProgress;

    public AnimFsmStateCombatMove(Agent owner) : base(owner)
	{

	}

    public override void Enter(AgentAction action)
	{       
        base.Enter(action);
	}

    public override void Exit()
	{
        Owner.BlackBoard.speed = 0;
        base.Exit();
	}

    public override void Loop()
	{
        UpdateFinalRotation();        
        
		_rotationProgress += Time.deltaTime * Owner.BlackBoard.rotationSmoothInMove;
		_rotationProgress = Mathf.Min(_rotationProgress, 1);
		Quaternion q = Quaternion.Slerp(_startRotation, _finalRotation, _rotationProgress );
        Owner.Transform.rotation = q;

		if (Quaternion.Angle(q, _finalRotation) > 40.0f)
			return;
            	
		float curSmooth = Owner.BlackBoard.speedSmooth * Time.deltaTime;
        Owner.BlackBoard.speed = Mathf.Lerp(Owner.BlackBoard.speed, _maxSpeed, curSmooth);

        if (_agentActionCombatMove.moveType == MoveType.FORWARD)
            Owner.BlackBoard.moveDir = Owner.Forward;
        else if (_agentActionCombatMove.moveType == MoveType.BACKWARD)
            Owner.BlackBoard.moveDir = -Owner.Forward;
        else if (_agentActionCombatMove.moveType == MoveType.RIGHTWARD)
            Owner.BlackBoard.moveDir = Owner.Right;
        else if (_agentActionCombatMove.moveType == MoveType.LEFTWARD)
            Owner.BlackBoard.moveDir = -Owner.Right;

        float dist = Owner.BlackBoard.speed * Time.deltaTime;

        if (Move(Owner.BlackBoard.moveDir * dist, true) == false)
            IsFinished = true;
      
        _movedDistance +=  dist;
        if (_movedDistance > _agentActionCombatMove.totalMoveDistance)
            IsFinished = true;

        if (_agentActionCombatMove.minDistanceToTarget > Owner.BlackBoard.DistanceToTarget)
            IsFinished = true;

        if (Owner.isPlayer == false)
        {
            //Owner.BlackBoard.Vigor = Owner.BlackBoard.Vigor + 0.1f;
        }
    }

    public override bool HandleSameAction(AgentAction action)
	{
		if (action as AgentActionCombatMove == null)
		{
            return false;		
		}

        Owner.BlackBoard.curAction.Release();
        IsFinished = false;
        Initialize(action);
        return true;
    }

	protected override void Initialize(AgentAction action)
	{
        base.Initialize(action);
        _agentActionCombatMove = Owner.BlackBoard.curAction as AgentActionCombatMove;
        
        UpdateFinalRotation();

        Owner.BlackBoard.motionType = MotionType.WALK;
		_rotationProgress = 0;
        _movedDistance = 0;
        Owner.BlackBoard.moveType = MoveType.NONE;
        UpdateMoveType();

        if (Owner.isPlayer == false && Owner.BlackBoard.moveType == MoveType.BACKWARD)
        {
            Owner.BlackBoard.Fear = 0;
        }
    }

    void UpdateFinalRotation()
    {
        if (_agentActionCombatMove.target == null)
            return;

        Vector3 dir = _agentActionCombatMove.target.Position - Owner.Position;
        dir.Normalize();
        _finalRotation.SetLookRotation(dir);
        _startRotation = Owner.Transform.rotation;

        if (_startRotation != _finalRotation)
           _rotationProgress = 0;
    }
   
    void UpdateMoveType()
    {
        Owner.BlackBoard.moveType = _agentActionCombatMove.moveType;        
        CrossFade(Owner.AnimSet.GetMoveAnim(_agentActionCombatMove.motionType, Owner.BlackBoard.moveType, Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState), 0.2f);
        _maxSpeed = _agentActionCombatMove.motionType == MotionType.RUN ? Owner.BlackBoard.maxRunSpeed : Owner.BlackBoard.maxCombatMoveSpeed;
    }
}
