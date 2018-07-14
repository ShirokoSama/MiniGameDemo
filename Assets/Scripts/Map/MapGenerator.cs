using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

public class MapGenerator : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
        TextAsset text = Resources.Load<TextAsset>("Haru");
        JSONNode mapData = JSONNode.Parse(text.text);
        mapInfo = mapData["MapInfo"];
        totalSize = mapInfo.Count;
	}
	
	// Update is called once per frame
	void Update () {
        foreach (JSONNode node in mapInfo.Childs)
        {
            float nodePosX = node["Position"][0].AsFloat;
            float nodePosY = node["Position"][1].AsFloat;
            int xCount = Mathf.FloorToInt((cameraTransform.position.x * 100.0f - cameraSize.x / 2 - maxGrassSize.x / 2) / mapSize.x);
            nodePosX = nodePosX + xCount * mapSize.x;
            if (nodePosX < cameraTransform.position.x * 100.0f - cameraSize.x / 2 - maxGrassSize.x / 2) nodePosX += mapSize.x;

            if (Mathf.Abs(nodePosX - cameraTransform.position.x * 100.0f) < cameraSize.x / 2 + maxGrassSize.x * node["Scale"].AsFloat / 2 
                && Mathf.Abs(nodePosY - cameraTransform.position.y * 100.0f) < cameraSize.y / 2 + maxGrassSize.y * node["Scale"].AsFloat / 2)
            {
                if (!generatedNodes.Contains(node))
                {
                    GameObject grass = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRelativePath + "/" + node["FileName"] + ".prefab");
                    grass.transform.position = new Vector3(nodePosX / 100.0f, nodePosY / 100.0f, 0.0f);
                    grass.transform.eulerAngles = new Vector3(0.0f, 0.0f, node["Rotation"].AsFloat);
                    grass.transform.localScale = new Vector3(node["Scale"].AsFloat, node["Scale"].AsFloat, node["Scale"].AsFloat);
                    grass.GetComponent<SpriteRenderer>().sortingOrder = totalSize - node["Index"].AsInt;
                    grass = Instantiate(grass);
                    generatedNodes.Add(node);
                    generatedObjects.Add(grass);
                }
                else
                {
                    //因为我这边一节节点存了之后立即会存一个游戏对象，所以节点和游戏对象应该是相同索引号的，但这样感觉有问题，怎么改好呢？
                    GameObject mapPiece = generatedObjects[generatedNodes.IndexOf(node)];
                    if (Mathf.Abs(mapPiece.transform.position.x * 100.0f - nodePosX) > 10.0f)
                    {
                        mapPiece.transform.position = new Vector3(nodePosX / 100.0f, nodePosY / 100.0f);
                    }
                }
            }
        }
	}
}
