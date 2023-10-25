using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3DView : MonoBehaviour
{

    public Transform map;
    private CameraState cameraState;
    private Camera camera;
    public Transform player;
    public float settingTime;
    private Vector3 targetPosition;
    private Quaternion targetRotation= Quaternion.Euler(45, 45, 0);
    public float targetField;
    //private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalField;
    //private float moveSpeed;
    private float rotateSpeed;
    private float fieldSpeed;
    public DirectionManager directionManager;

    // Start is called before the first frame update
    void Start()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float minZ = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;
        Transform platformCubes = map.Find("Platform Cubes");
        Transform blockCubes = map.Find("Block Cubes");
        foreach (Transform platform in platformCubes)
        {
            foreach (Transform cube in platform)
            {
                if (cube.position.x < minX)
                {
                    minX = cube.position.x;
                }
                if (cube.position.y < minY)
                {
                    minY = cube.position.y;
                }
                if (cube.position.z < minZ)
                {
                    minZ = cube.position.z;
                }
                if (cube.position.x > maxX)
                {
                    maxX = cube.position.x;
                }
                if (cube.position.y > maxY)
                {
                    maxY = cube.position.y;
                }
                if (cube.position.z > maxZ)
                {
                    maxZ = cube.position.z;
                }
            }
        }
        foreach (Transform block in blockCubes)
        {
            foreach (Transform cube in block)
            {
                if (cube.position.x < minX)
                {
                    minX = cube.position.x;
                }
                if (cube.position.y < minY)
                {
                    minY = cube.position.y;
                }
                if (cube.position.z < minZ)
                {
                    minZ = cube.position.z;
                }
                if (cube.position.x > maxX)
                {
                    maxX = cube.position.x;
                }
                if (cube.position.y > maxY)
                {
                    maxY = cube.position.y;
                }
                if (cube.position.z > maxZ)
                {
                    maxZ = cube.position.z;
                }
            }
        }
        targetPosition = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
        cameraState = GetComponent<CameraState>();
        camera = transform.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraState.GetIsRotating())
        {
            if (Input.GetKey(KeyCode.M))
            {
                if (!cameraState.GetIsUsing3DView())
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Up)
                    {
                        directionManager.MovePlayerToClosestPlatformCube();
                    }
                    //player.GetComponent<MeshRenderer>().enabled = false;
                    //originalPosition = transform.position;
                    originalRotation = transform.rotation;
                    originalField = camera.fieldOfView;
                    //moveSpeed = Vector3.Distance(transform.position, targetPosition) * (1 / settingTime);
                    rotateSpeed = Quaternion.Angle(transform.rotation, targetRotation) * (1 / settingTime);
                    fieldSpeed = (targetField - originalField) * (1 / settingTime);
                }
                cameraState.SetIsUsing3DView(true);
                camera.orthographic = false;
                //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                if (camera.fieldOfView < targetField)
                {
                    camera.fieldOfView += fieldSpeed * Time.deltaTime;
                }
            }
            else if (cameraState.GetIsUsing3DView())
            {
                //transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, rotateSpeed * Time.deltaTime);
                camera.fieldOfView -= fieldSpeed * Time.deltaTime;
                if (transform.rotation == originalRotation && camera.fieldOfView <= originalField)
                {
                    camera.orthographic = true;
                    if (cameraState.GetFacingDirection() == FacingDirection.Up)
                    {
                        directionManager.MovePlayerToClosestInvisibleCube();
                    }
                    //player.GetComponent<MeshRenderer>().enabled = true;
                    cameraState.SetIsUsing3DView(false);
                }
            }
        }
    }
}
