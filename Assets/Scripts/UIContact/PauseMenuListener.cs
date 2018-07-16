using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuListener : MonoBehaviour {

    public float fadeTime;
    [Range(0, 1)]
    public float targetAlpha;
    public float flowerRotateSpeed;
    public Sprite flowerClicked;
    public Sprite flowerUnclicked;
    public Image resumeFlower;
    public Image restartFlower;
    public Sprite musicOn;
    public Sprite musicOff;
    public Image musicImage;

    public static PauseMenuListener instance;

    private MonitorGraphic clickMonitorInstance;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 posStandby;
    private Vector3 posCall;
    private bool isMenuOn;
    private bool isFlowerRotate = false;
    //音乐启用先这里随便写了，音乐的管理不应该在这里进行。
    private bool isMusicOn;
    private float m_TimeSacleRef = 1.0f;

    //reference of coroutines
    private Coroutine fadeCoroutine = null;
    private Coroutine flowerButtonCoroutine = null;
    private Coroutine disappearMoveCoroutine = null;

    public void Awake()
    {
        instance = this;
        isMenuOn = false;
        isMusicOn = true;

        clickMonitorInstance = GameObject.FindGameObjectWithTag("ClickMonitor").GetComponent<MonitorGraphic>();
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
        MenuOff(resumeFlower);
    }

    public void OnRestartClicked()
    {
        MenuOff(restartFlower);
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
            // settings
            isMenuOn = true;
            m_TimeSacleRef = Time.timeScale;
            Time.timeScale = 0.0f; // pause
            clickMonitorInstance.raycastTarget = false;
            // animations
            rectTransform.localPosition = posCall;
            if (fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(Fade(0.0f, targetAlpha, fadeTime, canvasGroup));
            }
        }
    }

    public void MenuOff(Image rotateFlower)
    {
        if (isMenuOn 
            && fadeCoroutine == null 
            && disappearMoveCoroutine == null 
            && flowerButtonCoroutine == null)
        {
            // settings
            Time.timeScale = m_TimeSacleRef;
            clickMonitorInstance.raycastTarget = true;
            // animations
            flowerButtonCoroutine = StartCoroutine(FlowerButtonClicked(rotateFlower));
            fadeCoroutine = StartCoroutine(Fade(targetAlpha, 0.0f, fadeTime, canvasGroup));
            disappearMoveCoroutine = StartCoroutine(DisappearMove());
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
        fadeCoroutine = null;
    }

    IEnumerator DisappearMove()
    {
        yield return new WaitForSeconds(fadeTime);
        rectTransform.localPosition = posStandby;
        isMenuOn = false;
        disappearMoveCoroutine = null;
    }


    IEnumerator FlowerButtonClicked(Image flower)
    {
        isFlowerRotate = true;
        flower.sprite = flowerClicked;
        StartCoroutine(FlowerRotateStop(flower));
        while (isFlowerRotate)
        {
            flower.rectTransform.Rotate(new Vector3(0.0f, 0.0f, flowerRotateSpeed * Time.deltaTime));
            yield return null;
        }
    }

    IEnumerator FlowerRotateStop(Image flower)
    {
        yield return new WaitForSeconds(fadeTime);
        isFlowerRotate = false;
        flower.sprite = flowerUnclicked;
        flower.rectTransform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        flowerButtonCoroutine = null;
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
