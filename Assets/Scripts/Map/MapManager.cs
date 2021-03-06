﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

public class MapManager : MonoBehaviour {

    public static MapManager instance;
    public Transform cameraTransform;
    public Vector2 cameraSize;
    public Vector2 maxGrassSize;
    public Vector2 mapSize = new Vector2(8640.0f, 19200.0f);

    //所有节点存放
    JSONNode mapInfo;
    //所有已生成的节点
    private List<JSONNode> generatedNodes = new List<JSONNode>();
    //所有已生成的游戏对象
    private List<GameObject> generatedObjects = new List<GameObject>();
    //总节点数
    private int totalSize;
    string prefabRelativePath = @"Assets/Prefabs/MapComponents";

    //所有配置文件中记录的地图物件
    private List<MapPiece> mapPieces = new List<MapPiece>();
    //所有以生成的地图物件
    private List<MapPiece> generatedPieces = new List<MapPiece>();
    //AB清单文件
    AssetBundleManifest manifest;
    //缓存部分AB
    Dictionary<string, AssetBundle> cachedBundles = new Dictionary<string, AssetBundle>();

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }

        var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/StreamingAssets");
        manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        bundle.Unload(false);
        bundle = null;
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        foreach (MapPiece mapPiece in mapPieces)
        {
            mapPiece.RefreshPerFrame();

            //判断是否需要生成以及生成
            float piecePosX = mapPiece.currentPosition.x;
            float piecePosY = mapPiece.currentPosition.y;
            piecePosX = expandToRange(piecePosX, mapSize.x, cameraTransform.position.x * 100.0f);

            if (!mapPiece.Loadable)
            {
                if (generatedPieces.Contains(mapPiece))
                {
                    Destroy(mapPiece.gameObject);
                    generatedPieces.Remove(mapPiece);
                }
            }
            if (!generatedPieces.Contains(mapPiece))
            {
                if (NeedGenerated(new Vector2(piecePosX, piecePosY), mapPiece, cameraTransform.position))
                {
                    var bundle = LoadAssetBundle("mapcomponents/" + mapPiece.fileName.ToLower() + ".normal");
                    if (bundle == null)
                    {
                        Debug.Log("Fail To Load Bundle!");
                        return;
                    }
                    GameObject grass = bundle.LoadAsset<GameObject>(mapPiece.fileName + ".prefab");
                    //bundle.Unload(false);
                    //bundle = null;
                    //ClearTempBundles();
                    //GameObject grass = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRelativePath + "/" + mapPiece.fileName + ".prefab");
                    grass.transform.position = new Vector3(piecePosX / 100.0f, piecePosY / 100.0f, 0.0f);
                    grass = Instantiate(grass);
                    grass.transform.eulerAngles = new Vector3(0.0f, 0.0f, -mapPiece.currentRotation);
                    grass.transform.localScale = new Vector3(mapPiece.currentScale.x, mapPiece.currentScale.y, 1.0f);
                    grass.GetComponent<SpriteRenderer>().sortingOrder = totalSize - mapPiece.index;
                    //对于特定类型的游戏对象，生成其对应组件
                    if (mapPiece.type == MapPiece.MapType.Key)
                    {
                        grass.GetComponent<Key>().triggers = mapPiece.triggers;
                    }
                    else if (mapPiece.type == MapPiece.MapType.ShiftCrystal)
                    {
                        grass.GetComponent<ShiftCrystal>().trigger = mapPiece.shiftCrystalTrigger;
                    }
                    else if (mapPiece.type == MapPiece.MapType.TransferCrystal)
                    {
                        grass.GetComponent<TransferCrystal>().targetIndex = mapPiece.transferCrystalTrigger;
                    }
                    grass.GetComponent<MapObject>().detail = mapPiece;
                    generatedPieces.Add(mapPiece);
                    mapPiece.gameObject = grass;
                }
            }
            else
            {
                GameObject grass = mapPiece.gameObject;
                //同步位置
                if (Mathf.Abs(grass.transform.position.x * 100.0f - piecePosX) > 1.0f ||
                    Mathf.Abs(grass.transform.position.y * 100.0f - piecePosY) > 1.0f)
                {
                    grass.transform.position = new Vector3(piecePosX * 0.01f, piecePosY * 0.01f);
                }
                //同步旋转
                if (Mathf.Abs(grass.transform.eulerAngles.z + mapPiece.currentRotation) > 0.1f)
                {
                    grass.transform.eulerAngles = new Vector3(0.0f, 0.0f, -mapPiece.currentRotation);
                }
                //同步缩放
                if (Mathf.Abs(grass.transform.localScale.x - mapPiece.currentScale.x) > 0.001f)
                {
                    grass.transform.localScale = mapPiece.currentScale;
                }
                //同步可见性
                if (grass.GetComponent<MapObject>().visible != mapPiece.Visible)
                {
                    grass.GetComponent<MapObject>().MakeVisible(mapPiece.Visible);
                }
            }
        }
	}

    public void LoadMapPieceInfo()
    {
        TextAsset text = Resources.Load<TextAsset>("Haru");
        JSONNode mapData = JSONNode.Parse(text.text);
        mapInfo = mapData["MapInfo"];
        totalSize = mapInfo.Count;

        foreach (JSONNode node in mapInfo.Childs)
        {
            List<Key.KeyTrigger> keyTriggers = new List<Key.KeyTrigger>();
            foreach (JSONNode trigger in node["KeyTrigger"].Childs)
            {
                List<int> indexList = new List<int>();
                foreach (JSONNode indexNode in trigger["Index"].Childs)
                {
                    indexList.Add(indexNode.AsInt);
                }
                //设置默认值
                if (trigger["dPosition"][0] == null)
                {
                    trigger.Add("dPosition", new JSONArray()
                    {
                        new JSONData(0.0f),
                        new JSONData(0.0f)
                    });
                }
                if (trigger["dRotation"] == null)
                {
                    trigger.Add("dRotation", new JSONData(0.0f));
                }
                if (trigger["dScale"][0] == null)
                {
                    trigger.Add("dScale", new JSONArray
                    {
                        new JSONData(0.0f),
                        new JSONData(0.0f)
                    });
                }
                Key.KeyTrigger keyTrigger = new Key.KeyTrigger(indexList, new Vector2(trigger["dPosition"][0].AsFloat, trigger["dPosition"][1].AsFloat),
                    trigger["dRotation"].AsFloat, new Vector2(trigger["dScale"][0].AsFloat, trigger["dScale"][1].AsFloat),
                    trigger["Duration"].AsFloat, trigger["Visible"].AsBool, trigger["Triggerable"].AsBool, trigger["Load"].AsBool);
                keyTriggers.Add(keyTrigger);
            }

            List<int> children = new List<int>();
            foreach (JSONNode child in node["Children"].Childs)
            {
                children.Add(child.AsInt);
            }

            List<int> shiftIndex = new List<int>();
            foreach (JSONNode child in node["ShiftCrystalTrigger"]["Index"].Childs)
            {
                shiftIndex.Add(child.AsInt);
            }

            if (node["Visible"] == null)
            {
                node.Add("Visible", new JSONData(true));
            }
            if (node["Triggerable"] == null)
            {
                node.Add("Triggerable", new JSONData(true));
            }

            MapPiece mapPiece = new MapPiece((MapPiece.MapType)node["Type"].AsInt, node["Index"].AsInt, node["FileName"], new Vector2(node["Position"][0].AsFloat, node["Position"][1].AsFloat),
                node["Rotation"].AsFloat, new Vector2(node["Scale"][0].AsFloat, node["Scale"][1].AsFloat), node["Visible"].AsBool,
                children, node["Duration"].AsFloat, keyTriggers, node["TransferCrystalTrigger"].AsInt,
                new ShiftCrystal.ShiftCrystalTrigger(shiftIndex, node["ShiftCrystalTrigger"]["Direction"].AsBool), 
                node["Triggerable"].AsBool, node["Load"].AsBool);
            mapPieces.Add(mapPiece);
        }
    }

    public void LoadArchieve(JSONNode mapArchieve)
    {
        JSONNode archievePieces = mapArchieve["ArchievePieces"];
        foreach (MapPiece mapPiece in mapPieces)
        {

            if (archievePieces["Index" + mapPiece.index] != null)
            {
                JSONNode archievePiece = archievePieces["Index" + mapPiece.index];
                mapPiece.LoadArchive(new Vector2(archievePiece["CurrentPosition"][0].AsFloat, archievePiece["CurrentPosition"][1].AsFloat),
                    new Vector2(archievePiece["TargetPositionOffset"][0].AsFloat, archievePiece["TargetPositionOffset"][1].AsFloat),
                    archievePiece["MoveCountDown"].AsFloat, archievePiece["CurrentRotation"].AsFloat, archievePiece["TartgetRotationOffset"].AsFloat,
                    archievePiece["RotationCountDown"].AsFloat, new Vector2(archievePiece["CurrentScale"][0].AsFloat, archievePiece["CurrentScale"][1].AsFloat),
                    new Vector2(archievePiece["TargetScaleOffset"][0].AsFloat, archievePiece["TargetScaleOffset"][1].AsFloat), 
                    archievePiece["ScaleCountDown"].AsFloat, archievePiece["Visible"].AsBool,
                    archievePiece["Triggerable"].AsBool,archievePiece["TriggerableCountDown"].AsFloat, archievePiece["Loadable"].AsBool);
            }
        }
    }

    public MapPiece Get(int index)
    {
        foreach (MapPiece mapPiece in mapPieces)
        {
            if (mapPiece.index == index)
            {
                return mapPiece;
            }
        }
        return null;
    }

    public List<MapPiece> GetBetween(float yMin, float yMax)
    {
        List<MapPiece> result = new List<MapPiece>();
        foreach (MapPiece mapPiece in mapPieces)
        {
            if (mapPiece.currentPosition.y >= yMin && mapPiece.currentPosition.y <= yMax)
            {
                result.Add(mapPiece);
            }
        }
        return result;
    }

    public JSONClass GenerateArchieveNode()
    {
        JSONClass archievePieces = new JSONClass();
        foreach (MapPiece mapPiece in mapPieces)
        {
            if (mapPiece.changed)
            {
                archievePieces.Add("Index" + mapPiece.index.ToString(), mapPiece.GenerateArchivePiece());
            }
        }
        JSONClass mapArchieve = new JSONClass();
        mapArchieve.Add("ArchievePieces", archievePieces);
        return mapArchieve;
    }

    public bool NeedGenerated(Vector2 nodePosition, MapPiece mapPiece, Vector3 cameraPosition)
    {
        float nodeX = nodePosition.x;
        float nodeY = nodePosition.y;
        Vector2 maxSize = new Vector2();
        if (mapPiece.fileName.Contains("Obstacle"))
        {
            maxSize = maxGrassSize;
        }
        else
        {
            maxSize = new Vector2(900.0f, 700.0f);
        }
        float radius = Mathf.Sqrt(maxSize.x * maxSize.x * mapPiece.currentScale.x * mapPiece.currentScale.x + maxSize.y * maxSize.y * mapPiece.currentScale.y * mapPiece.currentScale.y) / 2.0f;
        return (Mathf.Abs(cameraPosition.x * 100.0f - nodeX) < radius + cameraSize.x / 2) && 
            (Mathf.Abs(cameraPosition.y * 100.0f - nodeY) < radius + cameraSize.y / 2) && mapPiece.Loadable;
    }

    public AssetBundle LoadAssetBundle(string bundlePath)
    {
        if (manifest == null)
        {
            return null;
        }

        var file = new List<string>();
        GetDependecies(bundlePath, file);

        for (int i = 0; i < file.Count; i++)
        {
            if (!cachedBundles.ContainsKey(file[i]))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + file[i]);
                cachedBundles.Add(file[i], bundle);
            }
        }
        if (!cachedBundles.ContainsKey(bundlePath))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + bundlePath);
            cachedBundles.Add(bundlePath, bundle);
            return bundle;
        }
        return cachedBundles[bundlePath];
    }

    public void ClearCachedBundles()
    {
        foreach (AssetBundle bundle in cachedBundles.Values)
        {
            bundle.Unload(false);
        }
        cachedBundles.Clear();
    }

    public void GetDependecies(string bundleName, List<string> dependenciesList)
    {
        if (manifest == null)
        {
            return;
        }

        var dep = manifest.GetAllDependencies(bundleName);
        if (dep.Length == 0)
        {
            return;
        }
        
        for (int i = 0, count = dep.Length; i < count; i++)
        {
            var item = dep[i];
            this.GetDependecies(item, dependenciesList);
            dependenciesList.Add(item);
        }
    }

    public float expandToRange(float originalValue, float width, float referenceValue)
    {
        int count = Mathf.FloorToInt(referenceValue / width);
        float expandValue = originalValue + width * count;
        if (referenceValue - expandValue > width * 0.5f)
        {
            return expandValue + width;
        }
        else if (expandValue - referenceValue > width * 0.5f)
        {
            return expandValue - width;
        }
        else
        {
            return expandValue;
        }
    }
}
