using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    private PatrolManager patrolManager;
    private float offset;
    private float speed;
    private CameraState cameraState;
    private PlayerState playerState;
    public Vector3 pointA;
    public Vector3 pointB;
    private Vector3 targetPoint;
    void Start()
    {
        patrolManager = GetComponentInParent<PatrolManager>();
        offset = patrolManager.offset;
        speed = patrolManager.speed;
        cameraState = patrolManager.cameraState;
        playerState = patrolManager.playerState;
        pointA = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
        pointB = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        targetPoint = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();

    }

    public void UpdatePosition()
    {
        pointA = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
        pointB = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        targetPoint = pointA;
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
}
