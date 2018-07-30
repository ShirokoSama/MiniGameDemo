using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour {

    //目前只是打算作为快速生成json的用的，除了visible、detail、和Makevisible(bool)不负担实际运行时的功能
    [System.Serializable]
    public struct KeyTrigger
    {
        public List<int> index;
        public List<MapObject> objects;
        public Vector2 dPosition;
        public float dRotation;
        public Vector2 dScale;
        public float duration;
        public bool visible;
        public bool triggerable;
        public bool load;
        KeyTrigger(List<int> index, Vector2 dPosition, float dRotation, Vector2 dScale, float duration, bool visible = true, bool triggerable = true, bool load = true)
        {
            this.index = index;
            this.objects = new List<MapObject>();
            this.dPosition = dPosition;
            this.dRotation = dRotation;
            this.dScale = dScale;
            this.duration = duration;
            this.visible = visible;
            this.triggerable = triggerable;
            this.load = load;
        }
    }

    [System.Serializable]
    public struct ShiftCrystalTrigger
    {
        public List<int> index;
        public List<MapObject> objects;
        public bool direction;
    }

    public MapPiece.MapType type = MapPiece.MapType.FixObject;
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public int index;
    public List<int> childrenIndex;
    public bool visible = true;
    public bool load = true;
    public List<KeyTrigger> keyTriggers;
    public int transferCrystalTrigger;
    public MapObject transferCrystalTriggerObject;
    public ShiftCrystalTrigger shiftCrystalTrigger;

    public MapPiece detail;
    private Sprite sprite;

    public void MakeVisible(bool visible)
    {
        if (visible)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
        this.visible = visible;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

}
