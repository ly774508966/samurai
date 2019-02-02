using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]public class AnimSetPlayer : AnimSet{    
    GameObject      _trails;
    AnimAttackData  _attackKnockdown;
    public AnimAttackData[] AttackData = new AnimAttackData[24];

    void Awake()
    {
        Animation anims = GetComponent<Animation>();

        anims.wrapMode = WrapMode.Once;

        anims["idle"].layer = 0;
        anims["idleSword"].layer = 0;
        anims["run"].layer = 0;
        anims["runSword"].layer = 0;
        anims["walk"].layer = 0;
        anims["walkSword"].layer = 0;

        anims["deathBack"].layer = 2;
        anims["deathFront"].layer = 2;
        
        anims["injuryFrontSword"].layer = 1;
        anims["injuryFrontSword"].speed = 0.9f;
        anims["injuryBackSword"].layer = 1;
        anims["injuryBackSword"].speed = 0.9f;

        anims["evadeSword"].layer = 1;

        anims["showSword"].layer = 0;
        anims["hideSword"].layer = 0;
        anims["showSwordRun"].layer = 0;
        anims["hidSwordRun"].layer = 0;      

        //anims["useLever"].layer = 0;
        // combo XXXXXX
        anims["attackX"].speed = 0.9f;
        anims["attackXX"].speed = 0.8f;
        anims["attackXXX"].speed = 0.8f;
        anims["attackXXXX"].speed = 0.8f;
        anims["attackXXXXX"].speed = 0.8f;

        anims["attackX"].layer = 1;
        anims["attackXX"].layer = 1;
        anims["attackXXX"].layer = 1;
        anims["attackXXXX"].layer = 1;
        anims["attackXXXXX"].layer = 1;
        // combo OOOXX
        anims["attackO"].speed = 1.2f;
        anims["attackOO"].speed = 1.5f;
        anims["attackOOO"].speed = 1.1f;
        anims["attackOOOX"].speed = 1;
        anims["attackOOOXX"].speed = 1.4f;

        anims["attackO"].layer = 1;
        anims["attackOO"].layer = 1;
        anims["attackOOO"].layer = 1;
        anims["attackOOOX"].layer = 1;
        anims["attackOOOXX"].layer = 1;
        // COMBO X00XX
        anims["attackXO"].speed = 1;
        anims["attackXOO"].speed = 1.2f;
        anims["attackXOOX"].speed = 1.2f;
        anims["attackXOOXX"].speed = 1.2f;

        anims["attackXO"].layer = 1;
        anims["attackXOO"].layer = 1;
        anims["attackXOOX"].layer = 1;
        anims["attackXOOXX"].layer = 1;

        // COMBO XX0XX
        anims["attackXXO"].speed = 1;
        anims["attackXXOX"].speed = 1.2f;
        anims["attackXXOXX"].speed = 1.3f;

        anims["attackXXO"].layer = 1;
        anims["attackXXOX"].layer = 1;
        anims["attackXXOXX"].layer = 1;

        // Combo OOXOO
        anims["attackOOX"].speed = 1;
        anims["attackOOXO"].speed = 1;
        anims["attackOOXOO"].speed = 1.3f;

        anims["attackOOX"].layer = 1;
        anims["attackOOXO"].layer = 1;
        anims["attackOOXOO"].layer = 1;

        // COMBO OXOOO
        anims["attackOX"].speed = 1.1f;
        anims["attackOXO"].speed = 1.2f;
        anims["attackOXOX"].speed = 1;
        anims["attackOXOXO"].speed = 1;

        anims["attackOX"].layer = 1;
        anims["attackOXO"].layer = 1;
        anims["attackOXOX"].layer = 1;
        anims["attackOXOXO"].layer = 1;


        _trails = Instantiate(Resources.Load("Player/trails_combo01")) as GameObject;
        _trails.SetActive(false);

        _attackKnockdown = new AnimAttackData("attackKnockdown", null, 1.5f, 0.65f, 0.2f, 0.6f, 1.0f, 20, 0, 0, CriticalHitType.None, 0, false, false, false, true);
        
        AttackData[0] = new AnimAttackData("attackX", _trails.transform.Find("trail_X").gameObject, 0.6f, 0.23f, 0.05f, 0.366f, 0.366f, 3, 15, 0.6f, CriticalHitType.Vertical, 0.2f, false, false, false, false);
        AttackData[1] = new AnimAttackData("attackXX", _trails.transform.Find("trail_XX").gameObject, 0.6f, 0.22f, 0.15f, 0.35f, 0.4f, 5, 15, 0.6f, CriticalHitType.Vertical, 0.25f, false, false, false, false);        
        AttackData[2] = new AnimAttackData("attackXXX", _trails.transform.Find("trail_XXX").gameObject, 0.7f, 0.25f, 0.20f, 0.30f, 0.366f, 10, 45, 0.75f, CriticalHitType.Horizontal, 0.3f, false, false, false, false);        
        AttackData[3] = new AnimAttackData("attackXXXX", _trails.transform.Find("trail_XXXX").gameObject, 0.8f, 0.28f, 0.22f, 0.35f, 0.366f, 12, 90, 0.8f, CriticalHitType.Horizontal, 0.5f, false, false, true, false);
        AttackData[4] = new AnimAttackData("attackXXXXX", _trails.transform.Find("trail_XXXXX").gameObject, 0.8f, 0.3f, 0.15f, 0.35f, 0.366f, 16, 45, 1.5f, CriticalHitType.Vertical, 0.6f, false, false, true, true);
        AttackData[5] = new AnimAttackData("attackO", _trails.transform.Find("trail_O").gameObject, 0.7f, 0.38f, 0.30f, 0.40f, 0.45f, 6, 45, 0.8f, CriticalHitType.Horizontal, 0.1f, false, true, false, false);
        AttackData[6] = new AnimAttackData("attackOO", _trails.transform.Find("trail_OO").gameObject, 0.7f, 0.34f, 0.10f, 0.35f, 0.4f, 10, 45, 0.8f, CriticalHitType.Vertical, 0.15f, false, true, false, false);        
        AttackData[7] = new AnimAttackData("attackOOO", _trails.transform.Find("trail_OOO").gameObject, 1.0f, 0.5f, 0.36f, 0.55f, 0.6f, 15, 15, 1.0f, CriticalHitType.None, 0, false, true, false, false);
        AttackData[8] = new AnimAttackData("attackOOOX", _trails.transform.Find("trail_OOOX").gameObject, 1.0f, 0.45f, 0.15f, 0.45f, 0.533f, 20, 45, 0.5f, CriticalHitType.Vertical, 0.8f, false, true, false, false);
        AttackData[9] = new AnimAttackData("attackOOOXX", _trails.transform.Find("trail_OOOXX").gameObject, 1.0f, 0.51f, 0.15f, 0.6f, 0.7f, 25, 20, 1.8f, CriticalHitType.Vertical, 1, false, true, true, true);        
        AttackData[10] = new AnimAttackData("attackXO", _trails.transform.Find("trail_XO").gameObject, 0.85f, 0.45f, 0.10f, 0.8f, 0.833f, 15, 25, 0.8f, CriticalHitType.None, 0, false, false, false, false);        
        AttackData[11] = new AnimAttackData("attackXOO", _trails.transform.Find("trail_XOO").gameObject, 0.8f, 0.45f, 0.20f, 0.50f, 0.55f, 20, 15, 0.8f, CriticalHitType.None, 0, false, false, false, false);
        AttackData[12] = new AnimAttackData("attackXOOX", _trails.transform.Find("trail_XOOX").gameObject, 0.7f, 0.4f, 0.3f, 0.7f, 0.8f, 20, 180, 1, CriticalHitType.Vertical, 0.8f, false, false, true, false);
        AttackData[13] = new AnimAttackData("attackXOOXX", _trails.transform.Find("trail_XOOXX").gameObject, 1.5f, 0.35f, 0.10f, 0.30f, 0.48f, 25, 20, 1.5f, CriticalHitType.Vertical, 1.0f, false, false, true, true);        
        AttackData[14] = new AnimAttackData("attackXXO", _trails.transform.Find("trail_XXO").gameObject, 0.7f, 0.30f, 0.15f, 0.4f, 0.6f, 15, 90, 1.0f, CriticalHitType.None, 0, true, false, true, true);        
        AttackData[15] = new AnimAttackData("attackXXOX", _trails.transform.Find("trail_XXOX").gameObject, 1.0f, 0.41f, 0.11f, 0.55f, 0.60f, 20, 15, 0.7f, CriticalHitType.Vertical, 1, false, false, true, false);
        AttackData[16] = new AnimAttackData("attackXXOXX", _trails.transform.Find("trail_XXOXX").gameObject, 1.0f, 0.5f, 0.1f, 0.4f, 0.6f, 20, 180, 1.2f, CriticalHitType.None, 0, true, false, true, true);        
        AttackData[17] = new AnimAttackData("attackOOX", _trails.transform.Find("trail_OOX").gameObject, 0.8f, 0.45f, 0.25f, 0.5f, 0.66f, 15, 25, 0.7f, CriticalHitType.Vertical, 1.2f, false, false, true, true);        
        AttackData[18] = new AnimAttackData("attackOOXO", _trails.transform.Find("trail_OOXX").gameObject, 0.7f, 0.6f, 0.2f, 0.6f, 0.7f, 20, 45, 0.7f, CriticalHitType.Vertical, 1.4f, false, false, false, false);
        AttackData[19] = new AnimAttackData("attackOOXOO", _trails.transform.Find("trail_OOXXX").gameObject, 1.0f, 0.45f, 0.05f, 0.65f, 1.03f, 25, 30, 1.5f, CriticalHitType.Vertical, 1.6f, false, false, true, true);        
        AttackData[20] = new AnimAttackData("attackOX", _trails.transform.Find("trail_OX").gameObject, 0.7f, 0.25f, 0.15f, 0.44f, 0.45f, 20, 25, 0.8f, CriticalHitType.Vertical, 0.4f, false, false, true, true);        
        AttackData[21] = new AnimAttackData("attackOXO", _trails.transform.Find("trail_OXO").gameObject, 1.0f, 0.35f, 0.25f, 0.4f, 0.55f, 20, 90, 1, CriticalHitType.Horizontal, 0.5f, false, false, true, false);
        AttackData[22] = new AnimAttackData("attackOXOX", _trails.transform.Find("trail_OXOO").gameObject, 1.0f, 0.35f, 0.15f, 0.3f, 0.5f, 20, 180, 1.2f, CriticalHitType.Horizontal, .7f, false, false, true, false);
        AttackData[23] = new AnimAttackData("attackOXOXO", _trails.transform.Find("trail_OXOOO").gameObject, 1.0f, 0.35f, 0.15f, 0.5f, 1.1f, 25, 180, 1.8f, CriticalHitType.Horizontal, 0.9f, false, false, true, true);
    }

    public override string GetBlockAnim( BlockState state, WeaponType weapon )   {   return null;  }	public override string GetIdleAnim( WeaponType weapon, WeaponState weaponState )	{        if (weaponState == WeaponState.NotInHands)            return "idle";        return "idleSword";	}
    public override string GetIdleActionAnim( WeaponType weapon, WeaponState weaponState )
    {
        return "idle";
    }
    public override string GetMoveAnim( MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState )	{        if (weaponState == WeaponState.NotInHands)        {            if (motion != MotionType.WALK)                return "run";            else                return "walk";        }        if (motion != MotionType.WALK)            return "runSword";
        return "walkSword";	}
    public override string GetRotateAnim( MotionType motionType, RotationType rotationType )
    {
        return null;
    }    public override string GetRollAnim( WeaponType weapon, WeaponState weaponState )    {        return "evadeSword";    }    public override string GetShowWeaponAnim( WeaponType weapon )    {        return  "showSwordRun";    }    public override string GetHideWeaponAnim( WeaponType weapon )    {
        return "hidSwordRun";    }
    public override string GetInjuryAnim( WeaponType weapon, DamageType type )
    {
        if (type == DamageType.Back)
            return "injuryBackSword";
        return "injuryFrontSword";
    }
    public override string GetDeathAnim( WeaponType weapon, DamageType type )
    {
        if(type == DamageType.Back)
            return "deathBack";
        return "deathFront";
    }
    public override string GetKnockdowAnim( KnockdownState block, WeaponType weapon )
    {
        return null;
    }
    public override AnimAttackData GetFirstAttackAnim(WeaponType weapon, AttackType attackType)
    {
        if (attackType == AttackType.Fatality)
            return _attackKnockdown;
        return null;
    }}