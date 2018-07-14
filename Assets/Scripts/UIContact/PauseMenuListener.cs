using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuListener : MonoBehaviour {

    public float fadeTime = 1.0f;
    [Range(0, 1)]
    public float targetAlpha = 1.0f;
    public float flowerRotateSpeed = 10.0f;
    public Sprite flowerClicked;
    public Sprite flowerUnclicked;
    public Image resumeFlower;
    public Image restartFlower;
    public Sprite musicOn;
    public Sprite musicOff;
    public Image musicImage;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 posStandby;
    private Vector3 posCall;
    public static PauseMenuListener instance;
    private bool isMenuOn;
    private bool flowerRotate = false;
    //音乐启用先这里随便写了，音乐的管理不应该在这里进行。
    private bool isMusicOn;

    private float m_TimeSacleRef = 1.0f;

    public void Awake()
    {
        instance = this;
        isMenuOn = false;
        isMusicOn = true;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        posStandby = rectTransform.localPosition;
        posCall = new Vector3(0, 0, 0);
        if (isMusicOn)
        {
            musicImage.sprite = musicOn;
        }
        else
        {
            musicImage.sprite = musicOff;
        }
    }

    public void OnResumeClicked()
    {
        // resume
        StartCoroutine(FlowerButtonClicked(resumeFlower));
        Time.timeScale = m_TimeSacleRef;
        MenuOff();
    }

    public void OnRestartClicked()
    {
        StartCoroutine(FlowerButtonClicked(restartFlower));
        Time.timeScale = m_TimeSacleRef;
        MenuOff();
    }

    public void OnMusicClicked()
    {
        SwitchMusic();
        if (isMusicOn)
        {
            musicImage.sprite = musicOn;
        }
        else
        {
            musicImage.sprite = musicOff;
        }
    }

    public void MenuOn()
    {
        if (!isMenuOn)
        {
            isMenuOn = true;
            // pause
            m_TimeSacleRef = Time.timeScale;
            Time.timeScale = 0.0f;

            rectTransform.localPosition = posCall;
            StartCoroutine(Fade(0.0f, targetAlpha, fadeTime, canvasGroup));
        }
    }

    public void MenuOff()
    {
        if (isMenuOn)
        {
            isMenuOn = false;
            StartCoroutine(Fade(targetAlpha, 0.0f, fadeTime, canvasGroup));
            StartCoroutine(DisappearMove());
        }
    }

    IEnumerator Fade(float fromAlpha, float targetAlpha, float fadeTime, CanvasGroup canvasGroup)
    {
        float delta = 1.0f / fadeTime;
        float degree = 0.0f;
        while (degree < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, targetAlpha, degree);
            degree += Time.unscaledDeltaTime * delta;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    IEnumerator DisappearMove()
    {
        yield return new WaitForSeconds(fadeTime);
        rectTransform.localPosition = posStandby;
    }


    IEnumerator FlowerButtonClicked(Image flower)
    {
        flowerRotate = true;
        flower.sprite = flowerClicked;
        StartCoroutine(FlowerRotateStop(flower));
        while (flowerRotate)
        {
            flower.rectTransform.Rotate(new Vector3(0.0f, 0.0f, flowerRotateSpeed * Time.deltaTime));
            yield return null;
        }
    }

    IEnumerator FlowerRotateStop(Image flower)
    {
        yield return new WaitForSeconds(1.0f);
        flowerRotate = false;
        flower.sprite = flowerUnclicked;
        flower.rectTransform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void SwitchMusic()
    {
        if (isMusicOn)
        {
            isMusicOn = false;
        }
        else
        {
            isMusicOn = true;
        }
    }
}
