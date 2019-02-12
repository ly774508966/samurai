using UnityEngine;using System.Collections;using System.Collections.Generic;


public class CombatEffectMgr : MonoBehaviour
{
    public class CombatEffectData
    {
        public GameObject go;
        public ParticleSystem[] emitters;
        //public Transform tran;
    }

    [System.Serializable]
    public class CombatEffect
    {
        private Queue<CombatEffectData> _cache = new Queue<CombatEffectData>();
        private List<CombatEffectData> _inUse = new List<CombatEffectData>();
        public GameObject prefab;

        public void Init(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CombatEffectData c = new CombatEffectData();
                c.go = GameObject.Instantiate(prefab) as GameObject;

                c.emitters = c.go.GetComponentsInChildren<ParticleSystem>();
                //c.tran = c.go.transform;
                _cache.Enqueue(c);
                c.go.SetActive(false);
            }
        }

        public void Update()
        {
            for (int i = _inUse.Count - 1; i >= 0; i--)
            {
                CombatEffectData c = _inUse[i];
                for (int ii = 0; ii < c.emitters.Length; ii++)
                {
                    if (c.emitters[ii].IsAlive() == false)
                    {
                        c.go.transform.parent = null;
                        _cache.Enqueue(_inUse[i]);
                        _inUse.RemoveAt(i);
                        c.go.SetActive(false);
                    }
                }                
            }
        }

        public CombatEffectData Get()
        {
            if (_cache.Count == 0)
                Init(2);

            return _cache.Dequeue();
        }

        public void Return(CombatEffectData c)
        {
            _inUse.Add(c);
        }

        public void Play(Vector3 pos, Vector3 dir)
        {
            if (_cache.Count == 0)
                Init(2);

            CombatEffectData c = _cache.Dequeue();
            _inUse.Add(c);

            c.go.SetActive(true);
            c.go.transform.position = pos;
            c.go.transform.rotation.SetLookRotation(dir);

            for (int i = 0; i < c.emitters.Length; i++)
                c.emitters[i].Play();
        }
    }

    static public CombatEffectMgr Instance = null;

    [SerializeField]
    CombatEffect Blood;
    [SerializeField]
    CombatEffect BloodBig;
    [SerializeField]
    CombatEffect BlockHit;
    [SerializeField]
    CombatEffect BlockBreak;
    [SerializeField]
    CombatEffect Critical;
    [SerializeField]
    CombatEffect Knockdown;

    [SerializeField]
    CombatEffect Spawn;
    [SerializeField]
    CombatEffect Disappear;
    [SerializeField]
    CombatEffect Whirl;
    [SerializeField]
    CombatEffect Roll;    

    void Awake()
    {        
        Instance = this;

        Blood.Init(10);
        BloodBig.Init(10);
        BlockHit.Init(5);
        BlockBreak.Init(3);
        Critical.Init(4);
        Knockdown.Init(4);

        Whirl.Init(3);
        Roll.Init(3);

        Spawn.Init(5);
        Disappear.Init(5);
    }

    void LateUpdate()
    {
        Blood.Update();
        BloodBig.Update();
        BlockHit.Update();
        BlockBreak.Update();
        Critical.Update();
        Knockdown.Update();
        Whirl.Update();
        Roll.Update();
        Spawn.Update();
        Disappear.Update();
    }

    public void PlayBloodEffect(Vector3 pos, Vector3 dir)
    {
        Blood.Play(pos, dir);
    }

    public void PlayBloodBigEffect(Vector3 pos, Vector3 dir)
    {
        BloodBig.Play(pos, dir);
    }

    public void PlayBlockHitEffect(Vector3 pos, Vector3 dir)
    {
        BlockHit.Play(pos, dir);
    }

    public void PlayBlockBreakEffect(Vector3 pos, Vector3 dir)
    {
        BlockBreak.Play(pos, dir);
    }

    public void PlayCriticalEffect(Vector3 pos, Vector3 dir)
    {
        Critical.Play(pos, dir);
    }

    public void PlayKnockdownEffect(Vector3 pos, Vector3 dir)
    {
        Knockdown.Play(pos, dir);
    }

    public void PlaySpawnEffect(Vector3 pos, Vector3 dir)
    {
        Spawn.Play(pos, dir);
    }

    public void PlayDisappearEffect(Vector3 pos, Vector3 dir)
    {
        Disappear.Play(pos, dir);
    }

    public CombatEffectData PlayWhirlEffect(Transform parent)
    {
        CombatEffectData c = Whirl.Get();

        c.go.transform.parent = parent;
        c.go.transform.position = parent.position + new Vector3(0, 1.6f, 0);
        c.go.transform.forward = parent.forward;

        for (int i = 0; i < c.emitters.Length; i++)
            c.emitters[i].Play();

        return c;
    }

    public void ReturnWhirlEffect(CombatEffectData c)
    {
        for (int i = 0; i < c.emitters.Length; i++)
            c.emitters[i].Stop();

        c.go.transform.parent = null;
        Whirl.Return(c);
    }

    public CombatEffectData PlayRollEffect(Transform parent)
    {
        CombatEffectData c = Roll.Get();

        c.go.transform.parent = parent;
        c.go.transform.localPosition = new Vector3(0, 0, 0);
        c.go.transform.forward = -parent.forward;

        for (int i = 0; i < c.emitters.Length; i++)
            c.emitters[i].Play();

        return c;
    }

    public void ReturnRolllEffect(CombatEffectData c)
    {
        for (int i = 0; i < c.emitters.Length; i++)
            c.emitters[i].Stop();

        c.go.transform.parent = null;
        Roll.Return(c);
    }


    public IEnumerator PlayAndStop(ParticleSystem emitter, float delay)
    {
        if (emitter == null)
            yield break;

        yield return new WaitForSeconds(delay);
        emitter.Play();

        yield return new WaitForEndOfFrame();
        emitter.Stop();
    }
}