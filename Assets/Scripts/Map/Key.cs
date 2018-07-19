using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public struct KeyTrigger
    {
        public int index;

        public KeyTrigger(int index)
        {
            this.index = index;
        }
    }

    public List<KeyTrigger> triggers;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && gameObject.GetComponent<MapObject>().detail.Triggerable)
        {
            foreach (KeyTrigger trigger in triggers)
            {
                MapPiece piece = MapManager.instance.Get(trigger.index);
                if (piece != null)
                {
                    Vector2 positionOffset = piece.endPosition - piece.originalPosition;
                    piece.SetNewMoveOffset(positionOffset, piece.duration);
                    float rotationOffset = piece.endRotation - piece.originalRotation;
                    piece.SetNewRotationOffset(rotationOffset, piece.duration);
                    float scaleRatio = piece.endScale / piece.originalScale.x;
                    piece.SetNewScaleRatio(scaleRatio, piece.duration);
                }
            }

            GetComponent<MapObject>().detail.Visible = false;
            GetComponent<MapObject>().detail.Triggerable = false;
        }
    }
}
