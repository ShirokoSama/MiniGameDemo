using System;
using Unity.Jobs;
using UnityEngine;

public class SpriteToParticles : MonoBehaviour {
    private ParticleSystem particleSys;  // 粒子系统组件
    private ParticleSystem.Particle[] particleArr;  // 粒子数组  
    private ParticleStatus[] StatusArr; // 记录粒子状态的数组
    private int particleNum;
    public Texture2D texture; // 2D 图像
    public int sampleStep = 1; // 粒子数量
    public int particleNumPerPixel = 1;
    private readonly NormalDistribution normalGenerator; // 高斯分布生成器
    public float Opacity = 1;  // 控制粒子的透明度
    public float scale = 1;
    public float particleScale = 1;
    private float interval;

    private readonly Vector3 eulerLeft2Right = new Vector3(-180, -90, 90);
    private readonly Vector3 eulerRight2Left = new Vector3(-180, 90, 90);

    // reference of player
    public GameObject Character;
    private Transform m_transform;

    // 这么写是错的
    //struct ParticleEmitJob : IJob
    //{
    //    public ParticleSystem jobParticleSys;
    //    public ParticleSystem.Particle[] jobParticleArr;
    //    public int jobParticleNum;
    //    public void Execute()
    //    {
    //        jobParticleSys.Emit(jobParticleNum);
    //        jobParticleSys.SetParticles(jobParticleArr, jobParticleArr.Length);
    //    }
    //}

    // Use this for initialization
    void Start () {
        m_transform = GetComponent<Transform>();
        m_transform.position = new Vector3(
            Character.transform.position.x - 15f,
            Character.transform.position.y, 0);
        interval = UnityEngine.Random.Range(10f, 20f);

        particleSys = GetComponent<ParticleSystem>();
        Color[] pix = texture.GetPixels();

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
                particleArr[index].position = new Vector3((j - (float)texture.width / 2) * scale * 2, 0f, (i - (float)texture.height / 2) * scale * 2);
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
        interval -= Time.deltaTime;
        if (interval <= 0)
        {
            interval = UnityEngine.Random.Range(10f, 20f);
            if (UnityEngine.Random.value < 0.5f)
            {
                m_transform.position = new Vector3(
                    Character.transform.position.x - 15f,
                    Character.transform.position.y, UnityEngine.Random.Range(-10f, 10f));
                m_transform.eulerAngles = eulerLeft2Right;
            }
            else
            {
                m_transform.position = new Vector3(
                                    Character.transform.position.x + 15f,
                                    Character.transform.position.y, UnityEngine.Random.Range(-10f, 10f));
                m_transform.eulerAngles = eulerRight2Left;
            }
            particleSys.Emit(particleNum);
            particleSys.SetParticles(particleArr, particleArr.Length);
        }
    }

}
