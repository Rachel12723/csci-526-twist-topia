using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOnPortal : MonoBehaviour
{   
    // playerMovement Script
    public GameObject player;
    private PlayerMovement playerMovement;
    
    // portal interaction keyCode
    public KeyCode usePortalCode;

    // facing direction
    public CameraState cameraState;
    private FacingDirection facingDirection;
    
    // portal transform
    public Transform portal1;
    public Transform portal2;
    
    // World Unit
    private float WorldUnit = 1.000f;
    public float xTolerance = 0.5f;
    public float yTolerance = 0.2f;
    public float zTolerance = 0.5f;

    // Direction Manager
    public DirectionManager directionManager;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {   
        // get the direction first
        facingDirection = cameraState.GetFacingDirection();
        PortalAction();
        /*
        if (!canStep1 && checkStepAwayPortal(portal1))
            canStep1 = true;
        else if (!canStep2 && checkStepAwayPortal(portal2))
            canStep2 = true;
            */
    }
    
    // according to the direction and closet distance decide how to portal the player
    private void PortalAction()
    {
        if (facingDirection == FacingDirection.Front)
        {
            float distanceX1 = player.transform.position.x - portal1.transform.position.x;
            float distanceY1 = player.transform.position.y - portal1.transform.position.y;
            float distance1 = Mathf.Sqrt(distanceX1 * distanceX1 + distanceY1 * distanceY1);
            
            float distanceX2 = player.transform.position.x - portal2.transform.position.x;
            float distanceY2 = player.transform.position.y - portal2.transform.position.y;
            float distance2 = Mathf.Sqrt(distanceX2 * distanceX2 + distanceY2 * distanceY2);
            
            if (distance1 < distance2)
            {
                if (CheckStepOnPortal(portal1) && Input.GetKeyDown(usePortalCode))
                //if (checkStepOnPortal(portal1) && canStep1 == true && Input.GetKeyDown(usePortalCode))
                {
                    PortalPlayer(portal2);
                }
            }
            else if (distance2 < distance1)
            {
                if (CheckStepOnPortal(portal2) && Input.GetKeyDown(usePortalCode))
                //if (checkStepOnPortal(portal2) && canStep2 == true && Input.GetKeyDown(usePortalCode))
                {
                    PortalPlayer(portal1);
                }
            }
        }
        
        else if (facingDirection == FacingDirection.Up)
        {
            float distanceX1 = player.transform.position.x - portal1.transform.position.x;
            float distanceY1 = player.transform.position.z - portal1.transform.position.z;
            float distance1 = Mathf.Sqrt(distanceX1 * distanceX1 + distanceY1 * distanceY1);
            
            float distanceX2 = player.transform.position.x - portal2.transform.position.x;
            float distanceY2 = player.transform.position.z - portal2.transform.position.z;
            float distance2 = Mathf.Sqrt(distanceX2 * distanceX2 + distanceY2 * distanceY2);
            
            if (distance1 < distance2)
            {
                if (CheckStepOnPortal(portal1) && Input.GetKeyDown(usePortalCode))
                //if (checkStepOnPortal(portal1) && canStep1 == true && Input.GetKeyDown(usePortalCode))
                {
                    PortalPlayer(portal2);
                }
            }
            else if (distance2 < distance1)
            {
                if (CheckStepOnPortal(portal2) && Input.GetKeyDown(usePortalCode))
                //if (checkStepOnPortal(portal2) && canStep2 == true && Input.GetKeyDown(usePortalCode))
                {
                    PortalPlayer(portal1);
                }
            }
        }
        
        
    }
    
    // check player is stepping on the portal
    private bool CheckStepOnPortal(Transform portal)
    {
        if (facingDirection == FacingDirection.Front)
        {
            if (Mathf.Abs(player.transform.position.x - portal.transform.position.x) < xTolerance &&
                Mathf.Abs(player.transform.position.y - portal.transform.position.y) < WorldUnit + yTolerance)
            {   
                //Debug.Log("StepOn!");
                return true;
            }
        }
        else if(facingDirection == FacingDirection.Up)
        {
            if (Mathf.Abs(player.transform.position.x - portal.transform.position.x) < xTolerance &&
                Mathf.Abs(player.transform.position.z - portal.transform.position.z) < zTolerance)
            {
                //Debug.Log("StepOn!");
                return true;
            }
        }

        return false;
    }
    /*
    // check player leaves the portal after just portaling
    private bool checkStepAwayPortal(Transform portal)
    {
        if (facingDirection == FacingDirection.Front)
        {
            if (Mathf.Abs(player.transform.position.x - portal.transform.position.x) > WorldUnit ||
                Mathf.Abs(player.transform.position.y - portal.transform.position.y) > WorldUnit + 0.5f)
            {
                //Debug.Log("StepAway!");
                return true;
            }
        }
        if (facingDirection == FacingDirection.Up)
        {
            if (Mathf.Abs(player.transform.position.x - portal.transform.position.x) > WorldUnit ||
                Mathf.Abs(player.transform.position.z - portal.transform.position.z) > WorldUnit)
            {
                //Debug.Log("StepAway!");
                return true;
            }
        }

        return false;
    }
    */
    // according different direction to portal the player
    private void PortalPlayer(Transform portal)
    {
        if (facingDirection == FacingDirection.Front)
        {
            /*
            if (portal.transform.position == portal1.transform.position)
                canStep1 = false;
            else if (portal.transform.position == portal2.transform.position)
                canStep2 = false;
                */
            player.GetComponent<CharacterController>().enabled = false;
            cameraState.SetIsRebinding(true);
            player.transform.position = new Vector3(portal.transform.position.x, portal.transform.position.y + 1, portal.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
            directionManager.UpdateInvisibleCubes();
        }

        if (facingDirection == FacingDirection.Up)
        {
            /*
            if (portal.transform.position == portal1.transform.position)
                canStep1 = false;
            else if (portal.transform.position == portal2.transform.position)
                canStep2 = false;
                */
            player.GetComponent<CharacterController>().enabled = false;
            cameraState.SetIsRebinding(true);
            float newY = player.transform.position.y;
            player.transform.position = new Vector3(portal.transform.position.x, player.transform.position.y, portal.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
