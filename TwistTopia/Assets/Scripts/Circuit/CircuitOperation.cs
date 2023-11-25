using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitOperation : MonoBehaviour
{
    private CircuitManager circuitManager;
    private Transform player;
    private CameraState cameraState;
    // button for rotation
    private KeyCode rotateCircuitCode;
    public CircuitType type;
    // list for rotatable circuits
    public List<Transform> rotatableCircuits;

    public List<Transform> platforms;
    // degrees when circuit rotates
    private float rotateDegrees = 90.0f;
    public bool circuitIsRotating = false;
    private float rotationTime;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private float rotationZ = 0f;
    private Transform circuitCube = null;
    // record if circuit is completed
    private bool circuitCompleted = false;

    private DirectionManager directionManager;
    private InputManager inputManager;
    private FadingInfo circuitInfo;


    // Start is called before the first frame update
    void Start()
    {
        circuitManager = transform.parent.gameObject.GetComponent<CircuitManager>();
        player = circuitManager.player;
        cameraState = circuitManager.cameraState;
        rotationTime = circuitManager.rotationTime;
        directionManager = circuitManager.directionManager;
        inputManager = circuitManager.inputManager;
        rotateCircuitCode = circuitManager.rotateCircuitCode;
        circuitInfo = circuitManager.circuitInfo;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCircuit();
        checkCircuitCompleted();
    }
    // rotate the rotatable circuits if player stands nearby and pushes button
    private void RotateCircuit()
    {
        int state = PlayerPrefs.GetInt("state");
        if (state == 0 && Input.GetKeyDown(rotateCircuitCode))
        {
            if (inputManager.GetAllowInteraction() && !circuitIsRotating)
            {
                if (type == CircuitType.Up)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Front)
                    {
                        foreach (Transform rotatableCircuit in rotatableCircuits)
                        {
                            if (Mathf.Abs(player.transform.position.x - rotatableCircuit.position.x) < 0.5f &&
                                Mathf.Abs(player.transform.position.y - rotatableCircuit.position.y) < 1.2f)
                            {
                                //rotatableCircuit.Rotate(Vector3.up, rotateDegrees);
                                circuitCube = rotatableCircuit;
                                rotationX = circuitCube.rotation.eulerAngles.x;
                                rotationY = circuitCube.rotation.eulerAngles.y + rotateDegrees;
                                rotationZ = circuitCube.rotation.eulerAngles.z;
                                circuitIsRotating = true;
                                break;
                            }
                        }
                    }
                }

                if (type == CircuitType.Front)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Up)
                    {
                        foreach (Transform rotatableCircuit in rotatableCircuits)
                        {
                            if (Mathf.Abs(player.transform.position.x - rotatableCircuit.position.x) < 1.2f &&
                                Mathf.Abs(player.transform.position.z - rotatableCircuit.position.z) < 1.2f)
                            {
                                //rotatableCircuit.Rotate(Vector3.forward, rotateDegrees);
                                circuitCube = rotatableCircuit;
                                rotationX = circuitCube.rotation.eulerAngles.x;
                                rotationY = circuitCube.rotation.eulerAngles.y;
                                rotationZ = circuitCube.rotation.eulerAngles.z + rotateDegrees;
                                circuitIsRotating = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (circuitIsRotating && circuitCube)
        {
            circuitCube.rotation = Quaternion.RotateTowards(circuitCube.rotation, Quaternion.Euler(rotationX, rotationY, rotationZ), rotateDegrees / rotationTime * Time.deltaTime);
            if (Quaternion.Angle(circuitCube.rotation, Quaternion.Euler(rotationX, rotationY, rotationZ)) < 1.0f)
            {
                circuitCube.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
                circuitIsRotating = false;
                circuitCube = null;
            }
        }
    }
    // check if circuit is completed in specific view
    private void checkCircuitCompleted()
    {
        if (type == CircuitType.Up)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Up)
            {
                bool judgeZero = true;
                foreach (Transform rotatableCircuit in rotatableCircuits)
                {
                    Vector3 eulerAngles = rotatableCircuit.rotation.eulerAngles;
                    float yRotation = eulerAngles.y;
                    Debug.Log(yRotation);
                    if (Mathf.Abs(yRotation) >= 0.01f)
                    {
                        judgeZero = false;
                        break;
                    }
                }

                if (judgeZero && !circuitCompleted && !cameraState.GetIsRotating())
                {
                    circuitCompleted = true;
                    Debug.Log("Circuit Completed!!");
                    showPlatform();
                }
            }
        }
        if (type == CircuitType.Front)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                bool judgeZero = true;
                foreach (Transform rotatableCircuit in rotatableCircuits)
                {
                    Vector3 eulerAngles = rotatableCircuit.rotation.eulerAngles;
                    float zRotation = eulerAngles.z;
                    if (Mathf.Abs(zRotation) >= 0.01f)
                    {
                        judgeZero = false;
                        break;
                    }
                }
                if (judgeZero && !circuitCompleted && !cameraState.GetIsRotating())
                {
                    circuitCompleted = true;
                    Debug.Log("Circuit Completed!!");
                    showPlatform();
                }
            }
        }
    }

    private void showPlatform()
    {
        if (circuitCompleted)
        {
            foreach (Transform platform in platforms)
            {
                platform.gameObject.SetActive(true);
            }
            directionManager.UpdateInvisibleCubes();
        }
        circuitInfo.SetIsShowed(true);
    }
}
