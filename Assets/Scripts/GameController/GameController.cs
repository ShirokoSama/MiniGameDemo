using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TooSimpleFramework;

public class GameController : MonoBehaviour {

    public enum State
    {
        TapToStart = 0,
        Play = 1,
        Pause = 2
    }

    public static GameController instance;
    [HideInInspector]
    public State gameState = State.TapToStart;
    public List<GaussShader> gaussShaders;
    public CollectionUIController collectionUIController;
    public PassHintController passHint;
    public Transform kunTransform;
    public Transform cameraTransform;
    public RectTransform hintTransform;
    public Camera mainUICamera;
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
        collectionUIController = (CollectionUIController)FindObjectOfType<CollectionUIController>();
        collectionUIController.Init();
        Framework.Instance.Initialize();
        AudioController audioController = FindObjectOfType<AudioController>();
        audioController.Init();

        MapManager.instance.LoadMapPieceInfo();
        ResolveArchieve();
        mainUICamera.enabled = false;
        AudioController.instance.PlayHaru();
        Time.timeScale = 0.0f;
    }

    private void Update()
    {
        //if (gameState == State.TapToStart)
        //{
        //    foreach (GaussShader shader in gaussShaders)
        //    {
        //        if (targetBlurSize > shader.BlurSpreadSize)
        //        {
        //            shader.BlurSpreadSize += 0.03f;
        //            if (targetBlurSize < shader.BlurSpreadSize)
        //            {
        //                targetBlurSize = 3.0f;
        //            }
        //        }
        //        else
        //        {
        //            shader.BlurSpreadSize -= blurTransformSpeed * Time.deltaTime;
        //            if (targetBlurSize > shader.BlurSpreadSize)
        //            {
        //                targetBlurSize = 10.0f;
        //            }
        //        }
        //    }
        //}
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
        collectionUIController.Collect();
    }

    public void ReachPassHeight()
    {

        if (collectionCount >= totalCollection)
        {
            passHint.Show();
            AudioController.instance.PlayLevelComplete();
        }
    }

    public void RecordArchieve()
    {
        if (!Directory.Exists(archievePath))
        {
            Directory.CreateDirectory(archievePath);
        }
        StreamWriter writer = File.CreateText(archievePath + "/Archieve.json");
        writer.Write(MakeArchieveNode().ToString());
        writer.Flush();
        writer.Close();
    }

    public void ResolveArchieve()
    {
        if (File.Exists(archievePath + "/Archieve.json"))
        {
            StreamReader reader = File.OpenText(archievePath + "/Archieve.json");
            JSONNode archieve = JSON.Parse(reader.ReadToEnd());
            if (archieve["CollectionCount"] == null)
            {
                archieve.Add("CollectionCount", new JSONData(0));
            }
            collectionCount = archieve["CollectionCount"].AsInt;
            collectionUIController.SetCollectionCount(collectionCount);

            kunTransform.position = new Vector2(archieve["KunPosition"][0].AsFloat, archieve["KunPosition"][1].AsFloat);
            cameraTransform.position = new Vector3(archieve["KunPosition"][0].AsFloat, archieve["KunPosition"][1].AsFloat, cameraTransform.position.z);
            MapManager.instance.LoadArchieve(archieve["MapChangeInfo"]);
        }
        else
        {
            collectionUIController.SetCollectionCount(collectionCount);
        }
    }

    public JSONClass MakeArchieveNode()
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

    public void TapStart()
    {
        gameState = State.Play;
        foreach (GaussShader  shader in gaussShaders)
        {
            shader.enabled = false;
        }
        Time.timeScale = 1.0f;
        hintTransform.localPosition = new Vector2(-1280.0f, 0.0f);
        mainUICamera.enabled = true;
        AudioController.instance.PlayStart();
    }

    public void Restart()
    {
        MapManager.instance.ClearCachedBundles();
        if (File.Exists(archievePath + "/Archieve.json"))
        {
            File.Delete(archievePath + "/Archieve.json");
        }
        AudioController.instance.OnDestroy();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
