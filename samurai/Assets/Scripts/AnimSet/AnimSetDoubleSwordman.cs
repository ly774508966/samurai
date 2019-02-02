using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSetDoubleSwordman : AnimSet
{
    protected AnimAttackData AnimAttacksSword1;
    protected AnimAttackData AnimAttacksSword2;
    protected AnimAttackData AnimAttackWhirl;

    void Awake()
    {        AnimAttacksSword1 = new AnimAttackData("attackA", null, 1, 0.25f, 0.50f, 7, 20, 1, CriticalHitType.None, false);
        AnimAttacksSword2 = new AnimAttackData("attackAA", null, 1, 0.25f, 0.50f, 7, 20, 1, CriticalHitType.None, false);
        AnimAttackWhirl = new AnimAttackData("attackWhirl", null, 1, 0.25f, 0.40f, 7, 10, 1, CriticalHitType.None, false);
        Animation anims = GetComponent<Animation>();
        anims["idle"].layer = 0;
        anims["idleSword"].layer = 0;
        anims["idleTaunt"].layer = 1;        anims["walk"].layer = 0;        anims["combatMoveF"].layer = 0;
        anims["combatMoveB"].layer = 0;
        anims["combatMoveR"].layer = 0;
        anims["combatMoveL"].layer = 0;
        anims["combatRunF"].layer = 0;
        anims["combatRunB"].layer = 0;

        anims["rotationL"].layer = 0;
        anims["rotationR"].layer = 0;


        anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injury01"].layer = 1;
        anims["injury02"].layer = 1;
        anims["injury03"].layer = 1;
        anims["injuryBack"].layer = 1;

        anims["attackA"].speed = 1.2f;
        anims["attackAA"].speed = 1.2f;

        anims["attackA"].layer = 0;
        anims["attackAA"].layer = 0;
        anims["attackWhirl"].layer = 0;

        // anims["attackA"].speed = 1.3f;
        //anims["attackAA"].speed = 1.3f;

        anims["knockdown"].layer = 0;
        anims["knockdownLoop"].layer = 0;
        anims["knockdownUp"].layer = 0;
        anims["knockdownDeath"].layer = 0;
        anims["showSword"].layer = 0;        anims["hideSword"].layer = 1;

        //        anims["spawn"].layer = 1;


    }

    public override string GetIdleAnim(WeaponType weapon, WeaponState weaponState)
    {        if (weaponState == WeaponState.NotInHands)            return "idle";        return "idleSword";
    }

    public override string GetIdleActionAnim(WeaponType weapon, WeaponState weaponState)
    {
        if (weapon == WeaponType.Katana)
            return "idleTaunt";

        return "idle";
    }

    public override string GetMoveAnim(MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState)    {        if (weaponState == WeaponState.NotInHands)        {            return "walk";        }        else
        {
            if (motion == MotionType.WALK)
            {
                if (move == MoveType.FORWARD)
                    return "combatMoveF";
                else if (move == MoveType.BACKWARD)
                    return "combatMoveB";
                else if (move == MoveType.RIGHTWARD)
                    return "combatMoveR";
                else
                    return "combatMoveL";
            }
            else
            {
                if (move == MoveType.FORWARD)
                    return "combatRunF";
                else if (move == MoveType.BACKWARD)
                    return "combatRunB";
            }        }

        return "idle";    }

    public override string GetRotateAnim(MotionType motionType, RotationType rotationType)
    {
        if (motionType == MotionType.BLOCK)
        {
            if (rotationType == RotationType.LEFT)
                return "blockStepL";

            return "blockStepR";
        }

        if (rotationType == RotationType.LEFT)
            return "rotationL";

        return "rotationR";
    }
    public override string GetRollAnim(WeaponType weapon, WeaponState weaponState) { return null; }


    public override string GetBlockAnim(BlockState state, WeaponType weapon)
    {
        return null;
    }    public override string GetShowWeaponAnim(WeaponType weapon)    {        return "showSword";    }    public override string GetHideWeaponAnim(WeaponType weapon)    {        return "hideSword";    }


    public override AnimAttackData GetFirstAttackAnim(WeaponType weapom, AttackType attackType)
    {
        if (attackType == AttackType.X)
            return AnimAttacksSword1;


        return AnimAttacksSword2;
    }
    

    public override AnimAttackData GetWhirlAttackAnim()
    {
        return AnimAttackWhirl;
    }   


    public override string GetInjuryAnim(WeaponType weapon, DamageType type)
    {

        if (type == DamageType.Back)
            return "injuryBack";

        string[] anims = { "injury01", "injury02", "injury03" };

        return anims[Random.Range(0, anims.Length)];
    }

    public override string GetDeathAnim(WeaponType weapon, DamageType type)
    {
        string[] anims = { "death01", "death02" };

        return anims[Random.Range(0, 100) % anims.Length];
    }

    public override string GetKnockdowAnim(KnockdownState state, WeaponType weapon)
    {
        switch (state)
        {
            case KnockdownState.Down:
                return "knockdown";
            case KnockdownState.Loop:
                return "knockdownLoop";
            case KnockdownState.Up:
                return "knockdownUp";
            case KnockdownState.Fatality:
                return "knockdownDeath";
            default:
                return "";
        }
    }

}
