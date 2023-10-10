using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    // Rotate
    public KeyCode rotateKeyCode;
    private FacingDirection facingDirection;

    // Player
    public GameObject player;
    private PlayerMovement playerMovement;

    // Invisible Cube
    public GameObject invisibleCube;
    private List<Transform> invisibleList = new List<Transform>();

    // World Unit
    public float WorldUnit = 1.000f;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(OnInvisibleCube());
        if (Input.GetKeyDown(rotateKeyCode))
        {
            if (facingDirection == FacingDirection.Front)
            {
                facingDirection = FacingDirection.Up;
                playerMovement.UpdateFacingDirection(facingDirection, 90f);
            }
            else if (facingDirection == FacingDirection.Up)
            {
                facingDirection = FacingDirection.Front;
                playerMovement.UpdateFacingDirection(facingDirection, 0f);
            }
        }
    }

    // Determines if the player is on an invisible cube
    private bool OnInvisibleCube()
    {
        foreach (Transform item in invisibleList)
        {
            if (Mathf.Abs(item.position.x - player.transform.position.x) < WorldUnit
                && Mathf.Abs(item.position.z - player.transform.position.z) < WorldUnit
                && player.transform.position.y - item.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - item.position.y > 0)
                return true;
        }
        return false;
    }
}
