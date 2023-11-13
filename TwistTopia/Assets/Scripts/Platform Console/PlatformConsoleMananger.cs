using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformConsoleMananger : MonoBehaviour
{
    public Transform player;
    public DirectionManager directionManager;
    public InputManager inputManager;
    public KeyCode platformRotationKeyCode;
    public CameraState cameraState;
    public bool platformIsRotating = false;
    public float rotationTime = 0.5f;

    public bool GetPlatformIsRotating()
    {
        return platformIsRotating;
    }

    public void SetPlatformIsRotating(bool platformIsRotating)
    {
        this.platformIsRotating = platformIsRotating;
    }
}
