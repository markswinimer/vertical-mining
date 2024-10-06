using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private float _damage = 40f;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    void Start()
    {
        rb.velocity = transform.right * speed;    
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Enemy enemy = other.GetComponent<Enemy>();

        Instantiate(impactEffect, transform.position, transform.rotation);
        
        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
