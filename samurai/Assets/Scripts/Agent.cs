using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public AgentType agentType;

    Fsm         _animFsm;
    PlayerInput _input;
    GOAPManager _goapMgr;
    AudioSource _audioEffect;       // 音效

    public BlackBoard BlackBoard { get; private set; }
    public Animation AnimEngine { get; private set; }
    public Transform Transform { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public AnimSet AnimSet { get; private set; }

    public Vector3 Position { get { return transform.position; } }
    public Vector3 Forward { get { return transform.forward; } }
    public Vector3 Right { get { return transform.right; } }
    public Vector3 ChestPosition { get { return Transform.position + transform.up * 1.5f; } }

    public bool isPlayer = false;

    Queue<AgentAction> _agentActions = new Queue<AgentAction>();

    void Awake()
    {
        BlackBoard = GetComponent<BlackBoard>();
        AnimEngine = GetComponent<Animation>();
        Transform = GetComponent<Transform>();
        CharacterController = GetComponent<CharacterController>();
        AnimSet = GetComponent<AnimSet>();
        _animFsm = GetComponent<Fsm>();
        _input = GetComponent<PlayerInput>();
        _goapMgr = GetComponent<GOAPManager>();
        _audioEffect = GetComponent<AudioSource>();
    }

    public void AddAction(AgentAction agentAction)
    {
        _agentActions.Enqueue(agentAction);
    }

    public void Loop()
    {
        _goapMgr.Loop();

        AgentAction nextAction = null;
        if (isPlayer)
        {
            LoopPlayer(out nextAction);
        }
        else
        {
            LoopNpc(out nextAction);

            BlackBoard.Rage += (BlackBoard.rageModificator * Time.deltaTime);
            if (BlackBoard.AheadOfTarget)
                BlackBoard.Fear += (BlackBoard.fearModificator * Time.deltaTime);
            else
                BlackBoard.Fear -= (BlackBoard.fearModificator * Time.deltaTime);

            BlackBoard.Vigor += (BlackBoard.vigorModificator * Time.deltaTime);
        }
        _animFsm.Loop(nextAction);
    }

    void LoopPlayer(out AgentAction nextAction)
    {
        if (false) // BLACKBOARD有死亡状态
        {
            //nextAction = DEATH行为;
        }
        else if (false) // BLACKBOARD有受伤状态
        {
            //nextAction = INJURY行为;
        }
        else if (_input != null)
        {
            nextAction = _input.GetInputAction(); // 获得玩家输入            
        }
        else
            nextAction = null;
    }
    void LoopNpc(out AgentAction nextAction)
    {
        if (_agentActions.Count > 0)
        {
            nextAction = _agentActions.Dequeue();
        }
        else
            nextAction = null;        
    }

    public void DoDamageFatality(Agent mainTarget, WeaponType byWeapon, AnimAttackData data)
    {
        if (mainTarget.BlackBoard.IsAlive == false || mainTarget.enabled == false)
            return;

        mainTarget.ReceiveDamage(this, byWeapon, 1000, data);
    }

    public void DoMeleeDamage(Agent mainTarget, WeaponType byWeapon, AnimAttackData data, bool critical, bool knockdown)
    {
        if (mainTarget == null)
            return;

        if (isPlayer)
        {
            EnemiesRecvDamage(mainTarget, byWeapon, data, critical, knockdown);
        }
        else
        {
            bool hit = false;
            Vector3 dirToEnemy;
            Vector3 attackerDir = this.Forward;            

            if (mainTarget.BlackBoard.invulnerable == false && mainTarget.BlackBoard.motionType != MotionType.ROLL)
            {
                dirToEnemy = mainTarget.Position - this.Position;

                float len = dirToEnemy.sqrMagnitude;

                if (len < this.BlackBoard.SqrWeaponRange)
                {
                    dirToEnemy.Normalize();

                    if (len < 0.5f * 0.5f || data.hitAngle == -1 || Vector3.Angle(attackerDir, dirToEnemy) < data.hitAngle)
                    {
                        mainTarget.ReceiveDamage(this, byWeapon, data.hitDamage, data);
                        hit = true;
                    }
                }
            }

            //if (hit)
                //attacker.SoundPlayHit();
            //else
                //attacker.SoundPlayMiss();
        }
    }

    void EnemiesRecvDamage(Agent mainTarget, WeaponType byWeapon, AnimAttackData data, bool critical, bool knockdown)
    {
        bool hit = false;
        bool block = false;
        bool knock = false;

        Vector3 dirToEnemy;
        Vector3 center = this.Position;
        Vector3 attackerDir = this.Forward;
        Agent enemy;

        List<Agent> enemies = Game.Instance.agents;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemy = enemies[i];

            if (enemy == this || enemy.BlackBoard.IsAlive == false || enemy.enabled == false || enemy.BlackBoard.IsKnockedDown)
                continue;            

            dirToEnemy = enemy.Position - center;
            float len = dirToEnemy.magnitude;
            dirToEnemy.Normalize();

            if (enemy.BlackBoard.invulnerable || (enemy.BlackBoard.damageOnlyFromBack && Vector3.Angle(attackerDir, enemy.Forward) > 80))
            {                
                enemy.ReceiveHitCompletelyBlocked(this);
                block = true;
                continue;
            }

            if (len > this.BlackBoard.weaponRange)
            {
                if (data.hitAreaKnockdown == true && knockdown && len < this.BlackBoard.weaponRange * 1.2f)
                {
                    knock = true;
                    enemy.ReceiveKnockDown(this, dirToEnemy * data.hitMomentum);
                }
                else if (data.useImpuls && len < this.BlackBoard.weaponRange * 1.4f)
                {
                    enemy.ReceiveImpuls(this, dirToEnemy * data.hitMomentum);
                }
                continue; //too far
            }

            if (len > 0.5f && Vector3.Angle(attackerDir, dirToEnemy) > data.hitAngle)
            {

                if (data.useImpuls)
                {                    
                    enemy.ReceiveImpuls(this, dirToEnemy * data.hitMomentum);
                }
                continue;
            }

            if (enemy.BlackBoard.criticalAllowed && data.hitCriticalType != CriticalHitType.None && 
                Vector3.Angle(attackerDir, enemy.Forward) < 45) // from behind
            {                
                enemy.ReceiveCriticalHit(enemy, this, data.hitCriticalType, false);
                hit = true;
            }
            else if (enemy.BlackBoard.IsBlocking)
            {             
                enemy.ReceiveBlockedHit(this, byWeapon, data.hitDamage, data);
                block = true;
            }
            else if (enemy.BlackBoard.criticalAllowed && critical && 
                (mainTarget == enemy || (data.hitCriticalType == CriticalHitType.Horizontal && Random.Range(0, 100) < 30)))
            {                
                enemy.ReceiveCriticalHit(enemy, this, data.hitCriticalType, false);
                hit = true;
            }
            else if (data.hitAreaKnockdown == true && knockdown)
            {             
                enemy.ReceiveKnockDown(this, dirToEnemy * (1 - (len / this.BlackBoard.weaponRange) + data.hitMomentum));
                knock = true;
            }
            else
            {                
                enemy.ReceiveDamage(this, byWeapon, data.hitDamage, data);                
                hit = true;
            }            
        }

        /*if (knock)
            attacker.SoundPlayKnockdown();
        else if (block)
            attacker.SoundPlayBlockHit();
        else if (hit)
            attacker.SoundPlayHit();
        else
            attacker.SoundPlayMiss();
            */
    }

    public void ReceiveHitCompletelyBlocked(Agent attacker)
    {
        CombatEffectMgr.Instance.PlayBlockHitEffect(ChestPosition, -attacker.Forward);
        /*BlackBoard.Berserk += BlackBoard.BerserkBlockModificator;
        BlackBoard.Rage += BlackBoard.RageBlockModificator;
        if (attacker.IsPlayer)
            Game.Instance.NumberOfBlockedHits++;*/
    }

    public void ReceiveBlockedHit(Agent attacker, WeaponType byWeapon, float damage, AnimAttackData data)
    {
        BlackBoard.attacker = attacker;
        BlackBoard.attackerWeapon = byWeapon;   
        //_goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.BLOCK_BROKEN);

        bool fromBehind = Vector3.Dot(attacker.Forward, Forward) > -0.1f;
        if (fromBehind)
        {
            BlackBoard.blockResult = BlockResult.FAIL;
            BlackBoard.health = Mathf.Max(1, BlackBoard.health - damage);
            BlackBoard.damageType = DamageType.BreakBlock; // 抵挡破防失败（扣血）
            CombatEffectMgr.Instance.PlayBloodEffect(Transform.position, -attacker.Forward);
            //SpriteEffectsManager.Instance.CreateBlood(Transform);
        }
        else
        {            
            if (data.breakBlock) // 如果对方招式有破防效果
            {
                BlackBoard.blockResult = BlockResult.FAIL;
                BlackBoard.damageType = DamageType.BreakBlock; // 抵挡破防失败（但不扣血）
                //if (attacker.isPlayer)
                  //  Game.Instance.NumberOfBreakBlocks++;
                //CombatEffectsManager.Instance.PlayBlockBreakEffect(Transform.position, -attacker.Forward);
            }
            else
            {
                BlackBoard.blockResult = BlockResult.SUCCESS;
                BlackBoard.damageType = DamageType.Front; // 抵挡破防成功
                //if (attacker.isPlayer)
                  //  Game.Instance.NumberOfBlockedHits++;
                CombatEffectMgr.Instance.PlayBlockHitEffect(ChestPosition, -attacker.Forward);
            }
        }
    }

    public void ReceiveImpuls(Agent attacker, Vector3 impuls)
    {        
        BlackBoard.attacker = attacker;
        BlackBoard.attackerWeapon = WeaponType.None;
        BlackBoard.impuls = impuls;
        BlackBoard.damageType = DamageType.Front;
        _goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.HIT);
    }

    public void ReceiveDamage(Agent attacker, WeaponType byWeapon, float damage, AnimAttackData data)
    {
        if (BlackBoard.IsAlive == false)
            return;

        if (attacker.isPlayer)
        {
            //Game.Instance.Hits += 1; 连击次数            
        }       

        BlackBoard.attacker = attacker;
        BlackBoard.attackerWeapon = byWeapon;
        BlackBoard.impuls = attacker.Forward * data.hitMomentum;

        if (BlackBoard.IsKnockedDown)
        { 
            BlackBoard.health = 0;
            BlackBoard.damageType = DamageType.InKnockdown;
            //_goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.DEAD);            
            //CombatEffectsManager.Instance.PlayCriticalEffect(Transform.position, -attacker.Forward);
            //StartCoroutine(Fadeout(3));
            //SoundPlay(SoundDataManager.Instance.FatalitySound);
            if (attacker.isPlayer)
            {
                //Game.Instance.Score += Experience;
                //Player.Instance.AddExperience(Experience, 1.5f + Game.Instance.Hits * 0.1f);
            }
        }
        else
        {
            BlackBoard.health = Mathf.Max(0, BlackBoard.health - damage);
            BlackBoard.damageType = DamageType.Front;            

            if (BlackBoard.IsAlive)
            {
                _goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.HIT);
                //SpriteEffectsManager.Instance.CreateBlood(Transform);
            }
            else
            {
                _goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.DEAD);
                //StartCoroutine(Fadeout(3));

                if (attacker.isPlayer)
                {
                    //Game.Instance.Score += Experience;
                    //Player.Instance.AddExperience(Experience, 1 + Game.Instance.Hits * 0.1f);
                }
            }

            if (damage >= 15)
                CombatEffectMgr.Instance.PlayBloodBigEffect(Transform.position, -attacker.Forward);
            else
                CombatEffectMgr.Instance.PlayBloodEffect(Transform.position, -attacker.Forward);

        }
    }

    public void ReceiveKnockDown(Agent attacker, Vector3 impuls)
    {
        if (BlackBoard.IsAlive == false/* || BlackBoard.knockDown == false*/)
            return;

        if (attacker.isPlayer)
        {
            //Game.Instance.Hits += 1;
            //Game.Instance.NumberOfKnockdowns++;
        }
        
        BlackBoard.attacker = attacker;
        BlackBoard.impuls = impuls;

        _goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.KNOCKDOWN);
        //CombatEffectsManager.Instance.PlayKnockdownEffect(Transform.position, -attacker.Forward);
    }

    public void ReceiveCriticalHit(Agent hitAgent, Agent attacker, CriticalHitType type, bool effectOnly/* = false*/)
    {        
        if (attacker.isPlayer)
        {
            //Game.Instance.Hits += 1;
            //Game.Instance.Score += Experience;
            //Player.Instance.AddExperience(Experience, 1.5f + Game.Instance.Hits * 0.1f);
            //Game.Instance.NumberOfCriticals++;
        }

        //BlackBoard.stop = true;
        BlackBoard.health = 0;
        
        if (type == CriticalHitType.Horizontal)
        {
            int r = Random.Range(0, 100);
            if (r < 33)
                ChoppedBodyFactory.Instance.Get(agentType, hitAgent.Transform, ChoppedBodyType.LEGS);
            else if (r < 66)
                ChoppedBodyFactory.Instance.Get(agentType, hitAgent.Transform, ChoppedBodyType.BEHEADED);
            else
                ChoppedBodyFactory.Instance.Get(agentType, hitAgent.Transform, ChoppedBodyType.HALF_BODY);
        }
        else
        {
            float dot = Vector3.Dot(Forward, attacker.Forward);

            if (dot < 0.5 && dot > -0.5f)
                ChoppedBodyFactory.Instance.Get(agentType, hitAgent.Transform, ChoppedBodyType.SLICE_LEFT_RIGHT);
            else
                ChoppedBodyFactory.Instance.Get(agentType, hitAgent.Transform, ChoppedBodyType.SLICE_FRONT_BACK);
        }

        //CombatEffectsManager.Instance.PlayCriticalEffect(Transform.position, -attacker.Forward);
        //Mission.Instance.ReturnHuman(hitAgent.gameObject);
        hitAgent.gameObject.SetActive(false); // 临时这么写着
    }

    public void PlaySound(AudioClip clip)
    {
        if (_audioEffect && clip)
            _audioEffect.PlayOneShot(clip);
    }

    public void PlaySoundLoop(AudioClip clip, float delay, float time, float fadeInTime, float fadeOutTime)
    {
        StartCoroutine(PlaySoundLoopImp(clip, delay, time, fadeInTime, fadeOutTime));
    }

    IEnumerator PlaySoundLoopImp(AudioClip clip, float delay, float time/*播放时长*/, float fadeInTime, float fadeOutTime)
    {
        if (_audioEffect == null || clip == null)
        {
            yield break;
        }

        _audioEffect.volume = 0;
        _audioEffect.loop = true;
        _audioEffect.clip = clip;

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(delay);

        _audioEffect.Play();

        float step = 1 / fadeInTime;
        while (_audioEffect.volume < 1)
        {
            _audioEffect.volume = Mathf.Min(1.0f, _audioEffect.volume + step * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time - fadeInTime - fadeOutTime);

        step = 1 / fadeInTime;
        while (_audioEffect.volume > 0)
        {
            _audioEffect.volume = Mathf.Max(0.0f, _audioEffect.volume - step * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _audioEffect.Stop();

        yield return new WaitForEndOfFrame();

        _audioEffect.volume = 1;
    }
}
