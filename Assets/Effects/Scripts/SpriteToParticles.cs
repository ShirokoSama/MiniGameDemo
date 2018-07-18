using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToParticles : MonoBehaviour {
    private ParticleSystem particleSys;  // 粒子系统组件
    private ParticleSystem.Particle[] particleArr;  // 粒子数组  
    private ParticleStatus[] StatusArr; // 记录粒子状态的数组
    private int particleNum;
    public Texture2D texture; // 2D 图像
    public int sampleStep = 1; // 粒子数量
    public int particleNumPerPixel = 1;
    private NormalDistribution normalGenerator; // 高斯分布生成器
    public float Opacity = 1;  // 控制粒子的透明度
    public float scale = 1;
    public float particleScale = 1;
    private float t = 0;
    
    // Use this for initialization
    void Start () {
        particleSys = GetComponent<ParticleSystem>();
        Color[] pix = texture.GetPixels();
        t = particleSys.main.duration;
        var em = particleSys.emission;

        particleNum = ((texture.width-1) / sampleStep + 1) * ((texture.height - 1) / sampleStep + 1);
        particleArr = new ParticleSystem.Particle[particleNum];
        particleSys.Emit(particleNum); 
        particleSys.GetParticles(particleArr);
        int index = 0;
        for (int i=0; i<texture.height; i++)
        {
            for (int j=0; j<texture.width; j++)
            {
                if (i % sampleStep != 0) continue;
                if (j % sampleStep != 0) continue;
                particleArr[index].position = new Vector3((j - (float)texture.width / 2) * scale, 0f, (i - (float)texture.width / 2) * scale);
                Color tmpColor = pix[i * texture.width + j];
                tmpColor.a = tmpColor.a * Opacity;
                particleArr[index].startColor = tmpColor;
                particleArr[index].startSize = particleScale;
                index++;
            }
        }
        particleSys.SetParticles(particleArr, particleArr.Length);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    //t -= 0.02f;
     //   particleSys.Simulate(t, true, true);
     //   Debug.Log(t);
    }
}
