using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Left: -1  Right: 1
    private int Horizontal = 0;
    private float HorizontalFloat = 0;

    // Down: -1  Up: 1
    private int Vertical = 0;
    private float VerticalFloat = 0;

    // Physical Parameter
    public float movementSpeed = 5f;
    public float gravity = 10f;
    private CharacterController characterController;
	public Transform goal;
	public string sceneName;
    
    // Contact with Enemy
    public Transform patrols;
    public PlayerReturn playerReturn;

	// World Unit
    public float WorldUnit = 1.000f;

    //Direction manager
    public DirectionManager directionManager;

    // Camera State
    public CameraState cameraState;

    // Player State
    private PlayerState playerState;

    // Input Manager
    public InputManager inputManager;
    private int lastHorizontalFlag = 0;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (inputManager.GetAllowMove())
        {
            if (playerState.GetUpIsDropping())
            {
                Horizontal = 0;
                Vertical = 0;
            }
            else
            {
                // Left/Right Key
                if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ||
                    (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))))
                {
                    Horizontal = 0;
                }
                else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
                {
                    Horizontal = -1;
                }
                else if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
                {
                    Horizontal = 1;
                }

                // Down/Up Key
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ||
                    (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))))
                {
                    Vertical = 0;
                }
                else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
                {
                    Vertical = 1;
                }
                else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
                {
                    Vertical = -1;
                }
            }

            // Movement
            Vector3 trans = Vector3.zero;
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, 0f);
            }
            else if (cameraState.GetFacingDirection() == FacingDirection.Up)
            {
                trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, Vertical * movementSpeed * Time.deltaTime);
            }
            characterController.Move(trans);
            TurnRound();
            // TouchEnemy();
            ReachGoal();
        }
    }
    private void TouchEnemy()
    {
        foreach (Transform patrol in patrols)
        {
            // Transform enemy = enemyNum.Find("EnemyModel");
            if (patrol.gameObject.activeSelf)
            {
                Vector3 enemyPosition = patrol.position;
    
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    if (Mathf.Abs(enemyPosition.y - transform.position.y) < WorldUnit &&
                        Mathf.Abs(enemyPosition.x - transform.position.x) < WorldUnit)
                    {
                        Debug.Log("Player touched the enemy and died!");
                        playerReturn.ResetPlayer();
                        break;
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
    
                    if (Mathf.Abs(enemyPosition.z - transform.position.z) < WorldUnit &&
                        Mathf.Abs(enemyPosition.x - transform.position.x) < WorldUnit)
                    {
                        Debug.Log("Player touched the enemy and died!");
                        playerReturn.ResetPlayer();
                        break;
                    }
                }
            }
        }
    }
	private void ReachGoal(){
		if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
			if (Mathf.Abs(goal.position.y - transform.position.y) < WorldUnit&&
                    Mathf.Abs(goal.position.x - transform.position.x) < WorldUnit)
			{
				Debug.Log("Goal!!!");
				LoadScene(sceneName);
			}
		}
		else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
			if (Mathf.Abs(goal.position.z - transform.position.z) < WorldUnit&&
                    Mathf.Abs(goal.position.x - transform.position.x) < WorldUnit)
            {
				Debug.Log("Goal!!!");
				LoadScene(sceneName);
			}
		}
	}
	public void LoadScene(string sceneName)
    {
        int level = PlayerPrefs.GetInt("Level");
        char lastChar = sceneName[sceneName.Length - 1];
        int lastDigit = int.Parse(lastChar.ToString());
        if (lastDigit - 1 > level)
        {
            PlayerPrefs.SetInt("Level", lastDigit - 1);
        }

        SceneManager.LoadScene(sceneName);
    }

    private void TurnRound()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // right is 1/left is -1/stationary is 0
        int horizontalFlag = 0;
        if (horizontalInput > 0)
        {
            horizontalFlag = 1;
        }
        else if(horizontalInput < 0)
        {
            horizontalFlag = -1;
        }
        else
        {
            horizontalFlag = 0;
        }
        //Debug.Log(horizontalFlag-lastHorizontalInput);
        if (Mathf.Abs(horizontalFlag-lastHorizontalFlag)==2)
        {
            //Debug.Log(horizontalInput);
            Debug.Log("Horizontal direction changed");
            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y += 180f;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        if (horizontalFlag != 0)
        {
            lastHorizontalFlag = horizontalFlag;
        }
    }

    public int getLastHorizontalFlag()
    {
        return lastHorizontalFlag;
    }
}
