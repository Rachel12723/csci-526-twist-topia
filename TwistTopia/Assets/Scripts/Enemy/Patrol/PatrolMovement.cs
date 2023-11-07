using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    private EnemyManager enemyManager;
    private CameraState cameraState;
    private PlayerState playerState;

    private PatrolManager patrolManager;
    private float offset;
    private float speed;

    private Vector3 originalPosition;
    public Vector3 pointA;
    public Vector3 pointB;
    private Vector3 targetPoint;
    void Start()
    {
        enemyManager = transform.parent.parent.gameObject.GetComponent<EnemyManager>();
        playerState = enemyManager.player.GetComponent<PlayerState>();
        cameraState = enemyManager.cameraState;

        patrolManager = GetComponentInParent<PatrolManager>();
        offset = patrolManager.offset;
        speed = patrolManager.speed;
        originalPosition = transform.position;
        pointA = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
        pointB = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        targetPoint = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();

    }

    public void ResetPatrol()
    {
        transform.position = originalPosition;
        UpdatePosition();
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
