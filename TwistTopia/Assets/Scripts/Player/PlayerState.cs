using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool upIsDropping = false;
    public bool positionUpdating = false;

    public void SetUpIsDropping(bool upIsDropping)
    {
        this.upIsDropping = upIsDropping;
    }

    public void SetPositionUpdating(bool positionUpdating)
    {
        this.positionUpdating = positionUpdating;
    }

    public bool GetUpIsDropping()
    {
        return upIsDropping;
    }

    public bool GetPositionUpdating()
    {
        return positionUpdating;
    }
}
