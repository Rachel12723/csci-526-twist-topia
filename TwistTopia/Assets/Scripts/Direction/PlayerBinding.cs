using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBinding : MonoBehaviour
{
    public GameObject player;
    private CameraState cameraState;
    public float rebindingTime;
    private bool setRebindingSpeed = false;
    private float rebindingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
        cameraState = GetComponent<CameraState>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!cameraState.GetIsRebinding())
        {
            transform.position = player.transform.position;
        }
        else
        {
            if (!setRebindingSpeed)
            {
                rebindingSpeed= Vector3.Distance(transform.position, player.transform.position) * (1 / rebindingTime);
                setRebindingSpeed = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, rebindingSpeed * Time.deltaTime);
            if(transform.position == player.transform.position)
            {
                cameraState.SetIsRebinding(false);
                setRebindingSpeed = false;
            }
        }
    }
}
