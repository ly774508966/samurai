using UnityEngine;

public class AnimFsmStateInjury : AnimFsmState
{
    Quaternion _finalRotation;
    Quaternion _startRotation;

    float _rotationProgress;
    float _moveTime;
    float _currentMoveTime;
    bool _positionOK = false;
    bool _rotationOk = false;

    Vector3 _impuls;
    float _endOfStateTime;
    
    public AnimFsmStateInjury(Agent owner) : base(owner)        
    {
    }


    public override void Enter(AgentAction action)
    {
        base.Enter(action);
        Owner.BlackBoard.motionType = MotionType.NONE;
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
        if (_rotationOk == false)
        {     
            _rotationProgress += Time.deltaTime * Owner.BlackBoard.rotationSmooth;

            if (_rotationProgress >= 1)
            {
                _rotationProgress = 1;
                _rotationOk = true;
            }

            _rotationProgress = Mathf.Min(_rotationProgress, 1);
            Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, _rotationProgress);
            Owner.Transform.rotation = q;
        }

        if (_positionOK == false)
        {
            _currentMoveTime += Time.deltaTime;
            if (_currentMoveTime >= _moveTime)
            {
                _currentMoveTime = _moveTime;
                _positionOK = true;
            }

            float progress = Mathf.Max(0, Mathf.Min(1.0f, _currentMoveTime / _moveTime));
            Vector3 impuls = Vector3.Lerp(_impuls, Vector3.zero, progress);
            if (MoveEx(impuls * Time.deltaTime) == false)
            {             
                _positionOK = true;
            }
        }

        if (_endOfStateTime <= Time.timeSinceLevelLoad)
            IsFinished = true;
    }

    public override bool HandleSameAction(AgentAction action)
    {
        if (action as AgentActionInjury == null)
            return false;

        Owner.BlackBoard.curAction.Release();
        IsFinished = false;
        Initialize(action);
        return true;        
    }

    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);
        AgentActionInjury injuryAction = Owner.BlackBoard.curAction as AgentActionInjury;        
        string animName = Owner.AnimSet.GetInjuryAnim(injuryAction.fromWeapon, injuryAction.damageType);

        CrossFade(animName, 0.1f);        
        _endOfStateTime = Owner.AnimEngine[animName].length + Time.timeSinceLevelLoad;        
        Owner.BlackBoard.motionType = MotionType.NONE;
        _moveTime = Owner.AnimEngine[animName].length * 0.5f;
        _currentMoveTime = 0;

        if (injuryAction.attacker != null && Owner.isPlayer == false)
        {
            Vector3 dir = injuryAction.attacker.Position - Owner.Transform.position;
            dir.Normalize();
            _finalRotation.SetLookRotation(dir);
            _startRotation = Owner.Transform.rotation;
            _rotationProgress = 0;
            _rotationOk = false;
        }
        else
        {
            _rotationOk = true;
        }

        _impuls = injuryAction.impuls * 10;
        _positionOK = _impuls == Vector3.zero;
        Owner.BlackBoard.motionType = MotionType.INJURY;
    }
}
