using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class CharacterContoller : MonoBehaviour
    {
        public GameObject character;
        public float speed = 100f;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!Input.GetMouseButtonDown(0))
                return;
            Vector3 mousePosition = Input.mousePosition;

        }


    }
}
