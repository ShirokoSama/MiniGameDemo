using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour {

    //目前只是打算作为快速生成json的用的，除了visible、detail、和Makevisible(bool)不负担实际运行时的功能
    

    public MapPiece.MapType type = MapPiece.MapType.FixObject;
    public Vector2 positionEnd = new Vector2(0.0f, 0.0f);
    public float rotationEnd = 0.0f;
    public float scaleEnd = 1.0f;
    public float duration = 1.0f;
    public bool visible = true;
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
