using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAttackData
{
    public string animName;
    public float moveDistance;// best attack distance

    //timers
    public float attackMoveStartTime;
    public float attackMoveEndTime;
    public float attackEndTime;    

    // hit parameters
    public float hitTime;
    public float hitDamage;
    public float hitAngle;
    public float hitMomentum;
    public CriticalHitType hitCriticalType;
    public bool hitAreaKnockdown;
    public bool breakBlock;
    public bool useImpuls;
    public float criticalModificator = 1;
    public bool sloMotion;

    // trail
    public GameObject trail;
    public Transform trailParenTrans;
    public GameObject dust;
    public Animation animationDust;
    public Animation anim;
    public Material material;
    public Material materialDust;

    // combo
    public bool firstAttackInCombo = true;
    public bool lastAttackInCombo = false;
    public int comboIndex;
    public bool fullCombo;
    public int comboStep;

    public AnimAttackData(string animName, GameObject trail, float moveDistance, float hitTime, float attackEndTime, float hitDamage, 
        float hitAngle, float hitMomentum, CriticalHitType criticalType, bool areaKnockDown)
    {
        this.animName = animName;
        this.trail = trail;

        if (this.trail)
        {
            trailParenTrans = this.trail.transform.parent;

            if (this.trail.transform.Find("dust"))
            {
                dust = this.trail.transform.Find("dust").gameObject;
                animationDust = dust.GetComponent<Animation>();
                materialDust = dust.GetComponent<Renderer>().material;
            }

            anim = this.trail.transform.parent.GetComponent<Animation>();
            if (this.trail.GetComponentInChildren(typeof(Renderer)))
                material = (this.trail.GetComponentInChildren(typeof(Renderer)) as Renderer).material;
            else if (this.trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)))
                material = (this.trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer).material;
            else
                material = null;

            if (material == null)
                Debug.LogError("Trail - no Material");
        }
        else
        {
            anim = null;
            material = null;
        }

        this.moveDistance = moveDistance;

        this.attackEndTime = attackEndTime;
        attackMoveStartTime = 0;
        attackMoveEndTime = this.attackEndTime * 0.7f;

        this.hitTime = hitTime;
        this.hitDamage = hitDamage;
        this.hitAngle = hitAngle;
        this.hitMomentum = hitMomentum;
        hitCriticalType = criticalType;
        hitAreaKnockdown = areaKnockDown;
        breakBlock = false;
        useImpuls = false;
        criticalModificator = 1;

    }

    public AnimAttackData(string animName, GameObject trail, float moveDistance, float hitTime, float moveStartTime, float moveEndTime, 
        float attackEndTime, float hitDamage, float hitAngle, float hitMomentum, CriticalHitType criticalType, float criticalMod, 
        bool areaKnockDown, bool breakBlock, bool useImpuls, bool sloMotion)
    {
        this.animName = animName;
        this.trail = trail;

        if (this.trail)
        {
            trailParenTrans = this.trail.transform.parent;

            if (this.trail.transform.Find("dust"))
            {
                dust = this.trail.transform.Find("dust").gameObject;
                animationDust = dust.GetComponent<Animation>();
                materialDust = dust.GetComponent<Renderer>().material;
            }

            anim = this.trail.transform.parent.GetComponent<Animation>();
            if (this.trail.GetComponent(typeof(Renderer)))
                material = (this.trail.GetComponent(typeof(Renderer)) as Renderer).material;
            else if (this.trail.GetComponentInChildren(typeof(Renderer)))
                material = (this.trail.GetComponentInChildren(typeof(Renderer)) as Renderer).material;
            else if (this.trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)))
                material = (this.trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer).material;
            else
                material = null;

            if (material == null)
                Debug.LogError("Trail - no Material");
        }
        else
        {
            anim = null;
            material = null;
        }

        this.moveDistance = moveDistance;

        attackMoveStartTime = moveStartTime;
        attackMoveEndTime = moveEndTime;
        this.attackEndTime = attackEndTime;

        this.hitTime = hitTime;
        this.hitDamage = hitDamage;
        this.hitAngle = hitAngle;
        this.hitMomentum = hitMomentum;
        hitCriticalType = criticalType;
        hitAreaKnockdown = areaKnockDown;
        this.breakBlock = breakBlock;
        this.useImpuls = useImpuls;
        criticalModificator = criticalMod;
        this.sloMotion = sloMotion;
    }


    override public string ToString() { return base.ToString() + ": " + animName; }
}
