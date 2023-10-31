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
	private GameObject inHandKeypon;
    private int keyCounter = 0;
    public UnityEngine.UI.Text keyText;
    public CameraState cameraState;
    public DirectionManager directionManager;
    public float WorldUnit = 1.000f;
	private float xOffset = 1.2f;
    private float zOffset = -0.56f;
    private float rotationSpeed = 1800f;
	private bool hasCompletedFullRotation = false;
	private float rotationProgress = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
		
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
		
		
		if(hasCompletedFullRotation)
		{
			// Calculate the rotation angle for this frame
   			 float rotationAngle = rotationSpeed * Time.deltaTime;

   			// Increment the rotation progress
    		rotationProgress += rotationAngle;

    		// Rotate the object around the y-axis
    		transform.Rotate(Vector3.up, rotationAngle);
			float leftX = player.transform.position.x - 1.7f;
			float rightX = player.transform.position.x + 1.7f;
			float topZ = player.transform.position.z + 1.7f;
			float downZ = player.transform.position.z - 1.7f;
			float radius = 1.7f; 
			Vector3 playerPosition = player.transform.position;
			foreach(Transform enemy in player.GetComponent<PlayerMovement>().enemies){
				Transform enemyModel = enemy.Find("EnemyModel");
				Vector3 enemyPosition = enemyModel.position;
    			float distanceX = enemyPosition.x - playerPosition.x;
    			float distanceZ = enemyPosition.z - playerPosition.z;
    			float distance = Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
    			if (distance <= radius)
    			{
        			Destroy(enemy.gameObject);
    			}
				//Debug.Log(enemyModel.position.x);
				//Debug.Log(enemyModel.position.z);
				//if(enemyModel.position.x <= rightX && enemyModel.position.x >= leftX && enemyModel.position.z <= topZ && enemyModel.position.z >= downZ)
					//Destroy(enemy.gameObject);
			}
    		// Check if the rotation has completed (2 full rotations, 720 degrees)
    		if (rotationProgress >= 1080.0f)
    		{
				Vector3 targetRotation = new Vector3(0f, 0f, 0f);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime * 4);
				hasCompletedFullRotation = false;
        		// Reset the progress if you want the rotation to continue indefinitely
        		rotationProgress = 0.0f;

        		// Alternatively, you can stop the rotation by disabling the script
        		//enabled = false;
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
					inHandKeypon = Instantiate(keyponInHand, player.transform);
        			inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                    keyCounter+=2;
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
					inHandKeypon = Instantiate(keyponInHand, player.transform);
        			inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                    keyCounter+=2;
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
			if (!hasCompletedFullRotation)
        	{
				hasCompletedFullRotation = true;
			}
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
				KeyponDestroyed();
                Debug.Log("Keys:" + keyCounter);
                break;
            }
        }
	}
	private void KeyponDestroyed()
	{
		if(keyCounter == 0)
		{
			Destroy(inHandKeypon);
		}
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
