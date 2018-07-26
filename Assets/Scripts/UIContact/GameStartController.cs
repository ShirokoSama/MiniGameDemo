using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;

public class GameStartController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public List<GaussShader> gaussShaders;

    private float TargetAlpha = 1.0f;
    private float duration = 1.0f;
    private bool StartFade = false;
    
    // Use this for initialization
    void Start ()
	{
	    canvasGroup = this.GetComponent<CanvasGroup>();
	    StartFade = false;
    }

    public void gameStart()
    {
        TargetAlpha = 0.0f;
        StartFade = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (StartFade) { 
	        if (this.canvasGroup.alpha != TargetAlpha)
	        {
	            if (Mathf.Abs(TargetAlpha - this.canvasGroup.alpha) <= 0.01f)
	                this.canvasGroup.alpha = TargetAlpha;
                else { 
	                this.canvasGroup.alpha -= 0.03f;
                    
	            }
            }
	        foreach (var shader in gaussShaders)
	        {
	            if (shader.enabled)
	                if (shader.BlurSpreadSize != 0.0f)
	                {
                        Debug.Log(shader.BlurSpreadSize);
	                    if (Mathf.Abs(shader.BlurSpreadSize - 0.0f) <= 0.01f)
	                        shader.UpdateBlurSpread(0.0f);
                        else
	                        shader.UpdateBlurSpread(shader.BlurSpreadSize - 0.03f);

	                    Debug.Log(shader.BlurSpreadSize);
                    }
	                else
	                {
	                    shader.enabled = false;
	                    StartFade = false;
	                    this.enabled = false;
	                }
	        }
        }
    }
}
