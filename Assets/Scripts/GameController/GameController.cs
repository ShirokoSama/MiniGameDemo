using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public CollectionUIController collectionUIcontroller;
    public PassHintController passHint;
    public const int totalCollection = 4;

    private int collectionCount = 0;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }
    

    void CollectFlower()
    {
        collectionCount++;
        collectionUIcontroller.Collect();
        if (collectionCount >= totalCollection)
        {
            passHint.Show();
        }
    }
}
