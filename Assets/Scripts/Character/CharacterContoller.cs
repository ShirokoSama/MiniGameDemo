using System;
using UnityEngine;

namespace HaruScene {
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            print(ray);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }
            Vector3 hitPosition = hit.point;
            Vector3 dir = hitPosition - character.transform.position;
            character.transform.position += speed * Time.deltaTime * dir;
        }

    }
}
