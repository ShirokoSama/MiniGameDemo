using System.Collections;
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

    // Use this for initialization
    void Start () {
        instance = this;
        TextAsset text = Resources.Load<TextAsset>("Haru");
        JSONNode mapData = JSONNode.Parse(text.text);
        mapInfo = mapData["MapInfo"];
        totalSize = mapInfo.Count;

        foreach (JSONNode node in mapInfo.Childs)
        {
            List<Key.KeyTrigger> keyTriggers = new List<Key.KeyTrigger>();
            foreach (JSONNode trigger in node["KeyTrigger"].Childs)
            {
                keyTriggers.Add(new Key.KeyTrigger(trigger["Index"].AsInt));
            }

            if (node["Visible"] == null)
            {
                node.Add("Visible", new JSONData(true));
            }

            mapPieces.Add(new MapPiece((MapPiece.MapType)node["Type"].AsInt, node["Index"].AsInt, node["FileName"], new Vector2(node["Position"][0].AsFloat, node["Position"][1].AsFloat),
                node["Rotation"].AsFloat, new Vector2(node["ScaleX"].AsFloat, node["ScaleY"].AsFloat), node["Visible"].AsBool,
                new Vector2(node["PositionEnd"][0].AsFloat, node["PositionEnd"][1].AsFloat), node["RotationEnd"].AsFloat, node["ScaleEnd"].AsFloat,
                node["Duration"].AsFloat, keyTriggers, node["TransferCrystalTrigger"].AsInt,
                new ShiftCrystal.ShiftCrystalTrigger(node["ShiftCrystalTrigger"]["yRange"][0].AsFloat, node["ShiftCrystalTrigger"]["yRange"][1].AsFloat, node["ShiftCrystalTrigger"]["Direction"].AsBool)));
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach (MapPiece mapPiece in mapPieces)
        {
            mapPiece.RefreshPerFrame();

            //判断是否需要生成以及生成
            float piecePosX = mapPiece.currentPosition.x;
            float piecePosY = mapPiece.currentPosition.y;
            int xCount = Mathf.FloorToInt((cameraTransform.position.x * 100.0f - cameraSize.x / 2 - maxGrassSize.x / 2) / mapSize.x);
            piecePosX = piecePosX + xCount * mapSize.x;
            if (piecePosX < cameraTransform.position.x * 100.0f - cameraSize.x / 2 - maxGrassSize.x / 2) piecePosX += mapSize.x;

            if (NeedGenerated(new Vector2(piecePosX, piecePosY), mapPiece.currentScale, cameraTransform.position))
            {
                if (!generatedPieces.Contains(mapPiece))
                {
                    GameObject grass = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRelativePath + "/" + mapPiece.fileName + ".prefab");
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
                else
                {
                    GameObject grass = mapPiece.gameObject;
                    //同步位置
                    if (Mathf.Abs(grass.transform.position.x * 100.0f - piecePosX) > 1.0f ||
                        Mathf.Abs(grass.transform.position.y * 100.0f - piecePosY) > 1.0f)
                    {
                        grass.transform.position = new Vector3(piecePosX / 100.0f, piecePosY / 100.0f);
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
                    if (grass.GetComponent<MapObject>().visible != mapPiece.visible)
                    {
                        grass.GetComponent<MapObject>().MakeVisible(mapPiece.visible);
                    }
                }
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

    public bool NeedGenerated(Vector2 nodePosition, Vector2 scale, Vector3 cameraPosition)
    {
        float nodeX = nodePosition.x;
        float nodeY = nodePosition.y;
        float radius = Mathf.Sqrt(maxGrassSize.x * maxGrassSize.x * scale.x * scale.x + maxGrassSize.y * maxGrassSize.y * scale.y * scale.y) / 2.0f;
        return (Mathf.Abs(cameraPosition.x * 100.0f - nodeX) < radius + cameraSize.x / 2) && 
            (Mathf.Abs(cameraPosition.y * 100.0f - nodeY) < radius + cameraSize.y / 2);
    }
}
