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
    private List<Vector3> rotations;
    private bool isRotating = false;

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
        rotations = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlatform();
    }

    private void RotatePlatform()
    {
        int state = PlayerPrefs.GetInt("state");
        if (Input.GetKeyDown(platformRotationKeyCode) && state == 0)
        {
            if (inputManager.GetAllowInteraction())
            {
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    if (Mathf.Abs(transform.position.x - player.position.x) < 0.5f
                        && Mathf.Abs(transform.position.y - player.position.y) < 1.2f)
                    {
                        rotations.Clear();
                        foreach (Transform platform in platforms)
                        {
                            if (platform.rotation.eulerAngles.y == 0f)
                            {
                                rotations.Add(new Vector3(platform.rotation.eulerAngles.x, 0f, (platform.rotation.eulerAngles.z + 270f) % 360f));
                            }
                            else if (platform.rotation.eulerAngles.y == 90f)
                            {
                                rotations.Add(new Vector3((platform.rotation.eulerAngles.x+90f)%360f, 90f, platform.rotation.eulerAngles.z));
                            }
                            else if (platform.rotation.eulerAngles.y == 180f)
                            {
                                rotations.Add(new Vector3(platform.rotation.eulerAngles.x, 180f, (platform.rotation.eulerAngles.z + 90f) % 360f));
                            }
                            else if (platform.rotation.eulerAngles.y == 270f)
                            {
                                rotations.Add(new Vector3((platform.rotation.eulerAngles.x + 270f) % 360f, 270f, platform.rotation.eulerAngles.z));
                            }
                        }
                        isRotating = true;
                        platformConsoleMananger.SetPlatformIsRotating(true);
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    if (Mathf.Abs(transform.position.x - player.position.x) < 1.2f
                        && Mathf.Abs(transform.position.z - player.position.z) < 1.2f)
                    {
                        rotations.Clear();
                        foreach (Transform platform in platforms)
                        {
                            rotations.Add(new Vector3(platform.rotation.eulerAngles.x, (platform.rotation.eulerAngles.y + 90f) % 360f, platform.rotation.eulerAngles.z));
                        }
                        isRotating = true;
                        platformConsoleMananger.SetPlatformIsRotating(true);
                    }
                }
            }
        }
        if (isRotating)
        {
            for(int i = 0;i<platforms.Count;i++)
            {
                platforms[i].rotation = Quaternion.RotateTowards(platforms[i].rotation, Quaternion.Euler(rotations[i]), 90 / rotationTime * Time.deltaTime);
                if (Quaternion.Angle(platforms[i].rotation, Quaternion.Euler(rotations[i])) < 1.0f)
                {
                    isRotating = false;
                }
            }
            if (!isRotating)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    platforms[i].rotation = Quaternion.Euler(rotations[i]);
                }
                directionManager.UpdateInvisibleCubes();
                platformConsoleMananger.SetPlatformIsRotating(false);
            }
        }
    }
}
