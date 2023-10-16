using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public CameraState cameraState;
    public PlayerState playerState;

    private Vector3 targetPoint;
    void Start()
    {
        targetPoint = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }
    
    void MoveEnemy()
    {
        if (!cameraState.isRotating && !playerState.positionUpdating)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

            // When the enemy reaches one of the points, it switches its target point to the other point.
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                if (targetPoint == pointA.position)
                    targetPoint = pointB.position;
                else
                    targetPoint = pointA.position;
            }
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // Logic to handle player death goes here
    //         Debug.Log("Player touched the enemy and died!");
    //     }
    // }
}
