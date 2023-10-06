using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAlignmentCheck : MonoBehaviour
{
    public Transform platform1;
    public Transform platform2;
    public Camera mainCamera;
    
    public bool ArePlatformsAligned()
    {
        Vector3 screenPos1 = mainCamera.WorldToScreenPoint(platform1.position);
        Vector3 screenPos2 = mainCamera.WorldToScreenPoint(platform2.position);

        return Mathf.Abs(screenPos1.y - screenPos2.y) < 1; // Check if the y positions are nearly the same in screen space
    }
}
