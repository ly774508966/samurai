using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITween : MonoBehaviour
{
    public static UITween Instance = null;
    
    private void Awake()
    {
        Instance = this;
    }

    public void FadeIn(GameObject go, float duration)
    {
        Graphic[] graphics = go.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.CrossFadeAlpha(1, duration, true);
        }
    }

    public void FadeOut(GameObject go, float fadeOutTime)
    {
        Graphic[] graphics = go.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            graphic.CrossFadeAlpha(1, 0, true);
            graphic.CrossFadeAlpha(0, fadeOutTime, true);
        }        
    }

    public void FadeOut(GameObject go, float delayTime, float fadeOutTime)
    {
        StopCoroutine("FadeOutImpl");
        StartCoroutine(FadeOutImpl(go, delayTime, fadeOutTime));        
    }

    IEnumerator FadeOutImpl(GameObject go, float delayTime, float fadeOutTime)
    {
        FadeIn(go, 0);
        yield return new WaitForSeconds(delayTime);
        FadeOut(go, fadeOutTime);
    }

    public void Bounce(GameObject go, float maxScale, float bounceTime, float fadeOutTime, float delayTime)
    {
        if (maxScale == 0)
        {
            return;
        }
        StopCoroutine("BounceImp");
        StartCoroutine(BounceImp(go, maxScale, bounceTime, fadeOutTime, delayTime));
    }

    IEnumerator BounceImp(GameObject go, float maxScale, float bounceTime, float fadeOutTime, float delayTime)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        if (rt == null)
        {
            yield break;
        }
        FadeIn(go, 0);
        float step = (maxScale - 1) / (0.4f * bounceTime);
        float curScale = 1f;
        while (curScale < maxScale)
        {
            curScale = Mathf.Min(maxScale, curScale + step * Time.deltaTime);
            rt.localScale = new Vector3(curScale, curScale, 1);            
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.2f * bounceTime);
        while (curScale > 1)
        {
            curScale = Mathf.Max(1, curScale - step * Time.deltaTime);
            rt.localScale = new Vector3(curScale, curScale, 1);
            yield return new WaitForEndOfFrame();
        }
        if (fadeOutTime > 0)
        {
            yield return new WaitForSeconds(delayTime);
            FadeOut(go, fadeOutTime);
        }        
    }
};
