using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Left: -1  Right: 1
    private int Horizontal = 0;
    // Down: -1  Up: 1
    private int Vertical = 0;

    // Physical Parameter
    public float movementSpeed = 5f;
    public float gravity = 10f;
    private CharacterController characterController;
	
	// player action keyCode
	public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;

	// pick up key and open blocks
	public Transform blocks;
    public Transform keys;
    private int keyCounter = 0;
	public UnityEngine.UI.Text keyText;
	
	public Transform goal;
	public string sceneName;
    
    // Contact with Enemy
    public Transform enemies;
    public PlayerReturn playerReturn;

	// World Unit
    public float WorldUnit = 1.000f;

    //Menu
    public GameObject menuPanel;

    //Direction manager
    public GameObject directionManager;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
		keyText.text = "Key: " + keyCounter;
		if (!menuPanel.activeSelf)
        {
            if (!playerState.GetIsRotating())
            {
                if (playerState.GetUpIsDropping())
                {
                    Horizontal = 0;
                    Vertical = 0;
                }
                else
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
                }

                // Movement
                Vector3 trans = Vector3.zero;
                if (playerState.GetFacingDirection() == FacingDirection.Front)
                {
                    trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, 0f);
                }
                else if (playerState.GetFacingDirection() == FacingDirection.Up)
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
				reachGoal();
            }

        }
    }

    private void pickUpKey()
    {
        if (playerState.GetFacingDirection() == FacingDirection.Front)
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
        else if (playerState.GetFacingDirection() == FacingDirection.Up)
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
    private void HandlePlayerDeath()
    {
        characterController.enabled = false; // stop current movement.
        transform.position = playerReturn.checkPoint;
        directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
        characterController.enabled = true;
    }
    
    private void TouchEnemy()
    {
        foreach (Transform enemyNum in enemies)
        {
            Transform enemy = enemyNum.Find("EnemyModel");
            if (enemy.gameObject.activeSelf)
            {
                Vector3 enemyPosition = enemy.position;

                if (playerState.GetFacingDirection() == FacingDirection.Front)
                {
                    if (Mathf.Abs(enemyPosition.y - transform.position.y) < WorldUnit &&
                        Mathf.Abs(enemyPosition.x - transform.position.x) < WorldUnit)
                    {
                        Debug.Log("Player touched the enemy and died!");
                        HandlePlayerDeath();
                        break;
                    }
                }
                else if (playerState.GetFacingDirection() == FacingDirection.Up)
                {

                    if (Mathf.Abs(enemyPosition.z - transform.position.z) < WorldUnit &&
                        Mathf.Abs(enemyPosition.x - transform.position.x) < WorldUnit)
                    {
                        Debug.Log("Player touched the enemy and died!");
                        HandlePlayerDeath();
                        break;
                    }
                }
            }
        }
    }
    private void openDoor()
    {

        if (playerState.GetFacingDirection() == FacingDirection.Front)
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
        else if (playerState.GetFacingDirection() == FacingDirection.Up)
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

	private void reachGoal(){
		if (playerState.GetFacingDirection() == FacingDirection.Front)
        {
			if (Mathf.Abs(goal.position.y - transform.position.y) < WorldUnit&&
                    Mathf.Abs(goal.position.x - transform.position.x) < WorldUnit)
			{
				Debug.Log("Goal!!!");
				LoadScene(sceneName);
			}
		}
		else if (playerState.GetFacingDirection() == FacingDirection.Up)
        {
			if (Mathf.Abs(goal.position.z - transform.position.z) < WorldUnit&&
                    Mathf.Abs(goal.position.x - transform.position.x) < WorldUnit)
            {
				Debug.Log("Goal!!!");
				//现在是马上加载 感觉需要中间加入时间动画或者菜单
				LoadScene(sceneName);
			}
		}
	}
	/*
	public void healthIncrease(){
		health++;
		Debug.Log("Aughhhhh! "  + health);
	}
	*/
	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
