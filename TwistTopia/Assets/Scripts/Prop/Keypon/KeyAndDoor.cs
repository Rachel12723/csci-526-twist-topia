using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAndDoor : MonoBehaviour
{
    public KeyCode unlockSlashCode;
    public Transform player;
    public Transform blocks;
	public Transform keypons;
	public InputManager inputManager;
	public GameObject keyponInHand;
	private GameObject inHandKeypon;
    private int keyCounter = 0;
    public UnityEngine.UI.Text keyText;
    public CameraState cameraState;
    public DirectionManager directionManager;
    public float WorldUnit = 1.000f;
	public Transform enemies;
    private EnemyManager enemyManager;
	private float xOffset = 1.2f;
    private float zOffset = -0.56f;
    private float rotationSpeed = 1800f;
	private bool hasCompletedFullRotation = false;
	private float rotationProgress = 0.0f;
	private PlayerMovement playerMovement;
	private int lastHorizontalFlag;
    // Start is called before the first frame update
    void Start()
    {
        if (player !=null)
		{
			playerMovement = player.GetComponent<PlayerMovement>();
		}
        if (enemies != null)
        {
            enemyManager = enemies.GetComponent<EnemyManager>();
        }
        else
        {
            Debug.Log("KeyAndDoor.cs: No enemies!");
        }
    }

    // Update is called once per frame
    void Update()
    {
		lastHorizontalFlag = playerMovement.getLastHorizontalFlag();
        keyText.text = "Key: " + keyCounter;
		PickUpKeypon();
		if (inputManager.GetAllowInteraction())
		{
            int state = PlayerPrefs.GetInt("state");

            if (state == 1 && Input.GetKeyDown(unlockSlashCode) && keyCounter > 0)
			{
				SlashAndOpen();
			}
		}
		if(hasCompletedFullRotation)
		{
			// Calculate the rotation angle for this frame
   			float rotationAngle = rotationSpeed * Time.deltaTime;
   			// Increment the rotation progress
    		rotationProgress += rotationAngle;
    		// Rotate the object around the y-axis
    		player.transform.Rotate(Vector3.up, rotationAngle);
			float radius = 1.7f; 
			Vector3 playerPosition = player.transform.position;
			if(enemies != null){
				foreach(Transform enemy in enemies)
				{ 
					foreach(Transform enemyInstance in enemy)
					{
						Vector3 enemyPosition = enemyInstance.position;
    					float distanceX = enemyPosition.x - playerPosition.x;
    					float distanceZ = enemyPosition.z - playerPosition.z;
    					float distance = Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
    					if (distance <= radius)
    					{
                            enemyManager.DestroyEnemy(enemyInstance);
    					}
					}
				}
			}
    		// Check if the rotation has completed (3 full rotations, 1080 degrees)
    		if (rotationProgress >= 1080.0f)
    		{
				Vector3 targetRotation = new Vector3(0f, 0f, 0f);
				player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, 
					Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime * 4);
				hasCompletedFullRotation = false;
        		rotationProgress = 0.0f;
    		}
		}
		KeyponDestroyed();
    }
	
	private void PickUpKeypon()
    {
        if (keypons != null)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                foreach (Transform keypon in keypons)
                {
                    if (Mathf.Abs(keypon.position.y - player.transform.position.y) < WorldUnit + 0.5f &&
                        Mathf.Abs(keypon.position.x - player.transform.position.x) < WorldUnit + 0.5f)
                    {
					
                        Destroy(keypon.gameObject);
					    inHandKeypon = Instantiate(keyponInHand, player.transform);
						if(lastHorizontalFlag>0)
						{
							inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
						}
        			    else if(lastHorizontalFlag<0)
						{
							inHandKeypon.transform.Rotate(0f, 180f, 0f);
							inHandKeypon.transform.localPosition = new Vector3(-xOffset, 0, -zOffset);
						}
                        keyCounter++;
                        //
                        int keyponnum = PlayerPrefs.GetInt("Keypon");
                        PlayerPrefs.SetInt("Keypon", keyponnum + 1);
                        //
                        Debug.Log("Keys:" + keyCounter);
                        break;
                    }
                }
            }
            else if (cameraState.GetFacingDirection() == FacingDirection.Up)
            {
                foreach (Transform keypon in keypons)
                {
                    if (Mathf.Abs(keypon.position.z - player.transform.position.z) < WorldUnit + 0.25f &&
                        Mathf.Abs(keypon.position.x - player.transform.position.x) < WorldUnit + 0.25f)
                    {
                        Destroy(keypon.gameObject);
					    inHandKeypon = Instantiate(keyponInHand, player.transform);
        			    if(lastHorizontalFlag>0)
						{
							inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
						}
        			    else if(lastHorizontalFlag<0)
						{
							inHandKeypon.transform.Rotate(0f, 180f, 0f);
							inHandKeypon.transform.localPosition = new Vector3(-xOffset, 0, -zOffset);
						}
                        keyCounter++;
                        //
                        int keyponnum = PlayerPrefs.GetInt("Keypon");
                        PlayerPrefs.SetInt("Keypon", keyponnum + 1);
                        //
                        Debug.Log("Keys:" + keyCounter);
                        break;
                    }
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
			if (!hasCompletedFullRotation)
        	{
				hasCompletedFullRotation = true;
			}
        }
    }
	private void FrontViewOpenDoor(){
		foreach (Transform block in blocks)
        {
			bool canOpen = false;
            foreach (Transform blockCube in block)
            {
				if (Mathf.Abs(blockCube.position.y - player.transform.position.y) < WorldUnit + 0.5f &&
                Mathf.Abs(blockCube.position.x - player.transform.position.x) < WorldUnit + 0.5f)
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
                //
                int keyponnum = PlayerPrefs.GetInt("Keypon");
                PlayerPrefs.SetInt("Keypon", keyponnum - 1);
                //
                KeyponDestroyed();
                Debug.Log("Keys:" + keyCounter);
                break;
            }
        }
	}
	private void KeyponDestroyed()
	{
		if(keyCounter == 0 && inHandKeypon!=null)
		{
			Destroy(inHandKeypon);
		}
	}
    public void KeyDrop(){
        if(keyCounter > 0){
            keyCounter--;
            //
            int keyponnum = PlayerPrefs.GetInt("Keypon");
            PlayerPrefs.SetInt("Keypon", keyponnum - 1);
            //
            Debug.Log("Oops! Be careful! " + keyCounter);
        }
    }

    public int GetKeyCounter()
    {
        return keyCounter;
    }
}
