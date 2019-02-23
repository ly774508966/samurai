using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteEffect
{    
    public Material[] materials;        // 任选其一
    public float lifeTime = 0;          // 存在时长（秒），0表示无效
    public float scaleVal = 0;          // scale数值，0表示无效 
    public float scaleTime = 0;         // scale时长（秒），0表示无效    
}

public class SpriteObject
{
    public float passTime = 0;       // 已经过时间（秒）
    public GameObject quad;          // 面片
    public SpriteEffect effect;

    public bool UpdateSprite()
    {
        passTime += Time.deltaTime;
        if (effect.lifeTime > 0 && passTime > effect.lifeTime)
        {
            GameObject.Destroy(quad);
            return false;
        }

        if (effect.scaleVal > 0)
        {
            quad.transform.localScale = Vector3.Lerp(quad.transform.localScale,
                    new Vector3(effect.scaleVal, effect.scaleVal, quad.transform.localScale.z), 
                    passTime / effect.scaleTime);
        }

        return true;
    }
}

public class SpriteEffectMgr : MonoBehaviour
{
    static public SpriteEffectMgr Instance = null;

    List<SpriteObject> _sprites = new List<SpriteObject>();
    List<int> _expireList = new List<int>();

    // 地面血迹
    public SpriteEffect bloodEffect;

    // 地面流动血迹
    public SpriteEffect flowingBloodEffect;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        for (int i = 0; i < _sprites.Count; ++i)
        {
            if (_sprites[i].UpdateSprite() == false)
            {
                _expireList.Add(i);
            }            
        }

        foreach (var idx in _expireList)
        {
            _sprites.Remove(_sprites[idx]);
        }
        _expireList.Clear();
    }

    void CreateSprite(SpriteEffect spriteEffect, Vector3 pos, Vector3 dir)
    {
        SpriteObject spriteObj = new SpriteObject();
        spriteObj.quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        spriteObj.quad.GetComponent<MeshCollider>().enabled = false;
        spriteObj.quad.GetComponent<MeshRenderer>().material = spriteEffect.materials[Random.Range(0, spriteEffect.materials.Length)];
        spriteObj.quad.transform.localPosition = pos;
        spriteObj.quad.transform.localEulerAngles = dir;
        spriteObj.effect = spriteEffect;
        _sprites.Add(spriteObj);
    }

    public void CreateBlood(Vector3 pos, Vector3 dir)
    {
        CreateSprite(bloodEffect, pos, dir);
    }

    public void CreateFlowingBlood(Vector3 pos, Vector3 dir)
    {
        CreateSprite(flowingBloodEffect, pos, dir);
    }
}