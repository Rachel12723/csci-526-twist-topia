using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Left: -1  Right: 1
    private int Horizontal = 0;
    // Down: -1  Up: 1
    private int Vertical = 0;

    // Physical Parameter
    public float movementSpeed = 5f;
    public float gravity = 8f;
    public CharacterController characterController;

    // Camera and Light Rotation
    private FacingDirection facingDirection;
    private float degree = 0;


    void Start()
    {

    }

    void Update()
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

        // Movement
        Vector3 trans = Vector3.zero;
        if (facingDirection == FacingDirection.Front)
        {
            trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, 0f);
        }
        else if (facingDirection == FacingDirection.Up)
        {
            trans = new Vector3(Horizontal * movementSpeed * Time.deltaTime, -gravity * Time.deltaTime, Vertical * movementSpeed * Time.deltaTime);
        }
        characterController.Move(trans);

        // Camera and Light Rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(degree, 0, 0), 8 * Time.deltaTime);
    }

    // Update the Facing Firection
    public void UpdateFacingDirection(FacingDirection newDirection, float angle)
    {
        facingDirection = newDirection;
        degree = angle;
    }
}
