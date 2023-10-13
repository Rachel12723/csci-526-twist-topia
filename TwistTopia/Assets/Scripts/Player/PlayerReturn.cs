using UnityEngine;
using System.Collections;

public class PlayerReturn : MonoBehaviour
{

    public GameObject player;
    private PlayerMovement playerMovement;
    public Vector3 checkPoint = Vector3.zero;
    public FacingDirection facingDirection;
    public float frontMinY;
    public float upMinY;
    public float dropY = 20f;
    public GameObject directionManager;

	void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        facingDirection = playerMovement.GetFacingDirection();
    }

    void LateUpdate()
    {
        if (facingDirection == FacingDirection.Front)
        {
            if (player.transform.position.y < frontMinY || player.transform.position.y >= upMinY)
            {
                player.GetComponent<CharacterController>().enabled = false;
                playerMovement.keyDrop();
                player.transform.position = checkPoint;
                directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
                playerMovement.dropUp = false;
                player.GetComponent<CharacterController>().enabled = true;
            }
        }
        else if(facingDirection == FacingDirection.Up)
        {
            if (player.transform.position.y < upMinY)
            {
                player.GetComponent<CharacterController>().enabled = false;
                playerMovement.keyDrop();
                player.transform.position = checkPoint;
                directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
                directionManager.GetComponent<DirectionManager>().MovePlayerToClosestInvisibleCube();
                playerMovement.dropUp = false;
                player.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    public void SetCheckPoint(Vector3 checkPoint)
    {
        this.checkPoint = checkPoint;
    }

    public void SetUpMinY(float minY)
    {
        upMinY = minY - dropY;
    }

    public void SetFrontMinY(float minY)
    {
        frontMinY = minY - dropY;
    }

    public void SetFacingDirection(FacingDirection facingDirection)
    {
        this.facingDirection = facingDirection;
    }
}
