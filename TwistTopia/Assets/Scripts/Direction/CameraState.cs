using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : MonoBehaviour
{
    public FacingDirection facingDirection = FacingDirection.Front;
    public bool isRotating = false;

    public void SetIsRotating(bool isRotating)
    {
        this.isRotating = isRotating;
    }

    public bool GetIsRotating()
    {
        return isRotating;
    }

    public void SetFacingDirection(FacingDirection facingDirection)
    {
        this.facingDirection = facingDirection;
    }

    public FacingDirection GetFacingDirection()
    {
        return facingDirection;
    }
}
