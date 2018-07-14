using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 这个类基本用于控制UI中HP的显示，获取其引用后，调用<see cref="SetCollectionCount(int)"/>设置其显示的HP
/// </summary>
/// <remarks>
/// 2018.6.20: NAiveD创建
/// </remarks>

public class CollectionUIController : MonoBehaviour {

    public GameObject[] collectionFlowers;
    
    private int totalCollectionCount;
    private int currentCollectionCount;

	// Use this for initialization
	void Start () {
        totalCollectionCount = collectionFlowers.Length;
        

        SetCollectionCount(2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCollectionCount(int collectionCount)
    {
        if (collectionCount > totalCollectionCount)
        {
            this.currentCollectionCount = totalCollectionCount;
        }
        else if(collectionCount < 0)
        {
            this.currentCollectionCount = 0;
        }
        else
        {
            this.currentCollectionCount = collectionCount;
        }

        for (int i = 0; i < this.currentCollectionCount; i++)
        {
            collectionFlowers[i].GetComponent<Animator>().SetTrigger("Fill");
        }

        for (int i = this.currentCollectionCount; i < totalCollectionCount; i++)
        {
            collectionFlowers[i].GetComponent<Animator>().SetTrigger("Unfill");
        }

    }

    public void Collect()
    {
        Collect(1);
    }

    public void Collect(int collectCount)
    {
        if (collectCount > totalCollectionCount - currentCollectionCount) currentCollectionCount = totalCollectionCount - currentCollectionCount;

        for (int i = currentCollectionCount; i < currentCollectionCount + collectCount; i++)
        {
            collectionFlowers[i].GetComponent<Animator>().SetTrigger("Collect");
        }
        currentCollectionCount += collectCount;
    }

    public void Drop(int dropCount)
    {
        if (dropCount > currentCollectionCount) dropCount = currentCollectionCount;

        for (int i = currentCollectionCount - dropCount; i < currentCollectionCount; i++)
        {
            collectionFlowers[i].GetComponent<Animator>().SetTrigger("Drop");
        }
        currentCollectionCount -= dropCount;
    }
}
