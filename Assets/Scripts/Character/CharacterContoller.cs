using System;
using UnityEngine;

namespace HaruScene {
    public class CharacterContoller : MonoBehaviour
    {
        public GameObject character;
        public float accelaration = 1;
        public float resistance = 0.2f;
        private Vector3 speed;

        // Use this for initialization
        private void Start()
        {
            character.transform.position.Set(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                0
                );
            speed = new Vector3(0f, 0f, 0f);
        }

        // Update is called once per frame
        private void Update()
        {
            speed -= speed.normalized * resistance * Time.deltaTime;
            if (!Input.GetMouseButton(0))
                return;
            Vector3 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mousePostion - character.transform.position;
            speed += dir.normalized * accelaration * Time.deltaTime;
            character.transform.position += speed;
        }

    }
}
