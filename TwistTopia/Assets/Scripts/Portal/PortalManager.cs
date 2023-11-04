using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    // playerMovement Script
    public GameObject player;

    // portal interaction keyCode
    public KeyCode usePortalCode;

    // facing direction
    public CameraState cameraState;

    // World Unit
    public float xTolerance = 0.5f;
    public float yTolerance = 0.2f;
    public float zTolerance = 0.5f;

    // Direction Manager
    public DirectionManager directionManager;

    // Input Manager
    public InputManager inputManager;
}
