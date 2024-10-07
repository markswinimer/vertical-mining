using System.Collections.Generic;
using UnityEngine;

public class VacuumEffect : MonoBehaviour
{
    public float vacuumForce = 5f;  // How fast objects get pulled towards the player
    public float liftForce = 4f;  // How fast objects get pulled towards the player
    private bool isVacuumActive = false;

    public GameObject suckPoint;
    
    private BoxCollider2D boxCollider;  // Reference to the Box Collider 2D

    private void Start()
    {
        // Get the Box Collider 2D attached to this object
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void StartVacuum()
    {
        isVacuumActive = true;
    }

    public void StopVacuum()
    {
        isVacuumActive = false;
    }

    // When an object enters the vacuum's range
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isVacuumActive)
        {
            // Check if the object is something you want to suck up
            if (other.CompareTag("Suckable"))  // Tag objects to be sucked up with "Suckable"
            {
                Vector2 vacuumCenter = suckPoint.GetComponent<BoxCollider2D>().bounds.center;

                float halfHeight = suckPoint.GetComponent<BoxCollider2D>().bounds.size.y / 2f;

                Vector2 direction = (vacuumCenter - (Vector2)other.transform.position).normalized;

                Vector2 lift = new Vector2(0, halfHeight) * liftForce;

                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

                rb.AddForce((direction + lift) * vacuumForce);
            }
        }
    }
}