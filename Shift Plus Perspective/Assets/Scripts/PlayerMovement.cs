using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private float leftBoundary;
    private float rightBoundary;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        // Calculate the next position
        float nextXPosition = transform.position.x + horizontal * moveSpeed * Time.deltaTime;
        
        // Check if the next position is within the boundaries
        if (nextXPosition > leftBoundary && nextXPosition < rightBoundary)
        {
            transform.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Calculate boundaries based on the platform's size and position
            Renderer platformRenderer = collision.gameObject.GetComponent<Renderer>();
            if (platformRenderer)
            {
                float platformWidth = platformRenderer.bounds.size.x;
                float platformCenter = collision.gameObject.transform.position.x;

                leftBoundary = platformCenter - platformWidth / 2;
                rightBoundary = platformCenter + platformWidth / 2;
            }
        }
    }
}
