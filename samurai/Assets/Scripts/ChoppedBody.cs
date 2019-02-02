using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ChoppedBody : MonoBehaviour
{
    public Material         transparentMaterial;
    public Material         diffuseMaterial;
    public AgentType        agentType;
    public ChoppedBodyType  choppedBodyType;

    Transform           _trans;
    Animation           _anims;
    AudioSource         _audio;
    SkinnedMeshRenderer _renderer;    

    void Awake()
    {
        _trans = transform;
        _anims = GetComponent<Animation>();
        _audio = GetComponent<AudioSource>();        
        _anims.wrapMode = WrapMode.ClampForever;

        _renderer = gameObject.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        _anims[_anims.clip.name].wrapMode = WrapMode.ClampForever;
    }

    void Activate(Transform spawnTransform)
    {
        _trans.position = spawnTransform.position;
        _trans.rotation = spawnTransform.rotation;
        _anims.Play();
        _audio.Play();
        _renderer.material = diffuseMaterial;
        StartCoroutine(Fadeout(5));
        //GuiManager.Instance.ShowBloodSplash();
    }

    void Deactivate()
    {

    }

    protected IEnumerator Fadeout(float delay)
    {
        if (transparentMaterial == null)
            yield break;

        yield return new WaitForSeconds(.1f);

        //SpriteEffectsManager.Instance.CreateBloodSlatter(Transform, 2, 3);

        yield return new WaitForSeconds(delay);

        //CombatEffectsManager.Instance.PlayDisappearEffect(Transform.position, Transform.forward);
        _renderer.material = transparentMaterial;

        Color color = new Color(1, 1, 1, 1);
        transparentMaterial.SetColor("_Color", color);

        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 4;
            if (color.a < 0)
                color.a = 0;

            transparentMaterial.SetColor("_Color", color);
            yield return new WaitForEndOfFrame();
        }

        color.a = 0;
        transparentMaterial.SetColor("_Color", color);

        ChoppedBodyFactory.Instance.Collect(gameObject, agentType, choppedBodyType);
    }

}



