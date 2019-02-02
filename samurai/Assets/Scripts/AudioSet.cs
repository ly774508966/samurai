using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSet : MonoBehaviour
{
    public AudioClip[] ControlSounds = null;
    public AudioClip KnockDownSound = null;
    public AudioClip FatalitySound = null;
    public AudioClip KatanaOnSound = null;
    public AudioClip KatanaOffSound = null;

    public AudioClip[] SpawnSounds = null;
    public AudioClip[] PrepareAttackSounds = null;
    public AudioClip[] BerserkSounds = null;
    public AudioClip[] StepSounds = null;
    public AudioClip[] RollSounds = null;

    public AudioClip[] AttackMissSounds = null;
    public AudioClip[] AttackHitSounds = null;
    public AudioClip[] AttackBlockSounds = null;

    public AudioClip WeaponOn = null;
    public AudioClip WeaponOff = null;

    public AudioClip ControlSound
    {
        get
        {
            return ControlSounds[Random.Range(0, ControlSounds.Length)];
        }
    }
    public AudioClip SpawnSound
    {
        get
        {
            if (SpawnSounds == null || SpawnSounds.Length == 0)
                return null;
            return SpawnSounds[Random.Range(0, SpawnSounds.Length)];
        }
    }
    public AudioClip StepSound
    {
        get
        {
            if (StepSounds == null || StepSounds.Length == 0)
                return null;
            return StepSounds[Random.Range(0, StepSounds.Length)];
        }
    }
    public AudioClip RollSound
    {
        get
        {
            if (RollSounds == null || RollSounds.Length == 0)
                return null;
            return RollSounds[Random.Range(0, RollSounds.Length)];
        }
    }
    public AudioClip PrepareAttackSound
    {
        get
        {
            if (PrepareAttackSounds == null || PrepareAttackSounds.Length == 0)
                return null;
            return PrepareAttackSounds[Random.Range(0, PrepareAttackSounds.Length)];
        }
    }
    public AudioClip BerserkSound
    {
        get
        {
            if (BerserkSounds == null || BerserkSounds.Length == 0)
                return null;
            return BerserkSounds[Random.Range(0, BerserkSounds.Length)];
        }
    }
    public AudioClip AttackMissSound
    {
        get
        {
            if (AttackMissSounds == null || AttackMissSounds.Length == 0)
                return null;
            return AttackMissSounds[Random.Range(0, AttackMissSounds.Length)];
        }
    }
    public AudioClip AttackHitSound
    {
        get
        {
            if (AttackHitSounds == null || AttackHitSounds.Length == 0)
                return null;
            return AttackHitSounds[Random.Range(0, AttackHitSounds.Length)];
        }
    }
    public AudioClip AttackBlockSound
    {
        get
        {
            if (AttackBlockSounds == null || AttackBlockSounds.Length == 0)
                return null;
            return AttackBlockSounds[Random.Range(0, AttackBlockSounds.Length)];
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
