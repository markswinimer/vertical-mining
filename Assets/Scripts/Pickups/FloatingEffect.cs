using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    private Vector3 originalPosition;
    private float floatTimer;
    public float velocityThreshold = 0.01f;  // Threshold to determine if the object has stopped

    private bool floatingStarted = false;  // Ensure floating starts only once
    private float stillnessTimer = 0f;  // Track how long the object has been still
    private Rigidbody2D rb;  // Reference to the Rigidbody2D

    void Start()
    {
        // Store the original position to float around
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
    }

    void Update()
    {
        // Check if the object is still (velocity is close to zero)
        if (rb.velocity.magnitude < velocityThreshold)
        {
            // Increment the stillness timer
            stillnessTimer += Time.deltaTime;

            // Check if the object has been still for the required time
            if (stillnessTimer >= stillnessDelay && !floatingStarted)
            {
                StartFloating();
            }
        }
        else
        {
            // Reset the stillness timer if the object is moving
            stillnessTimer = 0f;
        }

        // Perform the floating effect if it's started
        if (floatingStarted)
        {
            PerformFloating();
        }
    }

    void StartFloating()
    {
        // Mark the floating as started and reset the original position for floating
        floatingStarted = true;
        originalPosition = transform.position;
    }

    public float floatSpeed = 1f;  // Speed of the floating motion
    public float floatAmplitude = 0.1f;  // The height of the floating motion
    public float stillnessDelay = 0.3f;  // Delay before floating starts when object is still

    void PerformFloating()
    {
        // Calculate floating motion using a sin wave
        floatTimer += Time.deltaTime * floatSpeed;
        float newY = originalPosition.y + Mathf.Sin(floatTimer) * floatAmplitude;

        // Apply the new Y position (keeping the X and Z the same)
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}