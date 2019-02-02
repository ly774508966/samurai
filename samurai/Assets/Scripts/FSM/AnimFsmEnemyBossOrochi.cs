using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmEnemyBossOrochi : Fsm {

	// Use this for initialization
	void Start () {
        /*AnimFsmStateIdle idleState = new AnimFsmStateIdle();
        AddState(idleState);
        AnimFsmStateMove moveState = new AnimFsmStateMove();
        AddState(moveState);
        AnimFsmStateRoll rollState = new AnimFsmStateRoll();
        AddState(rollState);
        AnimFsmStateAttackMelee attackMeleeState = new AnimFsmStateAttackMelee();
        AddState(attackMeleeState);

        CurState = DefState = idleState;*/
    }
	
	// Update is called once per frame
	public override void Loop (AgentAction action)
    {
        base.Loop(action);
	}
}
