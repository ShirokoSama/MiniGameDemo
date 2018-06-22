using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuListener : MonoBehaviour {

    public float fadeTime = 1.0f;
    [Range(0, 1)]
    public float targetAlpha = 1.0f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 posStandby;
    private Vector3 posCall;
    public static PauseMenuListener instance;

    public void Awake()
    {
        instance = this;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        posStandby = rectTransform.localPosition;
        posCall = new Vector3(0, 0, 0);
    }

    public void OnBackgroundSoundChange()
    {

    }

    public void OnEffectSoundChange()
    {

    }

    public void OnResumeClicked()
    {
        Disappear();
    }

    public void Show()
    {
        rectTransform.localPosition = posCall;

        StartCoroutine(Fade(0.0f, targetAlpha, fadeTime, canvasGroup));
    }

    public void Disappear()
    {
        StartCoroutine(Fade(targetAlpha, 0.0f, fadeTime, canvasGroup));
        StartCoroutine(DisappearMove());
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

}
