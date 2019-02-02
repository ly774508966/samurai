using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmStateIdle : AnimFsmState
{
    float       _timeToFinishWeapon = 0;  // 拔刀/收刀时间

    Quaternion FinalRotation;
    Quaternion StartRotation;
    float CurrentRotationTime = 0;
    float RotationTime = 0;
    float EndOfStateTime = 0;
    string AnimName;

    public AnimFsmStateIdle(Agent owner) : base(owner)
    {
        
    }

    void PlayIdleAnim()
    {
        //Debug.Log(Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState).ToString());
        string name = Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState);
        CrossFade(name, 0.2f);
    }

    protected override void Initialize(AgentAction action)
    {
        //Debug.Log("idle");
        base.Initialize(action);

        Owner.BlackBoard.motionType = MotionType.NONE;
        Owner.BlackBoard.moveDir = Vector3.zero;
        Owner.BlackBoard.speed = 0;

        PlayIdleAnim();
    }
    override public void Loop()
    {
        /*if (_timeToFinishWeapon == 0)
        {
            if (Game.Instance.hasEnemy && BlackBoard.weaponState != WeaponState.Ready)
            {
                // 拔刀
                string name = AnimSet.GetShowWeaponAnim(BlackBoard.weaponSelected);
                _timeToFinishWeapon = Time.timeSinceLevelLoad + AnimEngine[name].length * 0.8f;
                AnimEngine.CrossFade(name, 0.1f);
                BlackBoard.weaponState = WeaponState.Ready;
            }
            if (Game.Instance.hasEnemy == false && BlackBoard.weaponState != WeaponState.NotInHands)
            {
                // 收刀
                string name = AnimSet.GetHideWeaponAnim(BlackBoard.weaponSelected);
                _timeToFinishWeapon = Time.timeSinceLevelLoad + AnimEngine[name].length * 0.8f;
                AnimEngine.CrossFade(name, 0.1f);
                BlackBoard.weaponState = WeaponState.NotInHands;
            }
        }
        else if (_timeToFinishWeapon < Time.timeSinceLevelLoad)
        {            
            CrossFade(AnimSet.GetIdleAnim(BlackBoard.weaponSelected, BlackBoard.weaponState), 0.2f);
            _timeToFinishWeapon = 0;
        }*/

        if (_timeToFinishWeapon == 0)
        {
            if (Owner.BlackBoard.desiredTarget != null)
            {
                if (Owner.BlackBoard.weaponState != WeaponState.Ready)
                {
                    // 拔刀
                    string name = Owner.AnimSet.GetShowWeaponAnim(Owner.BlackBoard.weaponSelected);
                    _timeToFinishWeapon = Time.timeSinceLevelLoad + Owner.AnimEngine[name].length * 0.8f;
                    Owner.AnimEngine.CrossFade(name, 0.1f);
                    Owner.BlackBoard.weaponState = WeaponState.Ready;
                }
                else if (Owner.isPlayer == false)
                {
                    RotateToTarget();
                }                
            }
            else if (Owner.BlackBoard.weaponState != WeaponState.NotInHands)
            {
                // 收刀
                string name = Owner.AnimSet.GetHideWeaponAnim(Owner.BlackBoard.weaponSelected);
                _timeToFinishWeapon = Time.timeSinceLevelLoad + Owner.AnimEngine[name].length * 0.8f;
                Owner.AnimEngine.CrossFade(name, 0.1f);
                Owner.BlackBoard.weaponState = WeaponState.NotInHands;
            }
        }
        else if (_timeToFinishWeapon < Time.timeSinceLevelLoad) // 拔刀或者收刀完毕
        {
            _timeToFinishWeapon = 0;
            CrossFade(Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState), 0.2f); // 播放待机动作         
        }

        if (Owner.isPlayer == false)
        {
            //Owner.BlackBoard.Vigor = Owner.BlackBoard.Vigor + 0.2f;
        }
    }

    void RotateToTarget()
    {
        if (EndOfStateTime == 0)
        {
            StartRotation = Owner.Transform.rotation;

            Vector3 finalDir;
            if (Owner.BlackBoard.desiredTarget != null)
            {
                finalDir = (Owner.BlackBoard.desiredTarget.Position + (Owner.BlackBoard.desiredTarget.GetComponent<BlackBoard>().moveDir * Owner.BlackBoard.desiredTarget.GetComponent<BlackBoard>().speed * 0.5f)) - Owner.Transform.position;
                finalDir.Normalize();
            }
            else
            {
                finalDir = Owner.Transform.forward;
            }
            
            FinalRotation.SetLookRotation(finalDir);
            float rotateAngle = Vector3.Angle(Owner.Transform.forward, finalDir);
            if (rotateAngle < 30)
            {
                return;
            }
            RotationTime = rotateAngle / (360.0f * Owner.BlackBoard.rotationSmooth);
            //if (RotationTime == 0)
            //{                
              //  return;
            //}

            if (Vector3.Dot(finalDir, Owner.Transform.right) > 0)
                AnimName = Owner.AnimSet.GetRotateAnim(Owner.BlackBoard.motionType, RotationType.RIGHT);
            else
                AnimName = Owner.AnimSet.GetRotateAnim(Owner.BlackBoard.motionType, RotationType.LEFT);
            CrossFade(AnimName, 0.01f);

            float animLen = Owner.AnimEngine[AnimName].length;
            int steps = Mathf.CeilToInt(RotationTime / animLen);
            EndOfStateTime = Owner.AnimEngine[AnimName].length * steps + Time.timeSinceLevelLoad;
        }
        else
        {
            CurrentRotationTime += Time.deltaTime * 0.5f;
            if (CurrentRotationTime >= RotationTime)
            {
                CurrentRotationTime = RotationTime;
            }

            float progress = CurrentRotationTime / RotationTime;
            Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, Mathfx.Hermite(0, 1, progress));
            Owner.Transform.rotation = q;
            
            if (EndOfStateTime <= Time.timeSinceLevelLoad)
            {
                EndOfStateTime = 0;
                CrossFade(Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.weaponSelected, Owner.BlackBoard.weaponState), 0.2f); // 播放待机动作
            }
        }                
    }
}