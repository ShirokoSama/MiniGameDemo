using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCrystal : MonoBehaviour {

    public struct ShiftCrystalTrigger
    {
        public float yMin;
        public float yMax;
        public bool direction;

        public ShiftCrystalTrigger(float yMin, float yMax, bool dir)
        {
            this.yMin = yMin;
            this.yMax = yMax;
            this.direction = dir;
        }
    }

    public ShiftCrystalTrigger trigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        int direction = 0;
        if (trigger.direction) direction = 1;
        else direction = -1;
        if (other.tag == "Player" && GetComponent<MapObject>().detail.Triggerable)
        {
            Debug.Log("OnTriggerEnter");
            List<MapPiece> mapPieces = MapManager.instance.GetBetween(trigger.yMin, trigger.yMax);
            foreach (MapPiece mapPiece in mapPieces)
            {
                mapPiece.SetNewMoveOffset(new Vector2(1080 * direction, 0.0f), GetComponent<MapObject>().detail.duration);
            }
        }
    }
}
