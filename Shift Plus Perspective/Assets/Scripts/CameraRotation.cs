using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target; // player
    public PlayerMovement playerMovementScript; // Reference to the player's movement script
    public Collider[] platformColliders; // Colliders for all platforms

    private Quaternion topDownRotation = Quaternion.Euler(90, 5, 365);
    private Vector3 topDownPosition = new Vector3(0, 20, 0);

    private Quaternion sideViewRotation = Quaternion.Euler(0, 0, 0);
    private Vector3 sideViewPosition = new Vector3(0, 0, -20);

    private bool isTopDownView = true;

    private void Start()
    {
        transform.rotation = topDownRotation;
        transform.position = topDownPosition;
        UpdatePlatformVisibility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleView();
        }
    }

    void ToggleView()
    {
        if (isTopDownView)
        {
            transform.rotation = sideViewRotation;
            transform.position = sideViewPosition;
        }
        else
        {
            transform.rotation = topDownRotation;
            transform.position = topDownPosition;
        }
        isTopDownView = !isTopDownView;

        UpdatePlatformVisibility();
    }

    bool IsObjectVisible(Camera cam, Collider col)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, col.bounds);
    }

    void UpdatePlatformVisibility()
    {
        bool isOnVisiblePlatform = false;

        // Check all platform colliders
        foreach (Collider col in platformColliders)
        {
            if (IsObjectVisible(this.GetComponent<Camera>(), col))
            {
                isOnVisiblePlatform = true;
                break;
            }
        }

        playerMovementScript.enabled = isOnVisiblePlatform;
    }
}

