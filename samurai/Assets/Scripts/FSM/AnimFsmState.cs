using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFsmState : FsmState
{
    public AnimFsmState(Agent owner) : base(owner)
    {        
        
    }    

    protected void CrossFade(string anim, float fadeInTime)
    {
        //if (Owner.debugAnims) Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " cross fade anim: " + anim + " in " + fadeInTime + "s.");
        if (Owner.AnimEngine.IsPlaying(anim))
            Owner.AnimEngine.CrossFadeQueued(anim, fadeInTime, QueueMode.PlayNow);
        else
            Owner.AnimEngine.CrossFade(anim, fadeInTime);        
    }

    protected bool Move(Vector3 velocity, bool slide /*= true*/ )
    {
        Vector3 old = Owner.Transform.position;

        Owner.Transform.position += Vector3.up * Time.deltaTime;

        velocity.y -= 9 * Time.deltaTime;
        CollisionFlags flags = Owner.CharacterController.Move(velocity);

        //Debug.Log("move " + flags.ToString());
        
        if (slide == false && (flags & CollisionFlags.Sides) != 0)
        {
            Owner.Transform.position = old;
            return false;
        }

        if ((flags & CollisionFlags.Below) == 0)
        {
            Owner.Transform.position = old;
            return false;
        }

        return true;
    }

    protected bool MoveEx(Vector3 velocity)
    {
        Vector3 old = Owner.Transform.position;
        Owner.Transform.position += Vector3.up * Time.deltaTime;
        velocity.y -= 9 * Time.deltaTime;
        CollisionFlags flags = Owner.CharacterController.Move(velocity);
        if (flags == CollisionFlags.None)
        {
            RaycastHit hit;
            if (Physics.Raycast(Owner.Transform.position, -Vector3.up, out hit, 3) == false)
            {
                Owner.Transform.position = old;
                return false;
            }
        }

        return true;
    }
}
