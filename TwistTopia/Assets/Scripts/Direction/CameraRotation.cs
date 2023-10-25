using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Camera and Light Rotation
    public float rotationSpeed = 8;
    private CameraState cameraState;
    private float degree = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraState = GetComponent<CameraState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraState.GetIsUsing3DView())
        {
            RotateTo(cameraState.GetFacingDirection());
            // Camera and Light Rotation
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(degree, 0, 0), rotationSpeed * Time.deltaTime);
            transform.rotation = rotation;
            float angle = rotation.eulerAngles.x;
            if (Mathf.Abs(degree - angle)< 0.01)
            {
                cameraState.SetIsRotating(false);
            }
        }
    }

    // Update the Facing Firection
    public void RotateTo(FacingDirection facingDirection)
    {
        if (facingDirection == FacingDirection.Front)
        {
            degree = 0f;
        }
        else if (facingDirection == FacingDirection.Up)
        {
            degree = 90f;
        }
    }
}
