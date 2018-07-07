using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public class BackgroundManager : MonoBehaviour {

    public Transform cameraTransform;
    public Vector2 cameraSize;
    public GameObject bgSprite;
    private GameObject[] bgObjects;
    private Stack<GameObject> availableBackground;
    private BackgroundPieceCollection collection;

    // Use this for initialization
    void Start () {

        XmlSerializer serializer = new XmlSerializer(typeof(BackgroundPieceCollection));
        collection = (BackgroundPieceCollection)serializer.Deserialize(XmlReader.Create(Application.dataPath + @"/Resources" + @"/BackgroundSplit.xml"));

        //var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "backgroundsplit-0-0.normal"));
        //if (bundle == null)
        //{
        //    Debug.Log("Fail to Load Bundle");
        //    return;
        //}
        //var bg00 = bundle.LoadAsset<Texture2D>("BackgroundSplit-0-0");
        //Sprite sprite = Sprite.Create(bg00, new Rect(0, 0, bg00.width, bg00.height), new Vector2(0.5f, 0.5f));
        //bgSprite.GetComponent<SpriteRenderer>().sprite = sprite;

        bgObjects = new GameObject[25];
        availableBackground = new Stack<GameObject>();
        for (int i = 0; i < 25; i++)
        {
            bgObjects[i] = new GameObject();
            bgObjects[i].AddComponent<SpriteRenderer>();
            //bgObjects[i].GetComponent<SpriteRenderer>().sprite = sprite;
            availableBackground.Push(bgObjects[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        List<BackgroundPiece> piecesToLoad = collection.FindWithin(cameraTransform.position.x * 100 - cameraSize.x / 2 - 256, 
            cameraTransform.position.x * 100 + cameraSize.x / 2 + 256,
            cameraTransform.position.y * 100 - cameraSize.y / 2 - 256,
            cameraTransform.position.y * 100 + cameraSize.y / 2 + 256);
        foreach (BackgroundPiece piece in piecesToLoad)
        {
            if (piece.shown != true)
            {
                var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "backgroundsplit-" + piece.x + "-" + piece.y + ".normal"));
                if (bundle == null)
                {
                    Debug.Log("Fail to Load Bundle");
                    return;
                }
                var backgroundTexture = bundle.LoadAsset<Texture2D>(piece.resName);
                Sprite sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(0.5f, 0.5f));
                GameObject backgroundObject = availableBackground.Pop();
                backgroundObject.GetComponent<SpriteRenderer>().sprite = sprite;
                backgroundObject.transform.position = new Vector3(piece.initialX / 100.0f, piece.initialY / 100.0f, 0);
                bundle.Unload(false);
                piece.shown = true;
            }
        }

        foreach (BackgroundPiece piece in collection.collection)
        {
            if (!piecesToLoad.Contains(piece))
            {
                piece.shown = false;
            }
        }
        ReuseOutsight();
    }

    void ReuseOutsight()
    {
        for (int i = 0; i < 25; i++)
        {
            if ((bgObjects[i].transform.position.x + 2.56f < cameraTransform.position.x - cameraSize.x / 200.0f 
                || bgObjects[i].transform.position.x - 2.56 > cameraTransform.position.x + cameraSize.x / 200.0f 
                || bgObjects[i].transform.position.y + 2.56 < cameraTransform.position.y - cameraSize.y / 200.0f 
                || bgObjects[i].transform.position.y - 2.56 > cameraTransform.position.y + cameraSize.y / 200.0f)
                && !availableBackground.Contains(bgObjects[i]))
            {
                availableBackground.Push(bgObjects[i]);
            }
        }
    }
}

[XmlRoot("BackgroundPieces")]
public class BackgroundPieceCollection
{
    [XmlArray("Collection"), XmlArrayItem("BackgroundPiece")]
    public List<BackgroundPiece> collection = new List<BackgroundPiece>();

    public List<BackgroundPiece> FindWithin(float startX, float endX, float startY, float endY)
    {
        List<BackgroundPiece> result = new List<BackgroundPiece>();
        foreach (BackgroundPiece piece in collection)
        {
            if (piece.initialX > startX && piece.initialX < endX && piece.initialY > startY && piece.initialY < endY)
            {
                result.Add(piece);
            }
        }
        return result;
    }

    public BackgroundPiece Get(int x, int y)
    {
        foreach (BackgroundPiece piece in collection)
        {
            if (piece.x == x && piece.y == y)
            {
                return piece;
            }
        }
        return null;
    }
}

[XmlRoot("BackgroundPiece")]
public class BackgroundPiece
{
    [XmlAttribute("x")]
    public int x;
    [XmlAttribute("y")]
    public int y;
    [XmlAttribute("width")]
    public int width;
    [XmlAttribute("height")]
    public int height;
    [XmlAttribute("resName")]
    public string resName;
    [XmlAttribute("initialX")]
    public float initialX;
    [XmlAttribute("initialY")]
    public float initialY;
    public bool shown = false;
}

