using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyandDoor : MonoBehaviour
{
    // player action keyCode
    public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;

    // pick up key and open blocks
    public Transform blocks;
    public Transform keys;
    private int keyCounter = 0;
    public UnityEngine.UI.Text keyText;
    public CameraState cameraState;
    public DirectionManager directionManager;
    public float WorldUnit = 1.000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        keyText.text = "Key: " + keyCounter;
        pickUpKey();
        if (Input.GetKeyDown(openDoorCode) && keyCounter > 0)
        {
            openDoor();
        }
    }
    private void pickUpKey()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform key in keys)
            {
                if (Mathf.Abs(key.position.y - transform.position.y) < WorldUnit &&
                    Mathf.Abs(key.position.x - transform.position.x) < WorldUnit)
                {
                    Destroy(key.gameObject);
                    keyCounter++;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            foreach (Transform key in keys)
            {
                if (Mathf.Abs(key.position.z - transform.position.z) < WorldUnit &&
                    Mathf.Abs(key.position.x - transform.position.x) < WorldUnit)
                {
                    Destroy(key.gameObject);
                    keyCounter++;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            } 
        }
    }
    private void openDoor()
    {

        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform block in blocks)
            {
                if (Mathf.Abs(block.position.y - transform.position.y) < WorldUnit + 0.5f &&
                    Mathf.Abs(block.position.x - transform.position.x) < WorldUnit + 0.5f)
                {
                    //Debug.Log("true dude!");
                    directionManager.DeleteBlockCubes(block);
                    Destroy(block.gameObject);
                    keyCounter--;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            bool canOpen = false;
            foreach (Transform block in blocks)
            {
                foreach (Transform blockCube in block)
                {
                    if (Mathf.Abs(blockCube.position.z - transform.position.z) < WorldUnit + 0.5f &&
                        Mathf.Abs(blockCube.position.x - transform.position.x) < WorldUnit + 0.5f)
                    {
                        canOpen = true;
                        break;
                    }
                }
                if(canOpen)
                {
                    directionManager.DeleteBlockCubes(block);
                    Destroy(block.gameObject);
                    keyCounter--;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            }
        }
    }

    public void keyDrop(){
        if(keyCounter > 0){
            keyCounter--;
            Debug.Log("Oops! Be careful! " + keyCounter);
        }
    }
}
