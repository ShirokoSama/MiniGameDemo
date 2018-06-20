using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
