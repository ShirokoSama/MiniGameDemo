using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using SimpleJSON;
using UnityEditor.Experimental.UIElements.GraphView;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

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
        Debug.Log(Selection.activeInstanceID);
        foreach (GameObject mapObject in objects)
        {
            if (mapObject.GetComponent<MapObject>().index != 0) {
                index++;
            }
        }

        for (int idx = 1; idx <= index; idx++)
        {
            foreach (GameObject mapObject in objects)
            {
                if (mapObject.GetComponent<MapObject>().index == idx)
                {
                    JSONClass mapNode = SaveNode(mapObject.GetComponent<MapObject>().type.GetHashCode(),
                        mapObject.GetComponent<MapObject>().index, GetFileName(mapObject),
                        mapObject.transform.position.x * 100.0f, mapObject.transform.position.y * 100.0f,
                        -mapObject.transform.eulerAngles.z, mapObject.transform.localScale.x,
                        mapObject.transform.localScale.y,
                        mapObject.GetComponent<MapObject>().childrenIndex, mapObject.GetComponent<MapObject>().visible,
                        mapObject.GetComponent<MapObject>().load,
                        mapObject.GetComponent<MapObject>().keyTriggers,
                        mapObject.GetComponent<MapObject>().transferCrystalTrigger,
                        mapObject.GetComponent<MapObject>().shiftCrystalTrigger);
                    mapInfo.Add(mapNode);
                }
            }
        }

        index++;
        foreach (GameObject mapObject in objects)
        {
            if (mapObject.GetComponent<MapObject>().index == 0)
            {
                JSONClass mapNode = SaveNode(mapObject.GetComponent<MapObject>().type.GetHashCode(), index, GetFileName(mapObject),
                    mapObject.transform.position.x * 100.0f, mapObject.transform.position.y * 100.0f,
                    -mapObject.transform.eulerAngles.z, mapObject.transform.localScale.x, mapObject.transform.localScale.y,
                    mapObject.GetComponent<MapObject>().childrenIndex, mapObject.GetComponent<MapObject>().visible, mapObject.GetComponent<MapObject>().load,
                    mapObject.GetComponent<MapObject>().keyTriggers, mapObject.GetComponent<MapObject>().transferCrystalTrigger,
                    mapObject.GetComponent<MapObject>().shiftCrystalTrigger);
                mapInfo.Add(mapNode);
                index++;
            }
        }

        FileInfo file = new FileInfo(outputPath + "/HaruGeneratedObjects.json");
        StreamWriter writer = file.CreateText();
        writer.Write(root.ToString());
        writer.Flush();
        writer.Dispose();
    }

    private static string GetFileName(GameObject obj)
    {
        Debug.Log(obj.name.Contains("Obstacle"));
        if (obj.name.Contains("Flower_Particles"))
            return obj.GetComponent<SpriteRenderer>().sprite.name;
        if (obj.name.Contains("Obstacle"))
            return obj.GetComponent<SpriteRenderer>().sprite.name;
        if (obj.name.Contains("Flowers"))
            return obj.GetComponent<SpriteRenderer>().sprite.name.Substring(0, obj.GetComponent<SpriteRenderer>().sprite.name.Length - 6);
        if (obj.name.Contains("Key"))
            return "Platform_Item_Key";
        return "ERROR";
    }

    [MenuItem("Other/Map/Get Selection Indexes")]
    public static void GetIndexes()
    {
        string outputPath = Application.dataPath + @"/Resources";
        GameObject[] objects = Selection.gameObjects;
        string s = "";
        foreach (GameObject mapObject in objects)
        {
            int index = mapObject.GetComponent<MapObject>().index;
            s += index.ToString() + ",";
        }
        Debug.Log(s);
    }

    public static JSONClass SaveNode(int type, int index, string fileName, float positionX, float positionY, float rotation, float scaleX, float scaleY, 
        List<int> children, bool visible, bool load, List<MapObject.KeyTrigger> keyTriggers, 
        int transferTrigger, MapObject.ShiftCrystalTrigger shiftTrigger)
    {
        JSONClass result = new JSONClass();
        result.Add("Type", new JSONData(type));
        result.Add("Index", new JSONData(index));
        result.Add("FileName", new JSONData(fileName));
        result.Add("Position", new JSONArray
        {
            new JSONData(positionX),
            new JSONData(positionY)
        });
        result.Add("Rotation", new JSONData(rotation));
        result.Add("Scale", new JSONArray
        {
            new JSONData(scaleX),
            new JSONData(scaleY)
        });
        JSONArray childrenJSON = new JSONArray();
        foreach (int child in children)
        {
            childrenJSON.Add(new JSONData(child));
        }
        result.Add("Children", childrenJSON);
        result.Add("Visible", new JSONData(visible));
        result.Add("Load", new JSONData(load));
        JSONArray keyTrigger = new JSONArray();
        foreach (MapObject.KeyTrigger trigger in keyTriggers)
        {
            JSONClass triggerClass = new JSONClass();
            JSONArray indexArray = new JSONArray();
            Debug.Log(trigger.dPosition.x);
            foreach (int ind in trigger.index)
            {
                indexArray.Add(new JSONData(ind));
            }

            triggerClass.Add("Index", indexArray);
            triggerClass.Add("dPosition", new JSONArray()
            {
                new JSONData(trigger.dPosition.x * 100.0f),
                new JSONData(trigger.dPosition.y * 100.0f)
            });
            triggerClass.Add("dRotation", new JSONData(trigger.dRotation));
            triggerClass.Add("dScale", new JSONArray()
            {
                new JSONData(trigger.dScale.x),
                new JSONData(trigger.dScale.y)
            });
            triggerClass.Add("Duration", new JSONData(trigger.duration)); ;
            triggerClass.Add("Visible", new JSONData(trigger.visible));
            triggerClass.Add("Triggerable", new JSONData(trigger.triggerable));
            triggerClass.Add("Load", new JSONData(trigger.load));
            keyTrigger.Add(triggerClass);
        }
        result.Add("KeyTrigger", keyTrigger);
        result.Add("TransferCrystalTrigger", new JSONData(transferTrigger));
        JSONClass shiftTriggerNode = new JSONClass();
        JSONArray shiftIndex = new JSONArray();
        foreach (int ind in shiftTrigger.index)
        {
            shiftIndex.Add(new JSONData(ind));
        }
        shiftTriggerNode.Add("Index", shiftIndex);
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

    [MenuItem("Other/Map/Load Map")]
    public static void LoadMapFromXML()
    {
        TextAsset asset = Resources.Load<TextAsset>("BackgroundSplit");
        MemoryStream stream = new MemoryStream(asset.bytes);

        XmlSerializer serializer = new XmlSerializer(typeof(BackgroundPieceCollection));
        BackgroundPieceCollection collection = (BackgroundPieceCollection)serializer.Deserialize(XmlReader.Create(stream));

        List<BackgroundPiece> piecesToLoad = collection.FindWithin(0, 8640, 0, 19200);
        foreach (BackgroundPiece piece in piecesToLoad)
        {
            if (piece.shown != true)
            {
                var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/background/backgroundsplit-" + piece.x + "-" + piece.y + ".normal");
                if (bundle == null)
                {
                    Debug.Log("Fail to Load Bundle");
                    return;
                }
                var backgroundTexture = bundle.LoadAsset<Texture2D>(piece.resName);
                Sprite sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(0.5f, 0.5f));
                GameObject obj = new GameObject();
                SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                int posx = 0;
                int posy = 0;
                for (int i = 0; i <= 37 - piece.y; i++)
                {
                    if (i == 0) posy += 128;
                    if (i == 1) posy += 128 + 256;
                    if (i > 1) posy += 512;
                }
                for (int i = 0; i <= piece.x; i++)
                {
                    if (i == 16) posx += 224 + 256;
                    else posx += 512;
                }
                obj.transform.position = new Vector3(posx/100.0f , posy / 100.0f, 0f);
                Debug.Log(obj.transform.position);
                obj = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                bundle.Unload(false);
            }
        }
    }
    [MenuItem("Other/Map/Unload Map")]
    public static void ReleaseFiles()
    {
        TextAsset asset = Resources.Load<TextAsset>("BackgroundSplit");
        MemoryStream stream = new MemoryStream(asset.bytes);

        XmlSerializer serializer = new XmlSerializer(typeof(BackgroundPieceCollection));
        BackgroundPieceCollection collection = (BackgroundPieceCollection)serializer.Deserialize(XmlReader.Create(stream));

        List<BackgroundPiece> piecesToLoad = collection.FindWithin(0, 8640, 0, 19200);
        foreach (BackgroundPiece piece in piecesToLoad)
        {
            var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/background/backgroundsplit-" + piece.x + "-" + piece.y + ".normal");
            bundle.Unload(true);
        }
    }

    [MenuItem("Other/Map/Load Prefabs From JSON")]
    public static void LoadPrefabsFromJSON()
    {
        // GameObject parentObject = Selection.gameObjects[0];
        string prefabRelativePath = @"Assets/Prefabs/MapComponents";
        TextAsset text = Resources.Load<TextAsset>("Haru");
        JSONNode mapTest = JSONNode.Parse(text.text);
        JSONNode mapInfo = mapTest["MapInfo"];
        foreach (JSONNode node in mapInfo.Childs)
        {
            GameObject mapPiece = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRelativePath + "/" + node["FileName"] + ".prefab");
            mapPiece = Instantiate(mapPiece);
            mapPiece.GetComponent<MapObject>().index = node["Index"].AsInt;
            mapPiece.GetComponent<MapObject>().position = new Vector2(node["Position"][0].AsFloat, node["Position"][1].AsFloat);
            mapPiece.GetComponent<MapObject>().rotation = node["Rotation"].AsFloat;
            mapPiece.GetComponent<MapObject>().scale = new Vector2(node["Scale"][0].AsFloat, node["Scale"][1].AsFloat);

            List<int> childrens = new List<int>();
            foreach (JSONNode child in node["Children"].AsArray)
                childrens.Add(child.AsInt);
            mapPiece.GetComponent<MapObject>().childrenIndex = childrens;

            List<MapObject.KeyTrigger> keyTriggers = new List<MapObject.KeyTrigger>();

            List<int> indexList = new List<int>();
            foreach (JSONNode trigger in node["KeyTrigger"].Childs)
            {
                MapObject.KeyTrigger currentKey = new MapObject.KeyTrigger();
                indexList = new List<int>();
                foreach (JSONNode index in trigger["Index"].AsArray)
                {
                    indexList.Add(index.AsInt);
                }
                currentKey.index = indexList;
                currentKey.dPosition = new Vector2(trigger["dPosition"][0].AsFloat / 100.0f,
                    trigger["dPosition"][1].AsFloat / 100.0f);
                currentKey.dRotation = trigger["dRotation"].AsFloat;
                currentKey.dScale = new Vector2(trigger["dScale"].AsFloat, trigger["dScale"].AsFloat);
                currentKey.duration = trigger["Duration"].AsFloat;
                currentKey.visible = trigger["Visible"].AsBool;
                currentKey.triggerable = trigger["Triggerable"].AsBool;
                currentKey.load = trigger["Load"].AsBool;
                keyTriggers.Add(currentKey);
            }

            mapPiece.GetComponent<MapObject>().keyTriggers = keyTriggers;
            mapPiece.GetComponent<MapObject>().transferCrystalTrigger = node["TransferCrystalTrigger"].AsInt;

            MapObject.ShiftCrystalTrigger shiftTrigger = new MapObject.ShiftCrystalTrigger();
            indexList = new List<int>();
            foreach (JSONNode index in node["ShiftCrystalTrigger"]["Index"].AsArray)
            {
                indexList.Add(index.AsInt);
            }

            shiftTrigger.index = indexList;
            shiftTrigger.direction = node["ShiftCrystalTrigger"]["Direction"].AsBool;
            mapPiece.GetComponent<MapObject>().shiftCrystalTrigger = shiftTrigger;

            mapPiece.GetComponent<MapObject>().type = (MapPiece.MapType)node["Type"].AsInt;
            mapPiece.transform.position = new Vector3(node["Position"][0].AsFloat / 100.0f, node["Position"][1].AsFloat / 100.0f, 0.0f);
            mapPiece.transform.eulerAngles = new Vector3(0, 0, -node["Rotation"].AsFloat);
            mapPiece.transform.localScale = new Vector3(node["Scale"][0].AsFloat, node["Scale"][1].AsFloat, 1.0f);
            mapPiece.GetComponent<MapObject>().visible = node["Visible"].AsBool;
            mapPiece.GetComponent<MapObject>().load = node["Load"].AsBool;
            mapPiece = PrefabUtility.InstantiatePrefab(mapPiece) as GameObject;
            // mapPiece.transform.parent = parentObject.transform;
        }
    }

}
