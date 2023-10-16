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
	public Transform goal;
	public string sceneName;
    
    // Contact with Enemy
    public Transform enemies;
    public PlayerReturn playerReturn;

	// World Unit
    public float WorldUnit = 1.000f;

    //Menu
    public GameObject menuPanel;

    // Guide Panel
    public GameObject guidePanel;

    //Direction manager
    public DirectionManager directionManager;

    // Camera State
    public CameraState cameraState;

    // Player State
    private PlayerState playerState;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
		if (!menuPanel.activeSelf)
        {
            if(guidePanel==null || !guidePanel.activeSelf)
            {
                if (!cameraState.GetIsRotating())
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
                    if (cameraState.GetFacingDirection() == FacingDirection.Front)
                    {
                        trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, 0f);
                    }
                    else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                    {
                        trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, Vertical * movementSpeed * Time.deltaTime);
                    }
                    characterController.Move(trans);
                    TouchEnemy();
				    ReachGoal();
                }
            }
        }
    }
    private void HandlePlayerDeath()
    {
        characterController.enabled = false; // stop current movement.
        transform.position = playerReturn.checkPoint;
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            directionManager.UpdateInvisibleCubes();
        }else if(cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            directionManager.MovePlayerToClosestInvisibleCube();
        }
        directionManager.UpdateInvisibleCubes();
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

                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    if (Mathf.Abs(enemyPosition.y - transform.position.y) < WorldUnit &&
                        Mathf.Abs(enemyPosition.x - transform.position.x) < WorldUnit)
                    {
                        Debug.Log("Player touched the enemy and died!");
                        HandlePlayerDeath();
                        break;
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
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
}
