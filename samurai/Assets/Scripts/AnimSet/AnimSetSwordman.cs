using UnityEngine;

[System.Serializable]
public class AnimSetSwordman : AnimSet
{

    protected AnimAttackData AnimAttacksSwordL;
    protected AnimAttackData AnimAttacksSwordCounter;

    void Awake()
    {        AnimAttacksSwordL = new AnimAttackData("attackA", null, 0.7f, 0.30f, 0.68f, 7, 20, 1, CriticalHitType.None, false);
        AnimAttacksSwordCounter = new AnimAttackData("attackCounter", null, 1.0f, 0.65f, 0.4f, 0.68f, 0.7f, 15, 25, 2, CriticalHitType.None, 0, false, false, false, false);

        Animation anims = GetComponent<Animation>();
        anims["idle"].layer = 0;
        anims["idleSword"].layer = 0;        anims["run"].layer = 0;        anims["runSword"].layer = 0;
        anims["runSwordBackward"].layer = 0;
        anims["walk"].layer = 0;        anims["combatMoveF"].layer = 0;
        anims["combatMoveB"].layer = 0;
        anims["combatMoveR"].layer = 0;
        anims["combatMoveL"].layer = 0;

        anims["rotationLeft"].layer = 0;
        anims["rotationRight"].layer = 0;


        anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injuryFront01"].layer = 1;
        anims["injuryFront02"].layer = 1;
        anims["injuryFront03"].layer = 1;
        anims["injuryFront04"].layer = 1;


        anims["blockStart"].layer = 0;
        anims["blockLoop"].layer = 0;
        anims["blockEnd"].layer = 0;
        anims["blockFailed"].layer = 0;
        anims["blockHit"].layer = 0;
        anims["blockStepLeft"].layer = 0;
        anims["blockStepRight"].layer = 0;

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
            return "idleTount";

        return "idle";
    }

    public override string GetMoveAnim(MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState)    {        if (weaponState == WeaponState.NotInHands)        {            if (motion == MotionType.WALK)
                return "walk";            else                return "run";        }        else
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
                    return "runSword";
                else if (move == MoveType.BACKWARD)
                    return "runSwordBackward";
            }        }

        return "idle";    }

    public override string GetRotateAnim(MotionType motionType, RotationType rotationType)
    {
        if (motionType == MotionType.BLOCK)
        {
            if (rotationType == RotationType.LEFT)
                return "blockStepLeft";

            return "blockStepRight";
        }

        if (rotationType == RotationType.LEFT)
            return "rotationLeft";

        return "rotationRight";
    }
    public override string GetRollAnim(WeaponType weapon, WeaponState weaponState) { return null; }    public override string GetBlockAnim(BlockState state, WeaponType weapon)    {
        if (state == BlockState.Start)
            return "blockStart";
        else if (state == BlockState.Loop)
            return "blockLoop";
        else if (state == BlockState.Failed)
            return "blockFailed";
        else if (state == BlockState.HitBlocked)
            return "blockHit";
        else
            return "blockEnd";    }    public override string GetShowWeaponAnim(WeaponType weapon)    {        return "showSword";    }    public override string GetHideWeaponAnim(WeaponType weapon)    {        return "hideSword";    }


    public override AnimAttackData GetFirstAttackAnim(WeaponType weapom, AttackType attackType)
    {
        if (attackType == AttackType.Counter)
            return AnimAttacksSwordCounter;

        return AnimAttacksSwordL;
    }

    public override string GetInjuryAnim(WeaponType weapon, DamageType type)
    {
        string[] anims = { "injuryFront01", "injuryFront02", "injuryFront03", "injuryFront04" };
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
