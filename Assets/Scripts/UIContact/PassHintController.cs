using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassHintController : MonoBehaviour {

    public float fadeInTime = 1.0f;
    [Range(0.0f, 1.0f)]
    public float targetAlpha = 1.0f;
    public Vector2 standByPosition;
    public Vector2 showPosition;
    public Animator flowerRotation;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform.localPosition = standByPosition;
        canvasGroup.alpha = 0.0f;
	}

    public void Show()
    {
        if (fadeCoroutine == null)
        {
            rectTransform.localPosition = showPosition;
            flowerRotation.SetTrigger("Rotate");
            fadeCoroutine = StartCoroutine(Fade(0.0f, targetAlpha, fadeInTime, canvasGroup));
        }
    }

    public void FadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        rectTransform.localPosition = standByPosition;
        flowerRotation.SetTrigger("UnRotate");
        fadeCoroutine = StartCoroutine(Fade(targetAlpha, 0.0f, fadeInTime, canvasGroup));
    }

    IEnumerator Fade(float startAlpha, float targetAlpha, float fadeTime, CanvasGroup group)
    {
        float timeLeft = fadeTime;
        while (targetAlpha - group.alpha > 0.01)
        {
            group.alpha = Mathf.Lerp(group.alpha, targetAlpha, Time.deltaTime / timeLeft);
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        group.alpha = targetAlpha;
        fadeCoroutine = null;
    }

    //IEnumerator FlowerRotate(Image flower)
    //{
    //    flowerRotate = true;
    //    while (flowerRotate)
    //    {
    //        flower.rectTransform.Rotate(new Vector3(0.0f, 0.0f, Time.deltaTime * rotateSpeed));
    //        yield return null;
    //    }
    //}
}
