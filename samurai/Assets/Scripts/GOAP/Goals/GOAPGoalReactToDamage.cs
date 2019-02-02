using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPGoalReactToDamage : GOAPGoal
{

    // Use this for initialization
    public GOAPGoalReactToDamage(Agent owner)
        : base(GOAPGoalType.REACT_TO_DAMAGE, owner)
    {

    }

    public override bool IsSatisfied(WorldState worldState)
    {
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.EVENT);
        return prop.GetEvent() == EventTypes.NONE;
    }

    /* public override void MakeSatisfied(WorldState worldState)
     {
         worldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.NONE);
     }*/

    public override void CalcWeight(WorldState worldState, BlackBoard blackBoard)
    {
        Weight = 0;
        WorldStateProp prop = worldState.GetWSProperty(WorldStatePropKey.EVENT);
        if (prop != null)
        {
            /*if (prop.GetEvent() == EventTypes.HIT || prop.GetEvent() == EventTypes.KNOCKDOWN || prop.GetEvent() == EventTypes.DEAD)
            {                
                Weight = blackBoard.GOAP_ReactToDamageRelevancy;                
            }*/
            switch (prop.GetEvent())
            {                
                case EventTypes.HIT:
                    Weight = blackBoard.GOAP_INJURY;
                    break;
                case EventTypes.DEAD:
                    Weight = blackBoard.GOAP_DEATH;
                    break;                
                case EventTypes.KNOCKDOWN:
                    Weight = blackBoard.GOAP_KNOCKDOWN;
                    break;               
                default:
                    break;
            }
        }
    }
}
