using UnityEngine;
using System.Collections;

public class PlayerReturn : MonoBehaviour
{
    private PlayerState playerState;
    private PlayerMovement playerMovement;
    public Vector3 checkPoint = Vector3.zero;
    public float frontMinY;
    public float upMinY;
    public float dropY = 20f;
    public GameObject directionManager;

	void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (directionManager.GetComponent<DirectionManager>().positionUpdated)
        {
            if (playerState.GetFacingDirection() == FacingDirection.Front || playerState.GetIsRotating())
            {
                if (transform.position.y < frontMinY || transform.position.y >= upMinY)
                {
                    GetComponent<CharacterController>().enabled = false;
                    playerMovement.keyDrop();
                    transform.position = checkPoint;
                    directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
                    playerState.SetUpIsDropping(false);
                    GetComponent<CharacterController>().enabled = true;
                }
            }
            else if(playerState.GetFacingDirection() == FacingDirection.Up && !playerState.GetIsRotating())
            {
                if (transform.position.y < upMinY)
                {
                    GetComponent<CharacterController>().enabled = false;
                    playerMovement.keyDrop();
                    transform.position = checkPoint;
                    directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
                    directionManager.GetComponent<DirectionManager>().MovePlayerToClosestInvisibleCube();
                    playerState.SetUpIsDropping(false);
                    GetComponent<CharacterController>().enabled = true;
                }
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
}
