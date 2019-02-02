using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateAttackMelee : AnimFsmState   
{
    enum AttackStatus
    {
        PREPARING,
        ATTACKING,
        FINISHED,
    }

    AgentActionAttackMelee _attackAction;
    float _timeToFinishWeapon = 0;  // 拔刀计时

    Quaternion  _finalRotation;
    Quaternion  _startRotation;
    Vector3     _startPosition;
    Vector3     _finalPosition;
    float       _currentRotationTime;
    float       _rotationTime;
    float       _moveTime;
    float       _currentMoveTime;
    float       _endOfStateTime;
    float       _hitTime;
    float       _attackPhaseTime;

    bool        _rotationOk = false;
    bool        _positionOK = false;    

    bool        _isCritical = false;
    bool        _knockdown = false;
    
    AttackStatus _attackStatus;    

    public AnimFsmStateAttackMelee(Agent owner) : base(owner)
    {
        
    }
    
    public override void Exit()
    {
        Owner.BlackBoard.speed = 0;
        base.Exit();
    }

    public override bool HandleSameAction(AgentAction action)
    {
        if (action as AgentActionAttackMelee == null)
        {
            return false;
        }

        if (_attackAction.BuddyAction == null)
        {
            if (_attackAction.attackPhaseDone)
            {
                _attackAction.Release();
                Initialize(action);
            }
            else
            {
                _attackAction.BuddyAction = action;
            }
        }

        return true;
        /*if (_attackAction.attackPhaseDone == false)
        {
            if (_attackAction.BuddyAction == null)
            {
                _attackAction.BuddyAction = action;
            }
            else if (_attackAction.BuddyAction.BuddyAction == null)
            {
                _attackAction.BuddyAction.BuddyAction = action;
            }
            else if (_attackAction.BuddyAction.BuddyAction.BuddyAction == null)
            {
                _attackAction.BuddyAction.BuddyAction.BuddyAction = action;
            }
            else if (_attackAction.BuddyAction.BuddyAction.BuddyAction.BuddyAction == null)
            {
                _attackAction.BuddyAction.BuddyAction.BuddyAction.BuddyAction = action;
            }
            return;
        }*/
    }

    public override void Loop()
    {
        if (Owner.BlackBoard.weaponState != WeaponState.Ready)
        {
            // 拔刀
            string name = Owner.AnimSet.GetShowWeaponAnim(Owner.BlackBoard.weaponSelected);
            _timeToFinishWeapon = Time.timeSinceLevelLoad + Owner.AnimEngine[name].length * 0.8f;
            Owner.AnimEngine.CrossFade(name, 0.1f);
            Owner.BlackBoard.weaponState = WeaponState.Ready;
        }
        else if (_timeToFinishWeapon > Time.timeSinceLevelLoad)
        {
            return;
        }
        else if (_attackStatus == AttackStatus.PREPARING)
        {
            bool dontMove = false;
            if (_rotationOk == false)
            {
                //Debug.Log("rotate");
                _currentRotationTime += Time.deltaTime;

                if (_currentRotationTime >= _rotationTime)
                {
                    _currentRotationTime = _rotationTime;
                    _rotationOk = true;
                }

                float progress = _currentRotationTime / _rotationTime;
                Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, progress);
                Owner.Transform.rotation = q;

                if (Quaternion.Angle(q, _finalRotation) > 20.0f)
                    dontMove = true;
            }

            if (dontMove == false && _positionOK == false)
            {
                _currentMoveTime += Time.deltaTime;
                if (_currentMoveTime >= _moveTime)
                {
                    _currentMoveTime = _moveTime;
                    _positionOK = true;
                }

                if (_currentMoveTime > 0)
                {
                    float progress = _currentMoveTime / _moveTime;
                    Vector3 finalPos = Mathfx.Hermite(_startPosition, _finalPosition, progress);
                    //if (MoveToCollideWithEnemy(finalPos, Transform.forward) == false)
                    if (Move(finalPos - Owner.Transform.position, true) == false)
                    {
                        _positionOK = true;
                    }
                }
            }

            if (_rotationOk && _positionOK)
            {
                _attackStatus = AttackStatus.ATTACKING;
                PlayAnim();
            }
        }
        else if (_attackStatus == AttackStatus.ATTACKING)
        {
            _currentMoveTime += Time.deltaTime;

            if (_attackPhaseTime < Time.timeSinceLevelLoad)
            {
                //Debug.Log(Time.timeSinceLevelLoad + " attack phase done");
                if (_attackAction.BuddyAction != null)
                {
                    AgentAction tmp = _attackAction.BuddyAction;
                    _attackAction.BuddyAction = null;
                    _attackAction.Release();
                    IsFinished = false;
                    Initialize(tmp);
                }
                else
                {
                    _attackAction.attackPhaseDone = true;
                    _attackStatus = AttackStatus.FINISHED;
                }
            }

            if (_currentMoveTime >= _moveTime)
                _currentMoveTime = _moveTime;

            if (_currentMoveTime > 0 && _currentMoveTime <= _moveTime)
            {
                float progress = Mathf.Min(1.0f, _currentMoveTime / _moveTime);
                Vector3 finalPos = Mathfx.Hermite(_startPosition, _finalPosition, progress);
                //if (MoveToCollideWithEnemy(finalPos, Transform.forward) == false)
                if (Move(finalPos - Owner.Transform.position, false) == false)
                {
                    _currentMoveTime = _moveTime;
                }

                // Debug.Log(Time.timeSinceLevelLoad + " moving");
            }

            if (_attackAction.hit == false && _hitTime <= Time.timeSinceLevelLoad)
            {
                _attackAction.hit = true;
                
                //if (BlackBoard.isPlayer && AnimAttackData.FullCombo)
                  //  GuiManager.Instance.ShowComboMessage(AnimAttackData.ComboIndex);              

                if (_attackAction.attackType == AttackType.Fatality)
                    Owner.DoDamageFatality(_attackAction.target, Owner.BlackBoard.weaponSelected, 
                        _attackAction.data);
                else
                    Owner.DoMeleeDamage(_attackAction.target, Owner.BlackBoard.weaponSelected, 
                        _attackAction.data, _isCritical, _knockdown);

                //if (_attackAction.data.lastAttackInCombo || _attackAction.data.comboStep == 3)
                  //  CameraBehaviour.Instance.ComboShake(_attackAction.data.comboStep - 3);

                /*if (AnimAttackData.LastAttackInCombo)
                    Owner.StartCoroutine(ShowTrail(AnimAttackData, 1, 0.3f, Critical, MoveTime - Time.timeSinceLevelLoad));
                else
                    Owner.StartCoroutine(ShowTrail(AnimAttackData, 2, 0.1f, Critical, MoveTime - Time.timeSinceLevelLoad));*/               
            }
        }
        else if (_attackStatus == AttackStatus.FINISHED && _endOfStateTime <= Time.timeSinceLevelLoad)
        {
            _attackAction.attackPhaseDone = true;
            //Debug.Log(Time.timeSinceLevelLoad + " attack finished");
            IsFinished = true;            
        }
    }

    private void PlayAnim()
    {
        CrossFade(_attackAction.data.animName, 0.2f);
                
        _hitTime = Time.timeSinceLevelLoad + _attackAction.data.hitTime;

        _startPosition = Owner.Transform.position;
        _finalPosition = _startPosition + Owner.Transform.forward * _attackAction.data.moveDistance;
        _moveTime = _attackAction.data.attackMoveEndTime - _attackAction.data.attackMoveStartTime;

        _endOfStateTime = Time.timeSinceLevelLoad + Owner.AnimEngine[_attackAction.data.animName].length * 0.9f;

        if (_attackAction.data.lastAttackInCombo)
        {            
            _attackPhaseTime = _endOfStateTime;
        }
        else
        {         
            _attackPhaseTime = Time.timeSinceLevelLoad + _attackAction.data.attackEndTime;
        }

        _currentMoveTime = -_attackAction.data.attackMoveStartTime;

        if (_attackAction.target && _attackAction.target.GetComponent<BlackBoard>().IsAlive)
        {            
            if (_isCritical)
            {
                CameraBehavior.Instance.ChangeTimeScale(0.25f, 0.5f);
                CameraBehavior.Instance.ChangeFov(25, 0.5f);
                CameraBehavior.Instance.Invoke("RestoreTimeScaleAndFov", 0.7f);
            }
            else if (_attackAction.attackType == AttackType.Fatality)
            {
                CameraBehavior.Instance.ChangeTimeScale(0.25f, 0.7f);
                CameraBehavior.Instance.ChangeFov(25, 0.65f);
                CameraBehavior.Instance.Invoke("RestoreTimeScaleAndFov", 0.8f);
            }
        }
    }

    public override void Enter(AgentAction action)
    {
        ComboMgr comboMgr = Owner.GetComponent<ComboMgr>();
        if (comboMgr)
        {
            comboMgr.Reset();
        }
        base.Enter(action);        
    }

    protected override void Initialize(AgentAction action)
    {
        //Debug.Log("attack");
        base.Initialize(action);
        _attackAction = Owner.BlackBoard.curAction as AgentActionAttackMelee;
        _attackAction.Initialize(/*Trans.GetComponent<Agent>()*/);        

        _attackStatus = AttackStatus.PREPARING;
        Owner.BlackBoard.motionType = MotionType.ATTACK;
        
        _attackAction.attackPhaseDone = false;
        _attackAction.hit = false;

        //if (_attackAction.data == null)
            //_attackAction.data = AnimSet.GetFirstAttackAnim(BlackBoard.weaponSelected, _attackAction.attackType);
        if (_attackAction.data == null)
            Debug.LogError("AnimAttackData == null");

        _startRotation = Owner.Transform.rotation;
        _startPosition = Owner.Transform.position;

        float angle = 0;

        bool backHit = false;

        float distance = 0;
        if (_attackAction.target != null)
        {
            Vector3 dir = _attackAction.target.Position - Owner.Transform.position;
            distance = dir.magnitude;

            if (distance > 0.1f)
            {
                dir.Normalize();
                angle = Vector3.Angle(Owner.Transform.forward, dir);
                
                if (angle < 40 && Vector3.Angle(Owner.Forward, _attackAction.target.Forward) < 80)
                    backHit = true;
            }
            else
            {
                dir = Owner.Transform.forward;
            }


            _finalRotation.SetLookRotation(dir);

            if (distance < Owner.BlackBoard.weaponRange)
                _finalPosition = _startPosition;
            else
                _finalPosition = _attackAction.target.transform.position - dir * Owner.BlackBoard.weaponRange;

            _moveTime = (_finalPosition - _startPosition).magnitude / 20.0f;
            _rotationTime = angle / 720.0f;
        }
        else
        {
            _finalRotation.SetLookRotation(_attackAction.attackDir);

            _rotationTime = Vector3.Angle(Owner.Transform.forward, _attackAction.attackDir) / 720.0f;
            _moveTime = 0;
        }

        _rotationOk = _rotationTime == 0;
        _positionOK = _moveTime == 0;

        _currentRotationTime = 0;
        _currentMoveTime = 0;

        if (Owner.isPlayer && _attackAction.data.hitCriticalType != CriticalHitType.None && _attackAction.target && 
            _attackAction.target.GetComponent<BlackBoard>().criticalAllowed && _attackAction.target.GetComponent<BlackBoard>().IsBlocking == false &&
            _attackAction.target.GetComponent<BlackBoard>().invulnerable == false)
        {
            if (backHit)
                _isCritical = true;
            else
            {
                // Debug.Log("critical chance" + Owner.GetCriticalChance() * AnimAttackData.CriticalModificator * Action.Target.BlackBoard.CriticalHitModifier);
                _isCritical = Random.Range(0, 100) < Owner.BlackBoard.criticalChance * _attackAction.data.criticalModificator * _attackAction.target.GetComponent<BlackBoard>().criticalHitModifier;
            }
        }
        else
            _isCritical = false;

        _knockdown = _attackAction.data.hitAreaKnockdown && Random.Range(0, 100) < 60 * Owner.BlackBoard.criticalChance;

        if (Owner.isPlayer == false)
        {
            Owner.BlackBoard.Vigor -= Owner.BlackBoard.vigorAttackCost;
            Owner.BlackBoard.Rage = 0;
            Owner.BlackBoard.Fear += Owner.BlackBoard.fearAttackModificator;
        }
    }
}
