using UnityEngine;

public abstract class AnimSet : MonoBehaviour
{
	public abstract string GetIdleAnim( WeaponType weapon, WeaponState weaponState );
    public abstract string GetIdleActionAnim( WeaponType weapon, WeaponState weaponState );
    public abstract string GetMoveAnim( MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState );
    public abstract string GetRotateAnim( MotionType motionType, RotationType rotationType );
    public abstract string GetRollAnim( WeaponType weapon, WeaponState weaponState );
    public abstract string GetBlockAnim( BlockState block, WeaponType weapon );
    public abstract string GetKnockdowAnim( KnockdownState block, WeaponType weapon );
    public abstract string GetShowWeaponAnim( WeaponType weapon );
    public abstract string GetHideWeaponAnim( WeaponType weapon );    
    public virtual string GetInjuryPhaseAnim(int phase) { return null; }
    public abstract string GetInjuryAnim( WeaponType weapon, DamageType type );
    public abstract string GetDeathAnim( WeaponType weapon, DamageType type );
    public virtual AnimAttackData GetFirstAttackAnim(WeaponType weapon, AttackType attackType) { return null; }
    public virtual AnimAttackData GetWhirlAttackAnim() { return null; }
}
