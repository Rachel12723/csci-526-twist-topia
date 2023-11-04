using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrame : MonoBehaviour
{
    // player action keyCode
    public KeyCode dropOffFrameCode;
    private int frameCounter = 0;
    public Transform frames;
    public UnityEngine.UI.Text frameText;
    public CameraState cameraState;
    // public DirectionManager directionManager;
    public float WorldUnit = 1.000f;
    public int maxZ;
    private float yOffset = 1.15625f;
	// public Transform enemies;
    // private EnemyManager enemyManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frameText.text = "Frame: " + frameCounter;
        //PickUpKey();
		PickUpFrame();
        if (Input.GetKeyDown(dropOffFrameCode) && frameCounter > 0)
        {
            DropOffFrame();
        }
    }

	private void PickUpFrame()
    {
        if (frames)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                foreach (Transform frame in frames)
                {
                    if (frame.gameObject.activeSelf && Mathf.Abs(frame.position.x - transform.position.x) < WorldUnit + 0.5f)
                    {
                        frame.gameObject.SetActive(false);
                        frameCounter++;
                        break;
                    }
                }
            }
        }
    }

	private void DropOffFrame(){
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            Transform frame = frames.GetChild(0);
            // Debug.Log(" x"+frame.position.x+" y"+frame.position.y+" z"+frame.position.z);
            Vector3 playerCurrPos = transform.position;
            frame.position = new Vector3(playerCurrPos.x, playerCurrPos.y + yOffset - 1, maxZ);
            // Debug.Log(" x"+frame.position.x+" y"+frame.position.y+" z"+frame.position.z);
            // Debug.Log(frame.gameObject.activeInHierarchy);
            frame.gameObject.SetActive(true);
            Debug.Log(frame.gameObject.activeInHierarchy);
            frameCounter--;
        }
	}

    public void ResetFrame(){

    }
}
