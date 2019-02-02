using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip bgmClip;          // 背景音乐

    public float fadeOutTime    = 0.4f;
    public float fadeInTime     = 0.4f;

    public AudioSource bgm;

    private void Awake()
    {
        
    }    

    public void FadeIn(AudioClip clip)
    {        
        StopCoroutine("FadeInMusic");
        StopCoroutine("FadeOutMusic");        
        StartCoroutine(FadeInMusic(clip, 1, fadeOutTime, fadeInTime));
    }

    public void FadeOut()
    {        
        StopCoroutine("FadeInMusic");
        StopCoroutine("FadeOutMusic");
        StartCoroutine(FadeOutMusic(fadeOutTime));
    }

    IEnumerator FadeInMusic(AudioClip clip, float musicVolume, float foTime, float fiTime)
    {
        if (bgm == null)
        {
            yield break;
        }

        if (bgm.isPlaying)
        {
            if (foTime == 0)
            {
                bgm.volume = 0;
                bgm.Stop();
            }
            else
            {
                float maxVolume = bgm.volume;
                float volume = bgm.volume;
                while (volume > 0)
                {
                    volume -= 1 / foTime * Time.deltaTime * maxVolume;
                    if (volume < 0)
                        volume = 0;
                    bgm.volume = volume;
                    yield return new WaitForEndOfFrame();
                }
                bgm.Stop();
            }
        }

        yield return new WaitForEndOfFrame();

        if (clip != null)
        {
            bgm.clip = clip;
            bgm.Play();
            if (fiTime == 0)
            {
                bgm.volume = musicVolume;
            }
            else
            {
                float maxVolume = 1;
                float volume = 0;
                while (volume < maxVolume)
                {
                    volume += 1 / fiTime * Time.deltaTime * maxVolume;
                    if (volume > maxVolume)
                        volume = maxVolume;
                    bgm.volume = volume;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    /*
    IEnumerator FadeInMusic(float time)
    {
        float volume = 0;
        StopCoroutine("FadeOutMusic");
        Music.Play();

        if (time == 0)
        {
            Music.volume = MaxMusicVolume;
            yield break;
        }


        //Debug.Log("Fade in music");
        while (volume < MaxMusicVolume)
        {
            volume += 1 / time * Time.deltaTime * MaxMusicVolume;
            if (volume > MaxMusicVolume)
                volume = MaxMusicVolume;

            Music.volume = volume;

            yield return new WaitForEndOfFrame();
        }
    }
    */

    IEnumerator FadeOutMusic(float foTime)
    {
        if (bgm == null || bgm.clip == null)
        {
            yield break;
        }

        if (foTime == 0)
        {
            bgm.volume = 0;
            bgm.Stop();
            yield break;
        }
        
        float volume = bgm.volume;
        while (volume > 0)
        {
            volume -= 1 / foTime * Time.deltaTime;
            if (volume < 0)
                volume = 0;

            bgm.volume = volume;

            yield return new WaitForEndOfFrame();
        }
        bgm.Stop();        
    }
}
