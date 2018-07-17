using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferCrystal : MonoBehaviour {

    public int targetIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && GetComponent<MapObject>().detail.triggerable)
        {

        }
    }
}
