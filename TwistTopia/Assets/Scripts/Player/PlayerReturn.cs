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
    private KeyAndDoor keyAndDoor;
    public int dropCount;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerMovement = GetComponent<PlayerMovement>();
        keyAndDoor = GetComponent<KeyAndDoor>();
        SetCheckPoint(transform.position);
        dropCount = 0;
    }

    void Update()
    {
        if (!playerState.GetPositionUpdating() && !cameraState.GetIsUsing3DView())
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front || cameraState.GetIsRotating())
            {
                if (transform.position.y < frontMinY || transform.position.y >= upMinY)
                {
                    ResetPlayer(FacingDirection.Front);
                }
            }
            else if(cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating())
            {
                if (transform.position.y < upMinY)
                {
                    ResetPlayer(FacingDirection.Up);
                }
            }
        }
    }

    public void ResetPlayer(FacingDirection facingDirection)
    {
        if (facingDirection == FacingDirection.Front)
        {
            //cameraState.SetIsRebinding(true);
            GetComponent<CharacterController>().enabled = false;
            keyAndDoor.KeyDrop();
            transform.position = checkPoint;
            directionManager.UpdateInvisibleCubes();
            playerState.SetUpIsDropping(false);
            GetComponent<CharacterController>().enabled = true;
            dropCount++;
        }
        else if(facingDirection == FacingDirection.Up)
        {
            //cameraState.SetFacingDirection(FacingDirection.Front);
            //cameraState.SetIsRebinding(true);
            GetComponent<CharacterController>().enabled = false;
            keyAndDoor.KeyDrop();
            transform.position = checkPoint;
            directionManager.UpdateInvisibleCubes();
            directionManager.MovePlayerToClosestInvisibleCube();
            playerState.SetUpIsDropping(false);
            GetComponent<CharacterController>().enabled = true;
            dropCount++;
        }
    }

    public void SetCheckPoint(Vector3 checkPoint)
    {
        this.checkPoint = checkPoint;
    }

    public void SetUpMinY(float minY)
    {
        upMinY = minY;
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
