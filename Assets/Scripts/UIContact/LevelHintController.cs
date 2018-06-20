using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个类控制如春之海这样的文字提示的渐入渐出，它必须依附于一个有CanvasGroup组件的实例上，相应的Canvas作为其子对象被控制透明度
/// </summary>
/// <remarks>
/// 2018.6.20: NAiveD创建
/// </remarks>

public class LevelHintController : MonoBehaviour {

    private CanvasGroup canvasGroup;
    public float fadeInSpeed = 2.0f;
    public float fadeOutSpeed = 2.0f;
    public float floatTime = 2.0f;
    public float targetAlpha = 1.0f;
    private bool fadeIn = false;
    private bool fadeOut = false;

	// Use this for initialization
	void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
        fadeIn = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (fadeIn)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, fadeInSpeed * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01)
            {
                canvasGroup.alpha = targetAlpha;
                fadeIn = false;
                StartCoroutine(HangOn());
            }
        }
        if (fadeOut)
        {
            Debug.Log("fadeOut");
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.0f, fadeInSpeed * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha) < 0.01)
            {
                canvasGroup.alpha = 0.0f;
                fadeOut = false;
                enabled = false;
            }
        }
    }

    IEnumerator HangOn()
    {
        yield return new WaitForSeconds(floatTime);
        fadeOut = true;
    }
    
}
