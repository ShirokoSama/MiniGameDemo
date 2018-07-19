using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public CollectionUIController collectionUIcontroller;
    public PassHintController passHint;
    public Transform kunTransform;
    public Transform cameraTransform;
    public const int totalCollection = 4;

    private string archievePath;
    private int collectionCount = 0;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        archievePath = Application.persistentDataPath + "/Archieve";
        MapManager mapManager = (MapManager)FindObjectOfType<MapManager>();
        mapManager.Init();
        collectionUIcontroller = (CollectionUIController)FindObjectOfType<CollectionUIController>();
        collectionUIcontroller.Init();

        MapManager.instance.LoadMapPieceInfo();
        ResolveArchieve();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            RecordArchieve();
        }
    }

    public void CollectFlower()
    {
        collectionCount++;
        collectionUIcontroller.Collect();
        if (collectionCount >= totalCollection)
        {
            passHint.Show();
        }
    }

    public void RecordArchieve()
    {
        if (!Directory.Exists(archievePath))
        {
            Directory.CreateDirectory(archievePath);
        }
        StreamWriter writer = File.CreateText(archievePath + "/Archieve.json");
        writer.Write(makeArchieveNode().ToString());
        writer.Flush();
        writer.Close();
    }

    public void ResolveArchieve()
    {
        if (File.Exists(archievePath + "/Archieve.json"))
        {
            StreamReader reader = File.OpenText(archievePath + "/Archieve.json");
            JSONNode archieve = JSON.Parse(reader.ReadToEnd());
            collectionCount = archieve["CollectionCount"].AsInt;
            collectionUIcontroller.SetCollectionCount(collectionCount);

            kunTransform.position = new Vector2(archieve["KunPosition"][0].AsFloat, archieve["KunPosition"][1].AsFloat);
            cameraTransform.position = new Vector3(archieve["KunPosition"][0].AsFloat, archieve["KunPosition"][1].AsFloat, cameraTransform.position.z);
            MapManager.instance.LoadArchieve(archieve["MapChangeInfo"]);
        }
    }

    public JSONClass makeArchieveNode()
    {
        JSONClass archieve = new JSONClass();
        archieve.Add("Time", new JSONData(DateTime.Now.ToLocalTime().ToString()));
        archieve.Add("CollectionCount", new JSONData(collectionCount));
        archieve.Add("KunPosition", new JSONArray()
        {
            new JSONData(kunTransform.position.x),
            new JSONData(kunTransform.position.y)
        });
        archieve.Add("MapChangeInfo", MapManager.instance.GenerateArchieveNode());
        return archieve;
    }
}
