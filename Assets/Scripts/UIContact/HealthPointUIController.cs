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
    public Sprite flowerFilled;
    public Sprite flowerUnfilled;

    private int maxHealth;
    private int healthPoint;
    private GameObject[] flowerImage;
    private GameObject[] particle;

	// Use this for initialization
	void Start () {
        maxHealth = healthFlower.Length;
        flowerImage = new GameObject[maxHealth];
        particle = new GameObject[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {
            foreach (Transform child in healthFlower[i].transform)
            {
                if (child.CompareTag("UIFlowerImage"))
                {
                    flowerImage[i] = child.gameObject;
                }
                else
                {
                    particle[i] = child.gameObject;
                }
            }
        }
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
            flowerImage[i].GetComponent<Image>().sprite = flowerFilled;
        }

        for (int i = this.healthPoint; i < maxHealth; i++)
        {
            flowerImage[i].GetComponent<Image>().sprite = flowerUnfilled;
        }

    }
}
