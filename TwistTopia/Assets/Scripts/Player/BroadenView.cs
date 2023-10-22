using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadenView : MonoBehaviour
{
    public KeyCode broadenViewCode;
    private Camera mainCamera;
    private float zoomSpeed = 10f;
    private float maxZoomSize = 10.0f;
    private float minZoomSize = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the main camera in the scene.
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(broadenViewCode))
        {
            //mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Min(mainCamera.orthographicSize + zoomSpeed * Time.deltaTime, maxZoomSize);
        }
        else
        {
            mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize - zoomSpeed * Time.deltaTime, minZoomSize);
        }
    }
}
