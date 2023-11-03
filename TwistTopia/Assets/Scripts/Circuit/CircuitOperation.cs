using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitOperation : MonoBehaviour
{
    private CircuitManager circuitManager;
    private Transform player;
    private CameraState cameraState;
    public KeyCode rotateCircuitCode;
    public CircuitType type;
    public List<Transform> rotatableCircuits;
    
    
    // Start is called before the first frame update
    void Start()
    {
        circuitManager = transform.parent.gameObject.GetComponent<CircuitManager>();
        player = circuitManager.player;
        cameraState = circuitManager.cameraState;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCircuit();
    }

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
                            rotatableCircuit.Rotate(Vector3.up, 90.0f);
                            break;
                        }
                    }
                }
            }
            
        }
    }
}
