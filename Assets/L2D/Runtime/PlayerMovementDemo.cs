using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace L2D
{
    public class PlayerMovementDemo : MonoBehaviour
    {
        public float speed = 1;
        [HideInInspector] public Vector3 movement;

        private void FixedUpdate()
        {

            movement = new Vector3();

            if (Input.GetKey(KeyCode.W))
                movement += Vector3.up;
            if (Input.GetKey(KeyCode.S))
                movement += Vector3.down;
            if (Input.GetKey(KeyCode.A))
                movement += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                movement += Vector3.right;

            movement.Normalize();
            movement *= speed * Time.fixedDeltaTime;

            transform.position += movement;
        }
    }
}