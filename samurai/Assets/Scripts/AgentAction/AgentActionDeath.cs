using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionDeath : AgentAction
{
    public Agent attacker;    
    public WeaponType fromWeapon;
    public DamageType damageType;
    public Vector3 impuls;

    public AgentActionDeath(Agent owner) : base(AgentActionType.DEATH, owner)
    {
 
    }

    public override void Reset(Agent agent)
    {
        attacker = null;
        impuls = Vector3.zero;
        base.Reset(agent);
    }
}
