using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAndDoor : MonoBehaviour
{
    // player action keyCode
    public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;
	public KeyCode SlashCode;

    // pick up key and open blocks
    public Transform blocks;
    public Transform keys;
	public Transform keypons;
	public GameObject keyponInHand;
	public GameObject player;
    private int keyCounter = 0;
    public UnityEngine.UI.Text keyText;
    public CameraState cameraState;
    public DirectionManager directionManager;
    public float WorldUnit = 1.000f;
	private float xOffset = 1.2f;
    private float zOffset = -0.56f;
	private bool firstRotate = false;
	private bool secondRotate = false;
	private bool thirdRotate = false;
	private float currentRotation = 0f;
    private float targetRotation = 20f;
    private float rotationSpeed = 30f; // Adjust the rotation speed as needed
    // Start is called before the first frame update
    void Start()
    {
        //UpViewSlash();
    }

    // Update is called once per frame
    void Update()
    {
        keyText.text = "Key: " + keyCounter;
        PickUpKey();
		PickUpKeypon();
        if (Input.GetKeyDown(openDoorCode) && keyCounter > 0)
        {
            //OpenDoor();
			SlashAndOpen();
        }
		
		if (firstRotate)
        {
            // Rotate the object around its Y-axis by 20 degrees
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * 3);

            // Check if the first rotation is complete (20 degrees)
            if (transform.rotation.eulerAngles.y >= 20.0f)
            {
                firstRotate = false;
                secondRotate = true;
            }
        }
        else if (secondRotate)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime * 6);
            if (transform.rotation.eulerAngles.y <= 300.0f && transform.rotation.eulerAngles.y > 25.0f)
            {
                secondRotate = false;
                thirdRotate = true;
            }
        }
        else if (thirdRotate)
        {
			Vector3 targetRotation = new Vector3(0f, 0f, 0f);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime * 4);
			if (transform.rotation.eulerAngles.y == 0)
            {	
				
                thirdRotate = false;
            }
        }
    }
	private void PickUpKeypon()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform keypon in keypons)
            {
                if (Mathf.Abs(keypon.position.y - transform.position.y) < WorldUnit &&
                    Mathf.Abs(keypon.position.x - transform.position.x) < WorldUnit)
                {
					
                    Destroy(keypon.gameObject);
					GameObject inHandKeypon = Instantiate(keyponInHand, player.transform);
        			inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                    keyCounter++;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            foreach (Transform keypon in keypons)
            {
                if (Mathf.Abs(keypon.position.z - transform.position.z) < WorldUnit &&
                    Mathf.Abs(keypon.position.x - transform.position.x) < WorldUnit)
                {
                    Destroy(keypon.gameObject);
					GameObject inHandKeypon = Instantiate(keyponInHand, player.transform);
        			inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                    keyCounter++;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            } 
        }
    }
	private void SlashAndOpen()
    {

        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            FrontViewOpenDoor();
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
			//UpViewSlash();
			if(!firstRotate&&!secondRotate&&!thirdRotate)
			firstRotate = true;
			/*
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
			*/
        }
    }
	private void FrontViewOpenDoor(){
		foreach (Transform block in blocks)
        {
			bool canOpen = false;
            foreach (Transform blockCube in block)
            {
				if (Mathf.Abs(blockCube.position.y - transform.position.y) < WorldUnit + 0.5f &&
                Mathf.Abs(blockCube.position.x - transform.position.x) < WorldUnit + 0.5f)
            	{
					canOpen = true;
                    break;
                }
            }
            if (canOpen)
            {
                directionManager.DeleteBlockCubes(block);
                Destroy(block.gameObject);
                keyCounter--;
                Debug.Log("Keys:" + keyCounter);
                break;
            }
        }
	}
	private void UpViewSlash(){
		
		//rotateStarted = false;
		
	}
    private void PickUpKey()
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
    private void OpenDoor()
    {

        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform block in blocks)
            {
                bool canOpen = false;
                foreach (Transform blockCube in block)
                {
                    if (Mathf.Abs(blockCube.position.y - transform.position.y) < WorldUnit + 0.5f &&
                        Mathf.Abs(blockCube.position.x - transform.position.x) < WorldUnit + 0.5f)
                    {
                        canOpen = true;
                        break;
                    }
                }
                if (canOpen)
                {
                    directionManager.DeleteBlockCubes(block);
                    Destroy(block.gameObject);
                    keyCounter--;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
                //if (Mathf.Abs(block.position.y - transform.position.y) < WorldUnit + 0.5f &&
                //    Mathf.Abs(block.position.x - transform.position.x) < WorldUnit + 0.5f)
                //{
                //    //Debug.Log("true dude!");
                //    directionManager.DeleteBlockCubes(block);
                //    Destroy(block.gameObject);
                //    keyCounter--;
                //    Debug.Log("Keys:" + keyCounter);
                //    break;
                //}
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

    public void KeyDrop(){
        if(keyCounter > 0){
            keyCounter--;
            Debug.Log("Oops! Be careful! " + keyCounter);
        }
    }

    public int GetKeyCounter()
    {
        return keyCounter;
    }
}
