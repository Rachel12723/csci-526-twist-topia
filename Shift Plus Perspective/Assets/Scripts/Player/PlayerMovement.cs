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
    private bool isRotating = false;
    private FacingDirection facingDirection;
    private float degree = 0;
    private float lastRotationX = 0f;
    private float currentRotationX = 0f;
	
	// player action keyCode
	public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;
	// pick up key and open blocks
	public Transform blocks;
    public Transform keys;
    private int keyCounter = 0;
	public UnityEngine.UI.Text keyText;
	private List<Transform> blockList = new List<Transform>();
    // Contact with Enemy
    public Transform enemies;

	// World Unit
    public float WorldUnit = 1.000f;

    //Menu
    public GameObject menuPanel;

    //Direction manager
    public GameObject directionManager;

    void Start()
    {

    }

    void Update()
    {
		keyText.text = "Key: " + keyCounter; // updates the displayed key count? where is text?
		if (!menuPanel.activeSelf) // If the menu is not active
        {
            if (!isRotating)
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
                //if (Input.GetKeyDown(pickUpKeyCode))
                //{
                    pickUpKey();
                TouchEnemy();
                //}
                if (Input.GetKeyDown(openDoorCode) && keyCounter > 0)
                {
                    openDoor();
                }
            }

            // Camera and Light Rotation
            Quaternion rotate = Quaternion.Slerp(transform.rotation, Quaternion.Euler(degree, 0, 0), 8 * Time.deltaTime);
            transform.rotation = rotate;
            lastRotationX = currentRotationX;
            currentRotationX = rotate.x;
            if (Mathf.Abs(currentRotationX - lastRotationX) < 0.00001)
            {
                isRotating = false;
            }else
            {
                isRotating = true;
            }
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
    
    private void TouchEnemy()
    {
        if (facingDirection == FacingDirection.Front)
        {
            foreach (Transform enemy in enemies)
            {
                if (Mathf.Abs(enemy.position.y - transform.position.y) < WorldUnit &&
                    Mathf.Abs(enemy.position.x - transform.position.x) < WorldUnit)
                {
                    Debug.Log("Player touched the enemy and died!");
                    break;
                }
            }
        }
        else if (facingDirection == FacingDirection.Up)
        {
            foreach (Transform enemy in enemies)
            {
                if (Mathf.Abs(enemy.position.z - transform.position.z) < WorldUnit &&
                    Mathf.Abs(enemy.position.x - transform.position.x) < WorldUnit)
                {
                    Debug.Log("Player touched the enemy and died!");
                    break;
                }
            } 
        }
    }

    private void openDoor()
    {

        if (facingDirection == FacingDirection.Front)
        {
            foreach (Transform block in blocks)
            {
                if (Mathf.Abs(block.position.y - transform.position.y) < WorldUnit + 0.5f &&
                    Mathf.Abs(block.position.x - transform.position.x) < WorldUnit + 0.5f)
                {
                    //Debug.Log("true dude!");
					directionManager.GetComponent<DirectionManager>().DeleteBlockCubes(block);
                    Destroy(block.gameObject);
                    keyCounter--;
                    Debug.Log("Keys:" + keyCounter);
                    break;
                }
            }
        }
        else if (facingDirection == FacingDirection.Up)
        {
            foreach (Transform block in blocks)
            {
                if (Mathf.Abs(block.position.z - transform.position.z) < WorldUnit + 0.5f &&
                    Mathf.Abs(block.position.x - transform.position.x) < WorldUnit + 0.5f)
                {
                    directionManager.GetComponent<DirectionManager>().DeleteBlockCubes(block);
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
	/*
	public void healthIncrease(){
		health++;
		Debug.Log("Aughhhhh! "  + health);
	}
	*/
}
