using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;

public class GameStartController : MonoBehaviour
{
    private CanvasGroup startHintCanvasGroup;
    private GaussianBlur gaussianBlurEffect;
    private Camera mainUICamera;
    private float targetAlpha;
    private bool startFade;

    public readonly float duration = 1.0f;
    
    // Use this for initialization
    void Start ()
	{
        startHintCanvasGroup = GameObject.Find("StartHint").GetComponent<CanvasGroup>();
        gaussianBlurEffect = this.GetComponent<GaussianBlur>();
        mainUICamera = this.GetComponent<Camera>();
        mainUICamera.cullingMask = 0; // cullingMask = nothing
        targetAlpha = 0.0f;
	    startFade = false;
    }

    public void GameStart()
    {
        startFade = true;
        mainUICamera.cullingMask = 1 << 5; // cullingMask = UI
    }
	
	// Update is called once per frame
	void Update () {
        if (startFade) { 
	        if (this.startHintCanvasGroup.alpha != targetAlpha)
	        {
	            if (Mathf.Abs(targetAlpha - this.startHintCanvasGroup.alpha) <= 0.01f)
	                this.startHintCanvasGroup.alpha = targetAlpha;
                else { 
	                this.startHintCanvasGroup.alpha -= 0.03f;
                    
	            }
            }
            if (gaussianBlurEffect.enabled)
            {
                if (gaussianBlurEffect.BlurSpreadSize != 0.0f)
                {
                    if (Mathf.Abs(gaussianBlurEffect.BlurSpreadSize - 0.0f) <= 0.01f)
                        gaussianBlurEffect.UpdateBlurSpread(0.0f);
                    else
                        gaussianBlurEffect.UpdateBlurSpread(gaussianBlurEffect.BlurSpreadSize - 0.03f);
                }
                else
                {
                    gaussianBlurEffect.enabled = false;
                    startFade = false;
                    this.enabled = false;
                }
            }
        }
    }
}
