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
    private List<Quaternion> rotations;
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
        rotations = new List<Quaternion>();
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
                            Vector3 axis = RotationVector3(platform.rotation.eulerAngles, FacingDirection.Front);
                            rotations.Add(platform.rotation * Quaternion.AngleAxis(90f, axis));
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
                            Vector3 axis = RotationVector3(platform.rotation.eulerAngles, FacingDirection.Up);
                            rotations.Add(platform.rotation * Quaternion.AngleAxis(90f, axis));
                        }
                        isRotating = true;
                        platformConsoleMananger.SetPlatformIsRotating(true);
                    }
                }
            }
        }
        if (isRotating)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].rotation = Quaternion.RotateTowards(platforms[i].rotation, rotations[i], 90 / rotationTime * Time.deltaTime);
                if (Quaternion.Angle(platforms[i].rotation, rotations[i]) < 1.0f)
                {
                    isRotating = false;
                }
            }
            if (!isRotating)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    platforms[i].rotation = rotations[i];
                }
                directionManager.UpdateInvisibleCubes();
                platformConsoleMananger.SetPlatformIsRotating(false);
            }
        }
    }

    private Vector3 RotationVector3(Vector3 rotation, FacingDirection facingDirection)
    {
        float x = (rotation.x + 360f) % 360f;
        float y = (rotation.y + 360f) % 360f;
        float z = (rotation.z + 360f) % 360f;
        if (facingDirection == FacingDirection.Front)
        {
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f))
            {
                return new Vector3(1f, 0f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f))
            {
                return new Vector3(-1f, 0f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f))
            {
                return new Vector3(0f, 1f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f))
            {
                return new Vector3(0f, -1f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f))
            {
                return new Vector3(0f, 0f, 1f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f))
            {
                return new Vector3(0f, 0f, -1f);
            }
        }
        else if(facingDirection == FacingDirection.Up)
        {
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 270) < 0.1f))
            {
                return new Vector3(1f, 0f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 90) < 0.1f))
            {
                return new Vector3(-1f, 0f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 180) < 0.1f))
            {
                return new Vector3(0f, 1f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 0) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 180) < 0.1f && Mathf.Abs(z - 0) < 0.1f))
            {
                return new Vector3(0f, -1f, 0f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 270) < 0.1f && Mathf.Abs(z - 270) < 0.1f))
            {
                return new Vector3(0f, 0f, 1f);
            }
            if ((Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 0) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 90) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 180) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 0) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 90) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 180) < 0.1f) ||
                (Mathf.Abs(y - 270) < 0.1f && Mathf.Abs(x - 90) < 0.1f && Mathf.Abs(z - 270) < 0.1f))
            {
                return new Vector3(0f, 0f, -1f);
            }
        }
        return Vector3.zero;
    }
}
