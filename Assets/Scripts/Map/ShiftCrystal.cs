using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCrystal : MonoBehaviour {

    [System.Serializable]
    public struct ShiftCrystalTrigger
    {
        public List<int> index;
        public bool direction;
        public ShiftCrystalTrigger(List<int> index, bool direction)
        {
            this.index = index;
            this.direction = direction;
        }
    }

    public ShiftCrystalTrigger trigger;
    private MapObject mapObject;

    private void Start()
    {
        mapObject = GetComponent<MapObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int direction = 0;
        if (trigger.direction) direction = 1;
        else direction = -1;
        if (other.tag == "Player" && mapObject.detail.Triggerable)
        {
            foreach (int index in trigger.index)
            {
                MapPiece piece = MapManager.instance.Get(index);
                piece.SetNewMoveOffset(new Vector2(1080f * direction, 0.0f), 5.0f);
            }
            mapObject.detail.TriggerBreak(5.0f);
            AudioController.instance.PlayGetItem();
        }
    }
}
