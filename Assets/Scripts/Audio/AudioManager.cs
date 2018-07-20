using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public AudioClip buttonClip;
    public AudioClip failClip;
    public AudioClip getItemClip;
    public AudioClip getKeyItemClip;
    public AudioClip haruClip;
    public AudioClip levelCompleteClip;
    public AudioClip settigClip;
    public AudioClip startClip;

    private List<AudioSource> audioSources;

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSources = new List<AudioSource>();
    }

    public void PlayButton()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = buttonClip;
        source.Play();
        source.loop = false;
    }

    public void PlayFail()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = failClip;
        source.Play();
        source.loop = false;
    }

    public void PlayGetItem()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = getItemClip;
        source.Play();
        source.loop = false;
    }

    public void PlayGetKeyItem()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = getKeyItemClip;
        source.Play();
        source.loop = false;
    } 

    public void PlayHaru()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = haruClip;
        source.Play();
        source.loop = true;
    }

    public void PlayLevelComplete()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = levelCompleteClip;
        source.Play();
        source.loop = false;
    }

    public void PlaySetting()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = settigClip;
        source.Play();
        source.loop = false;
    }

    public void PlayStart()
    {
        AudioSource source = GetEmptyAudioSource();
        source.clip = startClip;
        source.Play();
        source.loop = false;
    }

    AudioSource GetEmptyAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        GameObject gameOb = new GameObject();
        AudioSource audioSource = gameOb.AddComponent<AudioSource>();
        audioSources.Add(audioSource);
        return audioSource;
    }

}
