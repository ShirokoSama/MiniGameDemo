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
            JSONClass mapNode = SaveNode(mapObject.GetComponent<MapObject>().type.GetHashCode(), index, GetPureName(mapObject.name),
                mapObject.transform.position.x * 100.0f, mapObject.transform.position.y * 100.0f,
                mapObject.transform.eulerAngles.z, mapObject.transform.localScale.x, mapObject.transform.localScale.y, 
                mapObject.GetComponent<MapObject>().positionEnd.x, mapObject.GetComponent<MapObject>().positionEnd.y,
                mapObject.GetComponent<MapObject>().rotationEnd, mapObject.GetComponent<MapObject>().scaleEnd,
                mapObject.GetComponent<MapObject>().duration, mapObject.GetComponent<MapObject>().visible, 
                new List<Key.KeyTrigger>(), 0, new ShiftCrystal.ShiftCrystalTrigger(0.0f, 0.0f, false));
            mapInfo.Add(mapNode);
            index++;
        }

        FileInfo file = new FileInfo(outputPath + "/Haru.json");
        StreamWriter writer = file.CreateText();
        writer.Write(root.ToString());
        writer.Flush();
        writer.Dispose();
    }

    public static JSONClass SaveNode(int type, int index, string fileName, float positionX, float positionY, float rotation, float scaleX, float scaleY, 
        float positionEndX, float positionEndY, float rotationEnd, float scaleEnd, float duration, bool visible, List<Key.KeyTrigger> keyTriggers, 
        int transferTrigger, ShiftCrystal.ShiftCrystalTrigger shiftTrigger)
    {
        JSONClass result = new JSONClass();
        result.Add("Type", new JSONData(type));
        result.Add("Index", new JSONData(index));
        result.Add("FileName", new JSONData(fileName));
        JSONArray position = new JSONArray();
        position.Add(new JSONData(positionX));
        position.Add(new JSONData(positionY));
        result.Add("Position", position);
        result.Add("Rotation", new JSONData(rotation));
        result.Add("ScaleX", new JSONData(scaleX));
        result.Add("ScaleY", new JSONData(scaleY));
        JSONArray positionEnd = new JSONArray();
        positionEnd.Add(new JSONData(positionEndX));
        positionEnd.Add(new JSONData(positionEndY));
        result.Add("PositionEnd", positionEnd);
        result.Add("RotationEnd", new JSONData(rotationEnd));
        result.Add("ScaleEnd", new JSONData(scaleEnd));
        result.Add("Duration", new JSONData(duration));
        result.Add("Visible", new JSONData(visible));
        JSONArray keyTrigger = new JSONArray();
        result.Add("KeyTrigger", keyTrigger);
        result.Add("TransferCrystalTrigger", new JSONData(transferTrigger));
        JSONClass shiftTriggerNode = new JSONClass();
        JSONArray yRange = new JSONArray();
        yRange.Add(new JSONData(shiftTrigger.yMin));
        yRange.Add(new JSONData(shiftTrigger.yMax));
        shiftTriggerNode.Add("yRange", yRange);
        shiftTriggerNode.Add("Direction", new JSONData(shiftTrigger.direction));
        result.Add("ShiftCrystalTrigger", shiftTriggerNode);
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
            mapPiece.GetComponent<MapObject>().type = (MapPiece.MapType)node["Type"].AsInt;
            mapPiece.transform.position = new Vector3(node["Position"][0].AsFloat / 100.0f, node["Position"][1].AsFloat / 100.0f, 0.0f);
            mapPiece.transform.eulerAngles = new Vector3(0, 0, node["Rotation"].AsFloat);
            mapPiece.transform.localScale = new Vector3(node["ScaleX"].AsFloat, node["ScaleY"].AsFloat, node["Scale"].AsFloat);
            mapPiece.GetComponent<MapObject>().positionEnd.x = node["PositionEnd"][0].AsFloat;
            mapPiece.GetComponent<MapObject>().positionEnd.y = node["PositionEnd"][1].AsFloat;
            mapPiece.GetComponent<MapObject>().rotationEnd = node["RotationEnd"].AsFloat;
            mapPiece.GetComponent<MapObject>().scaleEnd = node["ScaleEnd"].AsFloat;
            mapPiece.GetComponent<MapObject>().duration = node["Duration"].AsFloat;
            mapPiece.GetComponent<MapObject>().visible = node["Visible"].AsBool;
            mapPiece = PrefabUtility.InstantiatePrefab(mapPiece) as GameObject;
            mapPiece.transform.parent = parentObject.transform;
        }
    }
}
