using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float offset = 2f;
    public float speed = 2f;
    public CameraState cameraState;
    public PlayerState playerState;
    public Transform patrols;
    private bool isChasing = false;
    private Vector3 targetPoint;
    void Start()
    {   
        pointA = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
        pointB = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        targetPoint = pointA;
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
                if (targetPoint == pointA)
                    targetPoint = pointB;
                else
                    targetPoint = pointA;
            }
        }
    }

    void AttackOtherEnemy()
    {
    }
}
