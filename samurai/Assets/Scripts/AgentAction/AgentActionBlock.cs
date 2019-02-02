using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActionBlock : AgentAction
{
    public Agent attacker;
    public WeaponType fromWeapon;
    public float time;

    public AgentActionBlock(Agent owner) : base(AgentActionType.BLOCK, owner)
    {
        Reset(owner);
    }

    public override void Reset(Agent agent)
    {
        base.Reset(agent);
        fromWeapon = WeaponType.None;
        attacker = null;
    }
}
