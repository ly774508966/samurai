using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateRoll : AnimFsmState
{
    Quaternion  _finalRotation;
    Quaternion  _startRotation;
    Vector3     _startPosition;
    Vector3     _finalPosition;
    float       _currentRotationTime;
    float       _rotationTime;
    float       _currentMoveTime;
    float       _moveTime;
    float       _endOfStateTime;
    //float       _blockEndTime;

    bool        _rotationOk = false;
    bool        _positionOK = false;

    /*public AnimFsmStateRoll(BlackBoard blackBoard, AnimSetPlayer animSetPlayer, Animation animEngine, Transform trans, CharacterController characterController)
        : base(blackBoard, animSetPlayer, animEngine, trans, characterController)
    {

    }*/
    public AnimFsmStateRoll(Agent owner) : base(owner)
    {

    }
    public override void Exit()
    {
        Owner.BlackBoard.speed = 0;
        base.Exit();
    }

    public override void Loop()
    {
        if (_rotationOk == false)
        {
            _currentRotationTime += Time.deltaTime;

            if (_currentRotationTime >= _rotationTime)
            {
                _currentRotationTime = _rotationTime;
                _rotationOk = true;
            }

            float progress = _currentRotationTime / _rotationTime;
            Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, progress);
            Owner.Transform.rotation = q;
        }

        if (_positionOK == false)// && (RotationOk || (Quaternion.Angle(Owner.Transform.rotation, FinalRotation) > 40.0f))
        {
            _currentMoveTime += Time.deltaTime;
            if (_currentMoveTime >= _moveTime)
            {
                _currentMoveTime = _moveTime;
                _positionOK = true;
            }

            float progress = _currentMoveTime / _moveTime;
            Vector3 finalPos = Mathfx.Hermite(_startPosition, _finalPosition, progress);
            //MoveTo(finalPos);
            if (Move(finalPos - Owner.Transform.position, true) == false)
                _positionOK = true;
        }

        if (_endOfStateTime <= Time.timeSinceLevelLoad)
            IsFinished = true;
    }

    protected override void Initialize(AgentAction action)
    {
        Debug.Log("roll");
        base.Initialize(action);
        AgentActionRoll rollAction = Owner.BlackBoard.curAction as AgentActionRoll;
        _currentMoveTime = 0;
        _currentRotationTime = 0;
        _startRotation = Owner.Transform.rotation;
        _startPosition = Owner.Transform.position;

        Vector3 finalDir;
        if (rollAction.toTarget != null)
        {
            finalDir = rollAction.toTarget.Position - Owner.Transform.position;
            finalDir.Normalize();

            _finalPosition = rollAction.toTarget.Position - finalDir * Owner.BlackBoard.weaponRange;
        }
        else
        {
            finalDir = rollAction.direction;
            _finalPosition = _startPosition + rollAction.direction * Owner.BlackBoard.rollDistance;
        }

        string animName = Owner.AnimSet.GetRollAnim(Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState);
        CrossFade(animName, 0.1f);

        _finalRotation.SetLookRotation(finalDir);

        _rotationTime = Vector3.Angle(Owner.Transform.forward, finalDir) / 1000.0f;
        _moveTime = Owner.AnimEngine[animName].length * 0.85f;
        _endOfStateTime = Owner.AnimEngine[animName].length * 0.9f + Time.timeSinceLevelLoad;

        _rotationOk = _rotationTime == 0;
        _positionOK = false;

        Owner.BlackBoard.motionType = MotionType.ROLL;        
    }

}
