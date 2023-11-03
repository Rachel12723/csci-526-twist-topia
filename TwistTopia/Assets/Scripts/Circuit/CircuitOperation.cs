using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitOperation : MonoBehaviour
{
    private CircuitManager circuitManager;
    private Transform player;
    private CameraState cameraState;
    // button for rotation
    public KeyCode rotateCircuitCode;
    public CircuitType type;
    // list for rotatable circuits
    public List<Transform> rotatableCircuits;

    public Transform platform;
    // degrees when circuit rotates
    private float rotateDegrees = 90.0f;
    // record if circuit is completed
    private bool circuitCompleted = false;

    private DirectionManager directionManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        circuitManager = transform.parent.gameObject.GetComponent<CircuitManager>();
        player = circuitManager.player;
        cameraState = circuitManager.cameraState;
        directionManager = circuitManager.directionManager;
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
        if (Input.GetKeyDown(rotateCircuitCode))
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
                            rotatableCircuit.Rotate(Vector3.up, rotateDegrees);
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
                            rotatableCircuit.Rotate(Vector3.forward, rotateDegrees);
                            break;
                        }
                    }
                }
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
                    if (yRotation != 0.0f)
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
                    if (zRotation != 0.0f)
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
            platform.gameObject.SetActive(true);
            directionManager.UpdateInvisibleCubes();
        }
            
    }
}
