using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAction : MonoBehaviour
{
    public GameObject player; // Drag your player object here
    public GameObject enemyModel;  // Drag your enemy object here
    public CameraState cameraState;  
    public KeyCode catchEnemy;
    public float xTolerance = 0.5f; // A small value to account for minor discrepancies in z-position
    public float yTolerance = 0.5f;
    public float proximityThreshold = 5.0f; // Distance within which frame is considered "close" to player
    
    private SpriteRenderer spriteRenderer;
    public Sprite frameWithEnemy;
    public Sprite frameWithoutEnemy;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frameWithoutEnemy;
    }
    
    void Update() {
        if (Input.GetKeyDown(catchEnemy)) {
            if (cameraState.facingDirection == FacingDirection.Front) {
                Debug.Log("return key pressed and the direction is front");
                float playerXDistanceToFrame = Math.Abs(player.transform.position.x - transform.position.x);
                float playerYDistanceToFrame = Math.Abs(player.transform.position.y - transform.position.y);
                float enemyXDistanceToFrame = Math.Abs(enemyModel.transform.position.x - transform.position.x);

                Debug.Log("player" + player.transform.position + "frame location" + transform.position + "enemy" + enemyModel.transform.position);
                Debug.Log("playerXDistanceToFrame" + playerXDistanceToFrame + "playerYDistanceToFrame" + playerYDistanceToFrame + "enemyXDistanceToFrame" + enemyXDistanceToFrame);
                if (playerXDistanceToFrame <= proximityThreshold && playerYDistanceToFrame <= yTolerance &&
                    enemyXDistanceToFrame <= xTolerance) {
                    CaptureEnemy();
                }
            }
        }
    }

    void CaptureEnemy() {
        // Deactivate the enemy
        enemyModel.SetActive(false);
        // Destroy(enemyModel);
        spriteRenderer.sprite = frameWithEnemy;
    
        // Optionally, change the frame's appearance to indicate the enemy is captured
    }
    
    // float DistanceXY(Vector3 pointA, Vector3 pointB) {
    //     return Mathf.Sqrt(Mathf.Pow(pointB.x - pointA.x, 2) + Mathf.Pow(pointB.y - pointA.y, 2));
    // }



}
