using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    [System.Serializable]
    public struct KeyTrigger
    {
        public List<int> index;
        public Vector2 dPosition;
        public float dRotation;
        public Vector2 dScale;
        public float duration;
        public bool visible;
        public bool triggerable;
        public bool load;
        public KeyTrigger(List<int> index, Vector2 dPosition, float dRotation, Vector2 dScale, float duration, bool visible = true, bool triggerable = true, bool load = true)
        {
            this.index = index;
            this.dPosition = dPosition;
            this.dRotation = dRotation;
            this.dScale = dScale;
            this.duration = duration;
            this.visible = visible;
            this.triggerable = triggerable;
            this.load = load;
        }
    }

    public List<KeyTrigger> triggers;
    private MapObject mapObject;

    private void Start()
    {
        mapObject = GetComponent<MapObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && mapObject.detail.Triggerable)
        {
            foreach (KeyTrigger trigger in triggers)
            {
                foreach (int index in trigger.index)
                {
                    MapPiece piece = MapManager.instance.Get(index);
                    piece.SetNewMoveOffset(trigger.dPosition, trigger.duration);
                    piece.SetNewRotationOffset(trigger.dRotation, trigger.duration);
                    piece.SetNewScaleRatio(trigger.dScale, trigger.duration);
                    piece.Visible = trigger.visible;
                    piece.Triggerable = trigger.triggerable;
                    piece.Loadable = trigger.load;
                }
            }

            mapObject.detail.Visible = false;
            mapObject.detail.Triggerable = false;
        }
    }
}
