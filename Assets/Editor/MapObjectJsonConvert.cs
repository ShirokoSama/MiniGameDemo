using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using SimpleJSON;

/// <summary>
/// 用于将选定的游戏对象存至MapTest.json
/// 或将MapTest.json保存的游戏对象（须在Prefabs/MapComponents中有预设体)直接到处至场景，作为选中游戏对象的子物体
/// </summary>
public class MapObjectJsonConvert : MonoBehaviour {
    
	[MenuItem("Other/Map/Save Slection To JSON")]
    public static void SaveSelectionToJSON()
    {
        string outputPath = Application.dataPath + @"/Resources";
        JSONClass root = new JSONClass();
        JSONArray mapInfo = new JSONArray();
        root.Add("MapInfo", mapInfo);

        GameObject[] objects = Selection.gameObjects;
        int index = 0;
        foreach (GameObject mapObject in objects)
        {
            JSONClass mapNode = SaveNode(index, GetPureName(mapObject.name), 
                mapObject.transform.position.x, mapObject.transform.position.y, 
                mapObject.transform.eulerAngles.z, mapObject.transform.localScale.x);
            mapInfo.Add(mapNode);
        }

        FileInfo file = new FileInfo(outputPath + "/Haru.json");
        StreamWriter writer = file.CreateText();
        writer.Write(root.ToString());
        writer.Flush();
        writer.Dispose();
    }

    public static JSONClass SaveNode(int index, string fileName, float positionX, float positionY, float rotation, float scale)
    {
        JSONClass result = new JSONClass();
        result.Add("Index", new JSONData(index));
        result.Add("FileName", new JSONData(fileName));
        JSONArray position = new JSONArray();
        position.Add(new JSONData(positionX));
        position.Add(new JSONData(positionY));
        result.Add("Position", position);
        result.Add("Rotation", new JSONData(rotation));
        result.Add("Scale", new JSONData(scale));
        return result;
    }

    public static string GetPureName(string source)
    {
        //string pattern = @"(.+)\s*(\(\d+\))?";

        string pattern = @"(\S+)\s*(\(\d+\))?";

        return Regex.Match(source, pattern).Groups[1].Value;
    }

    [MenuItem("Other/Map/Load Prefabs From JSON")]
    public static void LoadPrefabsFromJSON()
    {
        GameObject parentObject = Selection.gameObjects[0];
        string prefabRelativePath = @"Assets/Prefabs/MapComponents";
        TextAsset text = Resources.Load<TextAsset>("Haru");
        JSONNode mapTest = JSONNode.Parse(text.text);
        JSONNode mapInfo = mapTest["MapInfo"];
        foreach (JSONNode node in mapInfo.Childs)
        {
            GameObject mapPiece = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRelativePath + "/" + node["FileName"] + ".prefab");
            mapPiece.transform.position = new Vector3(node["Position"][0].AsFloat, node["Position"][1].AsFloat, 0.0f);
            mapPiece.transform.eulerAngles = new Vector3(0, 0, node["Rotation"].AsFloat);
            mapPiece.transform.localScale = new Vector3(node["Scale"].AsFloat, node["Scale"].AsFloat, node["Scale"].AsFloat);
            mapPiece = PrefabUtility.InstantiatePrefab(mapPiece) as GameObject;
            mapPiece.transform.parent = parentObject.transform;
            Debug.Log(prefabRelativePath + "/" + node["FileName"] + ".prefab");
        }
    }
}
