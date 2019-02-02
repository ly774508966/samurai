using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateAttackWhirl : AnimFsmState
{
    AgentActionAttackWhirl _agentAction;
    float _maxSpeed;
    float _timeToEndState;
    bool _moveOk;

    //CombatEffectsManager.CacheData Effect;
    float TimeToStartEffect;
    float TimeToEndEffect;

    Quaternion _finalRotation = new Quaternion();
    Quaternion _startRotation = new Quaternion();

    float _hitTimer;    
    float _rotationProgress;

    public AnimFsmStateAttackWhirl(Agent owner)
		: base(owner)
	{
    }


    public override void Enter(AgentAction action)
    {
        base.Enter(action);
        Owner.BlackBoard.invulnerable = true;
        //Effect = null;
    }

    public override void Exit()
    {
        Owner.BlackBoard.invulnerable = false;
        Owner.BlackBoard.speed = 0;
        //if (Effect != null)
          //  CombatEffectsManager.Instance.ReturnWhirlEffect(Effect);
        //Effect = null;
        base.Exit();    
    }

    public override void Loop()
    {
        UpdateFinalRotation(); 

        _rotationProgress += Time.deltaTime * Owner.BlackBoard.rotationSmoothInMove;
        _rotationProgress = Mathf.Min(_rotationProgress, 1);
        Quaternion q = Quaternion.Slerp(_startRotation, _finalRotation, _rotationProgress);
        Owner.Transform.rotation = q;

        if (_moveOk && Owner.AnimEngine[_agentAction.data.animName].time > Owner.AnimEngine[_agentAction.data.animName].length * 0.1f)
        {            
            float curSmooth = Owner.BlackBoard.speedSmooth * Time.deltaTime;
            Owner.BlackBoard.speed = Mathfx.Hermite(Owner.BlackBoard.speed, _maxSpeed, curSmooth);
            Owner.BlackBoard.moveDir = Owner.Forward;

            float dist = Owner.BlackBoard.speed * Time.deltaTime;
            _moveOk = Move(Owner.BlackBoard.moveDir * dist, true);

            if (_hitTimer < Time.timeSinceLevelLoad) // 伤害结算计时
            {
                if (Owner.BlackBoard.desiredTarget != null && Owner.BlackBoard.desiredTarget.BlackBoard.IsAlive &&
                    Owner.BlackBoard.desiredTarget.BlackBoard.motionType != MotionType.ROLL && 
                    Owner.BlackBoard.desiredTarget.BlackBoard.invulnerable == false)
                {
                    if ((Owner.BlackBoard.desiredTarget.Position - Owner.Position).sqrMagnitude < Owner.BlackBoard.weaponRange * Owner.BlackBoard.weaponRange)
                    {
                        Owner.BlackBoard.desiredTarget.ReceiveDamage(Owner, WeaponType.Body, _agentAction.data.hitDamage, _agentAction.data);
                        //Owner.SoundPlayHit();
                    }
                }
                
                _hitTimer = Time.timeSinceLevelLoad + 0.75f; 
            }
        }

        /*if (Effect == null && Time.timeSinceLevelLoad > TimeToStartEffect && Time.timeSinceLevelLoad < TimeToEndEffect)
        {
            Effect = CombatEffectsManager.Instance.PlayWhirlEffect(Transform);
        }
        else if (Effect != null && Time.timeSinceLevelLoad > TimeToEndEffect)
        {
            CombatEffectsManager.Instance.ReturnWhirlEffect(Effect);
            Effect = null;
        }*/


        if (_timeToEndState < Time.timeSinceLevelLoad)
            IsFinished = true;
    }


    protected override void Initialize(AgentAction action)
    {
        base.Initialize(action);
        _agentAction = Owner.BlackBoard.curAction as AgentActionAttackWhirl;

        _moveOk = true;
        CrossFade(_agentAction.data.animName, 0.2f);
        UpdateFinalRotation();
        Owner.BlackBoard.motionType = MotionType.WALK;
        _rotationProgress = 0;
        _timeToEndState = Owner.AnimEngine[_agentAction.data.animName].length * 0.9f + Time.timeSinceLevelLoad;
        _hitTimer = Time.timeSinceLevelLoad + 0.75f;

       // Owner.PlayLoopSound(Owner.BerserkSound, 1, AnimEngine[Action.Data.AnimName].length - 1, 0.5f, 0.9f);

        TimeToStartEffect = Time.timeSinceLevelLoad + 1;
        TimeToEndEffect = Time.timeSinceLevelLoad + Owner.AnimEngine[_agentAction.data.animName].length - 1;

        _maxSpeed = 2;
    }


    void UpdateFinalRotation()
    {
        Vector3 dir;
        if (Owner.BlackBoard.desiredTarget != null)
        {
            dir = Owner.BlackBoard.desiredTarget.Position - Owner.Position;
        }
        else
        {
            dir = Owner.Forward;
        }         
        dir.Normalize();

        _finalRotation.SetLookRotation(dir);
        _startRotation = Owner.Transform.rotation;

        if (_startRotation != _finalRotation)
            _rotationProgress = 0;
    }

}
