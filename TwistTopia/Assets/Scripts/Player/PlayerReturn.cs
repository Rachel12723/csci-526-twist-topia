using UnityEngine;
using System.Collections;

public class PlayerReturn : MonoBehaviour
{

    public GameObject player; // references the main player object 
    private PlayerMovement playerMovement; // holds a reference to the PlayerMovement.cs component of the player
    public Vector3 checkPoint = Vector3.zero; // used to reset the player's location
    public float MinY; // Threshold for the player's y-coord. Indicates the player has fallen
    public GameObject directionManager;

	void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (player.transform.position.y < MinY)
        {
            player.GetComponent<CharacterController>().enabled = false; // stop current movement.
            playerMovement.keyDrop(); // ？
            player.transform.position = checkPoint;
            directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
            player.GetComponent<CharacterController>().enabled = true;
        }
        
    }

    public void SetCheckPoint(Vector3 checkPoint)
    {
        this.checkPoint = checkPoint;
    }
}
