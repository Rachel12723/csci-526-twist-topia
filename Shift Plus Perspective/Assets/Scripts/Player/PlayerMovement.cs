using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Left: -1  Right: 1
    private int Horizontal = 0;
    // Down: -1  Up: 1
    private int Vertical = 0;

    // Physical Parameter
    public float movementSpeed = 5f;
    public float gravity = 8f;
    public CharacterController characterController;

    // Camera and Light Rotation
    private bool isRotating=false;
    private FacingDirection facingDirection;
    private float degree = 0;
    private float lastRotationX = 0f;
    private float currentRotationX = 0f;
	
	public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;
	// pick up key and open building
	public Transform buildings;
    public Transform keys;
    private int keyCounter = 0;
	private List<Transform> buildingBlockList = new List<Transform>();

	// World Unit
    public float WorldUnit = 1.000f;

    //Menu
    public GameObject menuPanel;

    void Start()
    {

    }

    void Update()
    {
		if (!menuPanel.activeSelf)
        {
        	// Left/Right Key
        	if (Input.GetAxis("Horizontal") < 0)
        	{
            	Horizontal = -1;
        	}
        	else if (Input.GetAxis("Horizontal") > 0)
        	{
            	Horizontal = 1;
        	}
        	else
        	{
            	Horizontal = 0;
        	}

        	// Down/Up Key
        	if (Input.GetAxis("Vertical") < 0)
        	{
            	Vertical = -1;
        	}
        	else if (Input.GetAxis("Vertical") > 0)
        	{
            	Vertical = 1;
        	}
        	else
       		{
            	Vertical = 0;
        	}

        	// Movement
        	Vector3 trans = Vector3.zero;
        	if (facingDirection == FacingDirection.Front)
        	{
            	trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, 0f);
        	}
        	else if (facingDirection == FacingDirection.Up)
        	{
            	trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, Vertical * movementSpeed * Time.deltaTime);
        	}
        	characterController.Move(trans);

        	// Camera and Light Rotation
        	Quaternion rotate = Quaternion.Slerp(transform.rotation, Quaternion.Euler(degree, 0, 0), 8 * Time.deltaTime);
        	transform.rotation = rotate;
        	lastRotationX = currentRotationX;
        	currentRotationX = rotate.x;
        	if (Mathf.Abs(currentRotationX - lastRotationX) < 0.0001)
        	{
            	isRotating = false;
        	}else
        	{
            	isRotating = true;
        	}
		}
		//if (Input.GetKeyDown(pickUpKeyCode))
		//{
			pickUpKey();
        //}
        if (Input.GetKeyDown(openDoorCode) && keyCounter > 0)
        {
            openDoor();
        }
    }

    // Update the Facing Firection
    public void UpdateFacingDirection(FacingDirection newDirection)
    {
        facingDirection = newDirection;
        if (facingDirection == FacingDirection.Front)
        {
            degree = 0f;
        }
        else if (facingDirection == FacingDirection.Up)
        {
            degree = 90f;
        }
    }

    // Set isRotating
    public void SetIsRotating(bool isRotating)
    {
        this.isRotating = isRotating;
    }

    // Get isRotating
    public bool GetIsRotating()
    {
        return isRotating;
    }
    private void pickUpKey()
    {
        if (facingDirection == FacingDirection.Front)
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
        else if (facingDirection == FacingDirection.Up)
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

        if (facingDirection == FacingDirection.Front)
        {
            int index = 0;
            foreach (Transform buildingBlock in buildings)
            {
                if (Mathf.Abs(buildingBlock.position.y - transform.position.y) < WorldUnit + 0.5f &&
                    Mathf.Abs(buildingBlock.position.x - transform.position.x) < WorldUnit + 0.5f)
                {
                    //Debug.Log("true dude!");
                    Destroy(buildingBlock.gameObject);
                    keyCounter--;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }

                index++;
            }
        }
        else if (facingDirection == FacingDirection.Up)
        {
            int index = 0;
            foreach (Transform buildingBlock in buildings)
            {
                if (Mathf.Abs(buildingBlock.position.z - transform.position.z) < WorldUnit + 0.5f &&
                    Mathf.Abs(buildingBlock.position.x - transform.position.x) < WorldUnit + 0.5f)
                {
                    Destroy(buildingBlock.gameObject);
                    keyCounter--;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
                index++;
            } 
        }
    }
}
