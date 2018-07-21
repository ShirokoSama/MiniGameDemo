using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urchin : MonoBehaviour {

    private MapObject mapObject;

    private void Start()
    {
        mapObject = GetComponent<MapObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && mapObject.detail.Triggerable == true)
        {

        }
    }
}
