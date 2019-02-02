using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    Agent _owner;    

    public float GOAP_BlockRelevancy = 0.7f;
    public float GOAP_GoToRelevancy = 0.3f;
    public float GOAP_StepOutRelevancy = 0.7f;
    public float GOAP_StepInRelevancy = 0.7f;
    public float GOAP_StepAroundRelevancy = 0.75f;
    public float GOAP_AttackTargetRelevancy = 0.8f;
    //public float GOAP_ReactToDamageRelevancy = 1.0f;
    public float GOAP_INJURY = 0.9f;
    public float GOAP_KNOCKDOWN = 0.95f;
    public float GOAP_DEATH = 1f;

    [SerializeField]
    float _vigor;                   // 当前体力值
    public float maxVigor;          // 最大体力值    
    public float vigorModificator = 10;
    public float vigorAttackCost = 50;
    public float Vigor
    {
        get
        {
            return _vigor;
        }
        set
        {
            _vigor = value;
            if (_vigor < 0)
            {
                _vigor = 0;
            }
            else if (_vigor > maxVigor)
            {
                _vigor = maxVigor;
            }
        }
    }

    [SerializeField]
    float _rage;                   // 当前怒气值
    public float maxRage;          // 最大怒气值
    public float rageModificator = 10;
    public float RageInjuryModificator = 10;
    public float RageBlockModificator = 0;
    public float Rage
    {
        get
        {
            return _rage;
        }

        set
        {
            _rage = value;
            if (_rage < 0)
            {
                _rage = 0;
            }
            else if (_rage > maxRage)
            {
                _rage = maxRage;
            }
        }
    }

    [SerializeField]
    float _fear;                     // 当前恐惧值
    public float maxFear;            // 最大恐惧值
    public float fearModificator = 10;
    public float fearInjuryModificator = 5;
    public float fearBlockModificator = 0;
    public float fearAttackModificator = 20;
    public float Fear
    {
        get
        {
            return _fear;
        }

        set
        {
            _fear = value;
            if (_fear < 0)
            {
                _fear = 0;
            }
            else if (_fear > maxFear)
            {
                _fear = maxFear;
            }
        }
    }    

    [System.NonSerialized]
    public AgentAction curAction = null;                                // 当前执行的action
    [System.NonSerialized]
    public MotionType motionType = MotionType.NONE;                     // 动作类型
    [System.NonSerialized]
    public WeaponState weaponState = WeaponState.NotInHands;            // 武器在手状态
    [System.NonSerialized]
    public WeaponType weaponSelected = WeaponType.Katana;               // 武器类型    

    public float weaponRange = 2;
    public float SqrWeaponRange { get { return weaponRange * weaponRange; } }

    public float combatRange = 4;
    public float SqrCombatRange { get { return combatRange * combatRange; } }

    public bool IsBlocking { get { return motionType == MotionType.BLOCK/* || motionType == MotionType.BLOCKING_ATTACK*/; } }
    public bool IsAlive { get { return health > 0 && gameObject.activeSelf; } }    
    public bool IsKnockedDown { get { return motionType == MotionType.KNOCKDOWN/* && knockDownDamageDeadly*/; } }

    public float maxSprintSpeed = 8;
    public float maxRunSpeed = 4;
    public float maxWalkSpeed = 1.5f;
    public float maxCombatMoveSpeed = 1;    
    public float maxKnockdownTime = 4;

    public float speedSmooth = 2.0f;
    public float rotationSmooth = 2.0f;
    public float rotationSmoothInMove = 8.0f;
    public float rollDistance = 4.0f;
    public float moveSpeedModifier = 1;
        
    //[System.NonSerialized]
    public float speed = 0;
    //[System.NonSerialized]
    public Vector3 moveDir;
    [System.NonSerialized]
    public MoveType moveType;    
        
    public float health = 100;
    public float maxHealth = 100;

    [System.NonSerialized]
    public Vector3 desiredPosition;
    [System.NonSerialized]
    public Vector3 desiredDirection;

    public Agent desiredTarget;

    [System.NonSerialized]
    public Agent attacker;
    [System.NonSerialized]
    public WeaponType attackerWeapon;
    [System.NonSerialized]
    public DamageType damageType;
    [System.NonSerialized]
    public Vector3 impuls;

    // Damage settings        
    public bool invulnerable = false;
    public bool criticalAllowed = true;    
    public float criticalHitModifier = 1;
    public float criticalChance = 18;
    public bool damageOnlyFromBack = false;
    //public bool knockDown = true;
    public bool knockDownDamageDeadly = false;
    public BlockResult blockResult = BlockResult.NONE;

    // 目标与我距离
    public float DistanceToTarget
    {
        get
        {
            if (desiredTarget != null)
            {
                Vector3 vec = desiredTarget.Position - transform.position;
                return vec.magnitude;
            }
            else
            {
                return Mathf.Infinity;
            }
        }
    }

    // 目标是否在武器范围内
    public bool InWeaponRange
    {
        get
        {
            if (desiredTarget != null)
            {
                return DistanceToTarget < weaponRange;
            }
            else
            {
                return false;
            }
        }
    }

    // 目标是否在战斗范围内
    public bool InCombatRange
    {
        get
        {
            if (desiredTarget != null)
            {
                return DistanceToTarget < combatRange;
            }
            else
            {
                return false;
            }            
        }
    }

    // 相对目标的角度
    float AngleToTargetForward
    {
        get
        {
            if (desiredTarget != null)
            {
                return Vector3.Angle(desiredTarget.Position - _owner.Position, desiredTarget.Forward);
            }
            else
                return Mathf.Infinity;
        }
    }

    // 目标是否面对（即看着）自己（不论自己的朝向如何）
    public bool AheadOfTarget
    {
        get
        {
            float angle = AngleToTargetForward;
            if (desiredTarget != null)
            {
                return angle > 135 && angle < 225;
            }
            else
            {
                return false;
            }
        }
    }

    // 目标是否背对（即没看见）自己（不论自己的朝向如何）
    public bool BehindTarget
    {
        get
        {
            float angle = AngleToTargetForward;
            if (desiredTarget != null)
            {
                return angle > 315 || angle < 45;
            }
            else
            {
                return false;
            }
        }
    }

    void Awake()
    {
        _owner = GetComponent<Agent>();
    }
}
