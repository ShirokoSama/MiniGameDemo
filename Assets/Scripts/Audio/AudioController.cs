using TooSimpleFramework;
using TooSimpleFramework.Components.Managers;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController instance;

    public AudioClip buttonClip;
    public AudioClip failClip;
    public AudioClip getItemClip;
    public AudioClip getKeyItemClip;
    public AudioClip haruClip;
    public AudioClip levelCompleteClip;
    public AudioClip settigClip;
    public AudioClip startClip;

    public float BGMVolume = 0.5f;

    private AudioManager m_audioManager;

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        m_audioManager = AudioManager.Instance;
        m_audioManager.BGMVolume = BGMVolume;
    }

    public void PlayButton()
    {
        m_audioManager.PlaySND(buttonClip);
    }

    public void PlayFail()
    {
        m_audioManager.PlaySND(failClip);
    }

    public void PlayGetItem()
    {
        m_audioManager.PlaySND(getItemClip);
    }

    public void PlayGetKeyItem()
    {
        m_audioManager.PlaySND(getKeyItemClip);
    } 

    public void PlayHaru()
    {
        m_audioManager.PlayBGMWithFade(haruClip, null, null, 5f);
    }

    public void PlayLevelComplete()
    {
        m_audioManager.PlaySND(levelCompleteClip);
    }

    public void PlaySetting()
    {
        m_audioManager.PlaySND(settigClip);
    }

    public void PlayStart()
    {
        m_audioManager.PlaySND(startClip);
    }

    public void MuteMusicOn()
    {
        m_audioManager.BGMVolume = 0;
        m_audioManager.SNDVolume = 0;
    }

    public void MuteMusicOff()
    {
        m_audioManager.BGMVolume = BGMVolume;
        m_audioManager.SNDVolume = 1;
    }

    public void OnDestroy()
    {
        Destroy(GameObject.Find("AudioManager"));
    }

}
