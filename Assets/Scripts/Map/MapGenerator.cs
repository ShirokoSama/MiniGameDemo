using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MapGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TextAsset text = Resources.Load<TextAsset>("Haru");
        JSONNode mapData = JSONNode.Parse(text.text);
        JSONNode mapInfo = mapData["MapInfo"];
        foreach(JSONNode node in mapInfo.Childs)
        {
            Debug.Log(node["Index"].AsInt);
            Debug.Log(node["FileName"]);
            Debug.Log(node["Position"][0].AsInt.ToString() + node["Position"][1].AsInt.ToString());
            Debug.Log(node["Rotation"].AsFloat);
            Debug.Log(node["Scale"].AsFloat);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
