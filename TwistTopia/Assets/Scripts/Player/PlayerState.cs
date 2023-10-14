using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public FacingDirection facingDirection = FacingDirection.Front;
    public bool isRotating = false;
    public bool upIsDropping = false;

    public void SetFacingDirection(FacingDirection facingDirection)
    {
        this.facingDirection = facingDirection;
    }

    public void SetIsRotating(bool isRotating)
    {
        this.isRotating = isRotating;
    }

    public void SetUpIsDropping(bool upIsDropping)
    {
        this.upIsDropping = upIsDropping;
    }

    public FacingDirection GetFacingDirection()
    {
        return facingDirection;
    }

    public bool GetIsRotating()
    {
        return isRotating;
    }

    public bool GetUpIsDropping()
    {
        return upIsDropping;
    }
}
