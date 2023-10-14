using UnityEngine;
using System.Collections;

public class PlayerReturn : MonoBehaviour
{
    private PlayerState playerState;
    private PlayerMovement playerMovement;
    public CameraState cameraState;
    public Vector3 checkPoint = Vector3.zero;
    public float frontMinY;
    public float upMinY;
    public float dropY = 20f;
    public DirectionManager directionManager;
    //private KeyAndDoor keyAndDoor;

	void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerMovement = GetComponent<PlayerMovement>();
        //keyAndDoor = GetComponent<KeyAndDoor>();
        SetCheckPoint(transform.position);
    }

    void Update()
    {
        if (!playerState.GetPositionUpdating())
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front || cameraState.GetIsRotating())
            {
                if (transform.position.y < frontMinY || transform.position.y >= upMinY)
                {
                    GetComponent<CharacterController>().enabled = false;
                    //keyAndDoor.KeyDrop();
                    transform.position = checkPoint;
                    directionManager.UpdateInvisibleCubes();
                    playerState.SetUpIsDropping(false);
                    GetComponent<CharacterController>().enabled = true;
                }
            }
            else if(cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating())
            {
                if (transform.position.y < upMinY)
                {
                    Debug.Log(transform.position);
                    GetComponent<CharacterController>().enabled = false;
                    //keyAndDoor.KeyDrop();
                    transform.position = checkPoint;
                    directionManager.UpdateInvisibleCubes();
                    directionManager.MovePlayerToClosestInvisibleCube();
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
        upMinY = minY+2f;
    }

    public void SetFrontMinY(float minY)
    {
        frontMinY = minY - dropY;
    }

    public float GetDropY()
    {
        return dropY;
    }
}
