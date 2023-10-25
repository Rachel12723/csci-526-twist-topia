using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBinding : MonoBehaviour
{
    public GameObject player;
    private CameraState cameraState;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
        cameraState = GetComponent<CameraState>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
