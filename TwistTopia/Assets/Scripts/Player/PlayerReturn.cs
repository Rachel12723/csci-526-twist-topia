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
    public FadingInfo deathInfo;
    public FrameAction frameAction;
    public PlayerFrame playerFrame;
    public Transform checkpoints;
    public Material lightMaterial;

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
                    ResetPlayer();
                }
            }
            else if(cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating())
            {
                if (transform.position.y < upMinY)
                {
                    ResetPlayer();
                }
            }
        }

        UpdateCheckPoint();
    }

    public void ResetPlayer()
    {
        
        if(cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            cameraState.SetFacingDirection(FacingDirection.Front);
        }
		cameraState.SetIsRebinding(true);
        GetComponent<CharacterController>().enabled = false;
        //keyAndDoor.KeyDrop();
        transform.position = checkPoint;
        directionManager.UpdateInvisibleCubes();
        //directionManager.MovePlayerToClosestInvisibleCube();
        playerState.SetUpIsDropping(false);
        GetComponent<CharacterController>().enabled = true;
        dropCount++;
        deathInfo.SetIsShowed(true);
        if (frameAction != null && playerFrame!=null)
        {
            playerFrame.ResetFrame();
            frameAction.ReleaseEnemy(false);
        }

    }

    private void UpdateCheckPoint()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            if (checkpoints != null)
            {
                foreach (Transform checkpoint in checkpoints)
                {
                    if (Mathf.Abs(transform.position.x - checkpoint.position.x) < 0.5f &&
                        Mathf.Abs(transform.position.y - checkpoint.position.y) < 0.5f)
                    {
                        SetCheckPoint(new Vector3(checkpoint.position.x, checkpoint.position.y, transform.position.z));
                        checkpoint.Find("Light").gameObject.GetComponent<Renderer>().material = lightMaterial;
                        break;
                    }
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            if (checkpoints != null)
            {
                foreach (Transform checkpoint in checkpoints)
                {
                    if (Mathf.Abs(transform.position.x - checkpoint.position.x) < 1.0f &&
                        Mathf.Abs(transform.position.z - checkpoint.position.z) < 1.0f)
                    {
                        SetCheckPoint(new Vector3(checkpoint.position.x, checkpoint.position.y, checkpoint.position.z));
                        checkpoint.Find("Light").gameObject.GetComponent<Renderer>().material = lightMaterial;
                        break;
                    }
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
