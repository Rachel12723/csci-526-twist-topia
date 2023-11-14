using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotation : MonoBehaviour
{
    private PlatformConsoleMananger platformConsoleMananger;
    private Transform player;
    private DirectionManager directionManager;
    private InputManager inputManager;
    private KeyCode platformRotationKeyCode;
    private CameraState cameraState;
    private float rotationTime;
    public float rotationX = 0f;
    public float rotationY = 0f;
    public float rotationZ = 0f;
    public List<Transform> platforms;

    // Start is called before the first frame update
    void Start()
    {
        platformConsoleMananger = transform.parent.gameObject.GetComponent<PlatformConsoleMananger>();
        player = platformConsoleMananger.player;
        directionManager = platformConsoleMananger.directionManager;
        inputManager = platformConsoleMananger.inputManager;
        platformRotationKeyCode = platformConsoleMananger.platformRotationKeyCode;
        cameraState = platformConsoleMananger.cameraState;
        rotationTime = platformConsoleMananger.rotationTime;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlatform();
    }

    private void RotatePlatform()
    {
        if (Input.GetKeyDown(platformRotationKeyCode))
        {
            if (inputManager.GetAllowInteraction())
            {
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    if (Mathf.Abs(transform.position.x - player.position.x) < 0.5f
                        && Mathf.Abs(transform.position.y - player.position.y) < 1.2f)
                    {
                        if (rotationY == 0f)
                        {
                            rotationZ -= 90f;
                            rotationZ %= 360f;
                        }
                        else if(rotationY == 90f)
                        {
                            rotationX += 90f;
                            rotationX %= 360f;
                        }
                        else if (rotationY == 180f)
                        {
                            rotationZ += 90f;
                            rotationZ %= 360f;
                        }
                        else if (rotationY == 270f)
                        {
                            rotationX -= 90f;
                            rotationX %= 360f;
                        }
                        //rotationX += 90f;
                        //rotationX %= 360f;
                        platformConsoleMananger.SetPlatformIsRotating(true);
                        //foreach (Transform platform in platforms)
                        //{
                        //    platform.rotation = Quaternion.Euler(platform.rotation.eulerAngles.x, platform.rotation.eulerAngles.y, platform.rotation.eulerAngles.z + 90);
                        //}
                        //directionManager.UpdateInvisibleCubes();
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    if (Mathf.Abs(transform.position.x - player.position.x) < 1.2f
                        && Mathf.Abs(transform.position.z - player.position.z) < 1.2f)
                    {
                        rotationY += 90f;
                        rotationY %= 360f;
                        platformConsoleMananger.SetPlatformIsRotating(true);
                        //foreach (Transform platform in platforms)
                        //{
                        //    platform.rotation = Quaternion.Euler(platform.rotation.eulerAngles.x, platform.rotation.eulerAngles.y + 90, platform.rotation.eulerAngles.z);
                        //}
                        //directionManager.UpdateInvisibleCubes();
                    }
                }
            }
        }
        if (platformConsoleMananger.GetPlatformIsRotating())
        {
            foreach (Transform platform in platforms)
            {
                platform.rotation = Quaternion.RotateTowards(platform.rotation, Quaternion.Euler(rotationX, rotationY, rotationZ), 90 / rotationTime * Time.deltaTime);
                if (Quaternion.Angle(platform.rotation, Quaternion.Euler(rotationX, rotationY, rotationZ))<1.0f)
                {
                    platformConsoleMananger.SetPlatformIsRotating(false);
                }
            }
            if (!platformConsoleMananger.GetPlatformIsRotating())
            {
                foreach (Transform platform in platforms)
                {
                    platform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
                }
                directionManager.UpdateInvisibleCubes();
            }
        }
    }
}
