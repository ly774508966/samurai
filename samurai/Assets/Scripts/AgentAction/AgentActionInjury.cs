using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionInjury : AgentAction
{
    public Agent attacker;
    public WeaponType fromWeapon;
    public DamageType damageType;
    public Vector3 impuls;

    public AgentActionInjury(Agent owner) : base(AgentActionType.INJURY, owner)
    {
        Reset(owner);
    }
    
    public override void Reset(Agent agent)
    {
        attacker = null;
        impuls = Vector3.zero;
        base.Reset(agent);     
    }
}
