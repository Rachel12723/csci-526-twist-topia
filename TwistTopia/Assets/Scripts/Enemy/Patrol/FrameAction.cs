using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAction : MonoBehaviour
{
    public GameObject player; // Drag your player object here
    public Transform patrols;  // Drag your enemy object here
    public CameraState cameraState; 
    public Camera camera;
    public KeyCode catchEnemy;
    public float xTolerance = 0.5f; // A small value to account for minor discrepancies in z-position
    public float yTolerance = 0.5f;
    public float proximityThreshold = 5.0f; // Distance within which frame is considered "close" to player
    
    private SpriteRenderer spriteRenderer;
    public Sprite frameWithEnemy;
    public Sprite frameWithoutEnemy;

    //Data
    public delegate void EnemyEventHandler(string road);
    public static event EnemyEventHandler OnEnemyCatched;

    public delegate void EnterEventHandler(Vector3 player, Vector3 frame, Vector3 enemy);
    public static event EnterEventHandler OnEnemyNotCatched;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        spriteRenderer.sprite = frameWithoutEnemy;
    }
    
    void Update() {
        if (Input.GetKeyDown(catchEnemy)) {
            if (cameraState.facingDirection == FacingDirection.Front) {
                Debug.Log("return key pressed and the direction is front");
                // Vector3 playerLoc = player.transform.position;
                Vector3 frameLoc = transform.position;
                Vector3 frameScreenPos = camera.WorldToScreenPoint(frameLoc);
                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

                bool isFrameOnScreen = (frameScreenPos.x >= 0 && frameScreenPos.x <= Screen.width) && 
                                  (frameScreenPos.y >= 0 && frameScreenPos.y <= Screen.height);
                // float playerXDistanceToFrame = Math.Abs(playerLoc.x - frameLoc.x);
                // float playerYDistanceToFrame = Math.Abs(playerLoc.y - frameLoc.y);
                foreach (Transform patrol in patrols)
                {
                    float enemyXDistanceToFrame = Math.Abs(patrol.position.x - frameLoc.x);
                
                    // Debug.Log("player" + playerLoc + "frame location" + frameLoc + "enemy" + enemyModel.transform.position);
                    // Debug.Log("playerXDistanceToFrame" + playerXDistanceToFrame + "playerYDistanceToFrame" + playerYDistanceToFrame + "enemyXDistanceToFrame" + enemyXDistanceToFrame);
                    // if (playerXDistanceToFrame <= proximityThreshold && playerYDistanceToFrame <= yTolerance &&
                    //     enemyXDistanceToFrame <= xTolerance) {
                    if (isFrameOnScreen && enemyXDistanceToFrame <= xTolerance) {
                        CaptureEnemy(patrol);
                        OnEnemyCatched?.Invoke(patrol.tag);
                    }
                    else
                    {
                        OnEnemyNotCatched?.Invoke(player.transform.position, transform.position, patrol.transform.position);
                    }
                }

            }
        }
    }

    void CaptureEnemy(Transform patrol) {
        // Deactivate the enemy
        patrol.gameObject.SetActive(false);
        // Destroy(enemyModel);
        spriteRenderer.sprite = frameWithEnemy; // change the frame's appearance to indicate the enemy is captured
    }
    
    public void ReleaseEnemy(bool user) {
        // Reactivate the enemy
        foreach (Transform patrol in patrols)
        {
            if (!patrol.gameObject.activeSelf)
            {   // if not user initiated(means user died), original position
                if (user)
                {
                    patrol.position = player.transform.position;
                }
                patrol.gameObject.SetActive(true);
            }
        }
        spriteRenderer.sprite = frameWithoutEnemy;
    }
}
