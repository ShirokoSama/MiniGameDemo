using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private int backgroundWidth = 8640;

    // Use this for initialization
    void Start () {
        TextAsset asset = Resources.Load<TextAsset>("BackgroundSplit");
        MemoryStream stream = new MemoryStream(asset.bytes);

        XmlSerializer serializer = new XmlSerializer(typeof(BackgroundPieceCollection));
        collection = (BackgroundPieceCollection)serializer.Deserialize(XmlReader.Create(stream));

        //Debug.Log("Unity :" + Application.dataPath + "!/assets/BackgroundSplit.xml");

        //var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "backgroundsplit-0-0.normal"));
        //if (bundle == null)
        //{
        //    Debug.Log("Fail to Load Bundle");
        //    return;
        //}
        //var bg00 = bundle.LoadAsset<Texture2D>("BackgroundSplit-0-0");
        //Sprite sprite = Sprite.Create(bg00, new Rect(0, 0, bg00.width, bg00.height), new Vector2(0.5f, 0.5f));
        //bgSprite.GetComponent<SpriteRenderer>().sprite = sprite;

        bgObjects = new GameObject[36];
        availableBackground = new Stack<GameObject>();
        for (int i = 0; i < bgObjects.Length; i++)
        {
            bgObjects[i] = new GameObject();
            bgObjects[i].AddComponent<SpriteRenderer>();
            bgObjects[i].GetComponent<SpriteRenderer>().sortingLayerName = "Background";
            availableBackground.Push(bgObjects[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        float startX = cameraTransform.position.x * 100 - cameraSize.x / 2 - 256;
        float endX = cameraTransform.position.x * 100 + cameraSize.x / 2 + 256;
        float startY = cameraTransform.position.y * 100 - cameraSize.y / 2 - 256;
        float endY = cameraTransform.position.y * 100 + cameraSize.y / 2 + 256;
        List<BackgroundPiece> piecesToShow = collection.FindWithin(startX, endX, startY, endY);
        List<BackgroundPiece> piecesToLoad = collection.FindWithin(startX - 256, endX + 256, startY - 256, endY + 256);

        foreach (BackgroundPiece piece in collection.collection)
        {
            if (!piecesToLoad.Contains(piece))
            {
                piece.shown = false;
                piece.loading = false;
            }
        }
        ReuseOutsight();

        foreach (BackgroundPiece piece in piecesToLoad)
        {
            if (piece.shown != true)
            {
                if (piecesToShow.Contains(piece) && !piece.loading)
                {
                    var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/background/backgroundsplit-" + piece.x + "-" + piece.y + ".normal");
                    if (bundle == null)
                    {
                        Debug.Log("Fail to Load Bundle");
                        return;
                    }
                    piece.loading = true;
                    var backgroundTexture = bundle.LoadAsset<Texture2D>(piece.resName);
                    Sprite sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(0.5f, 0.5f));
                    GameObject backgroundObject = availableBackground.Pop();
                    backgroundObject.GetComponent<SpriteRenderer>().sprite = sprite;

                    float initPosX = piece.initialX / 100.0f;
                    int xCount = Mathf.FloorToInt(cameraTransform.position.x * 100 / backgroundWidth);
                    initPosX = initPosX + backgroundWidth / 100.0f * xCount;
                    if (initPosX < cameraTransform.position.x - cameraSize.x / 100.0f) initPosX += backgroundWidth / 100.0f;
                    if (initPosX > cameraTransform.position.x + cameraSize.x / 100.0f) initPosX -= backgroundWidth / 100.0f;
                    backgroundObject.transform.position = new Vector3(initPosX, piece.initialY / 100.0f, 0);
                    bundle.Unload(false);
                    piece.shown = true;
                }
                else
                {
                    if (!piece.loading)
                    {
                        StartCoroutine(LoadBackgroundAsync(piece));
                    }
                }
            }
        }

    }

    void ReuseOutsight()
    {
        for (int i = 0; i < bgObjects.Length; i++)
        {
            if ((bgObjects[i].transform.position.x + 5.12f < cameraTransform.position.x - cameraSize.x / 200.0f 
                || bgObjects[i].transform.position.x - 5.12f > cameraTransform.position.x + cameraSize.x / 200.0f 
                || bgObjects[i].transform.position.y + 5.12f < cameraTransform.position.y - cameraSize.y / 200.0f 
                || bgObjects[i].transform.position.y - 5.12f > cameraTransform.position.y + cameraSize.y / 200.0f)
                && !availableBackground.Contains(bgObjects[i]))
            {
                availableBackground.Push(bgObjects[i]);
            }
        }
    }

    IEnumerator LoadBackgroundAsync(BackgroundPiece piece)
    {
        piece.loading = true;
        var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/background/backgroundsplit-" + piece.x + "-" + piece.y + ".normal");
        if (bundle == null)
        {
            Debug.Log("Fail to Load Bundle");
        }
        var bgTextureRequest = bundle.LoadAssetAsync<Texture2D>(piece.resName);
        yield return bgTextureRequest;
        var bgTexture = bgTextureRequest.asset as Texture2D;
        Sprite sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        GameObject backgroundObject = availableBackground.Pop();
        backgroundObject.GetComponent<SpriteRenderer>().sprite = sprite;

        float initPosX = piece.initialX / 100.0f;
        int xCount = Mathf.FloorToInt(cameraTransform.position.x * 100 / backgroundWidth);
        initPosX = initPosX + backgroundWidth / 100.0f * xCount;
        if (initPosX < cameraTransform.position.x - cameraSize.x / 100.0f) initPosX += backgroundWidth / 100.0f;
        if (initPosX > cameraTransform.position.x + cameraSize.x / 100.0f) initPosX -= backgroundWidth / 100.0f;
        backgroundObject.transform.position = new Vector3(initPosX, piece.initialY / 100.0f, 0);
        bundle.Unload(false);
        piece.shown = true;
    }
}

[XmlRoot("BackgroundPieces")]
public class BackgroundPieceCollection
{
    
    [XmlArray("Collection"), XmlArrayItem("BackgroundPiece")]
    public List<BackgroundPiece> collection = new List<BackgroundPiece>();

    private int backgroundWidth = 8640;

    public List<BackgroundPiece> FindWithin(float startX, float endX, float startY, float endY)
    {
        List<BackgroundPiece> result = new List<BackgroundPiece>();
        foreach (BackgroundPiece piece in collection)
        {
            float X = piece.initialX;
            float Y = piece.initialY;
            int xCount = Mathf.FloorToInt(startX / backgroundWidth);
            X = X + xCount * backgroundWidth;
            if (X < startX) X += backgroundWidth;
            if (X > startX && X < endX && Y > startY && Y < endY)
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
    public bool loading = false;
}

