using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferCrystal : MonoBehaviour {

    public int targetIndex;
    private MapObject mapObject;

    private void Start()
    {
        mapObject = GetComponent<MapObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && mapObject.detail.Triggerable)
        {
            MapPiece piece = MapManager.instance.Get(targetIndex);
            if (piece.Triggerable == true)
            {
                piece.TriggerBreak(5.0f);
            }
            mapObject.detail.TriggerBreak(5.0f);

            other.transform.position = piece.currentPosition / 100.0f;
            GameController.instance.cameraTransform.position = new Vector3(piece.currentPosition.x, piece.currentPosition.y, GameController.instance.cameraTransform.position.z);
        }
    }
}
