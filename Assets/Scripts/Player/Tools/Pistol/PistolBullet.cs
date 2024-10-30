using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    public float speed = 20f;
    private int _damage = 3;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    void Start()
    {
        rb.velocity = transform.right * speed;    
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        Debug.Log("other = " + other);

        GameObject impactEffectVFX = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEffectVFX, 2f);

        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
