using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 这个类基本用于控制UI中HP的显示，获取其引用后，调用<see cref="SetHealthPoint(int)"/>设置其显示的HP
/// </summary>
/// <remarks>
/// 2018.6.20: NAiveD创建
/// </remarks>

public class HealthPointUIController : MonoBehaviour {

    public GameObject[] healthFlower;
    
    private int maxHealth;
    private int healthPoint;

	// Use this for initialization
	void Start () {
        maxHealth = healthFlower.Length;
        

        SetHealthPoint(2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHealthPoint(int healthPoint)
    {
        if (healthPoint > maxHealth)
        {
            this.healthPoint = maxHealth;
        }
        else if(healthPoint < 0)
        {
            this.healthPoint = 0;
        }
        else
        {
            this.healthPoint = healthPoint;
        }

        for (int i = 0; i < this.healthPoint; i++)
        {
            healthFlower[i].GetComponent<Animator>().SetTrigger("Fill");
        }

        for (int i = this.healthPoint; i < maxHealth; i++)
        {
            healthFlower[i].GetComponent<Animator>().SetTrigger("Unfill");
        }

    }

    public void Heal(int healPoint)
    {
        if (healPoint > maxHealth - healthPoint) healthPoint = maxHealth - healthPoint;

        for (int i = healthPoint; i < healthPoint + healPoint; i++)
        {
            healthFlower[i].GetComponent<Animator>().SetTrigger("Heal");
        }
        healthPoint += healPoint;
    }

    public void Fade(int fadePoint)
    {
        if (fadePoint > healthPoint) fadePoint = healthPoint;

        for (int i = healthPoint - fadePoint; i < healthPoint; i++)
        {
            healthFlower[i].GetComponent<Animator>().SetTrigger("Fade");
        }
        healthPoint -= fadePoint;
    }
}
