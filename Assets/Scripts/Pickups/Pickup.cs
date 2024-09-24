using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHitGround = false;

    private void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground") && !hasHitGround)
    //     {
    //         hasHitGround = true; 
    //         // StartCoroutine(ApplyFrictionAfterGroundHit());
    //     }
    // }
}