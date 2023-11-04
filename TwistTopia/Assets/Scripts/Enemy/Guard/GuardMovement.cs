using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    private bool isAlive = true;

    public Vector3 minRange;
    public Vector3 maxRange;
    private Vector3 outsideMinPosition;
    private Vector3 outsideMaxPosition;
    private Vector3 outsideTargetPosition;
    private Vector3 targetPosition;

    private GuardManager guardManager;

    private float speed;

    private GameObject player;
    private PlayerState playerState;

    private CameraState cameraState;

    private Transform map;

    public bool hasKey = false;
    private Transform keys;
    private GameObject key;

    private GameObject platformCube;

    // Start is called before the first frame update
    void Start()
    {
        // Variable binding
        outsideMinPosition = minRange;
        outsideMaxPosition = new Vector3(maxRange.x, maxRange.y, minRange.z);
        transform.position = outsideMaxPosition;
        outsideTargetPosition = outsideMinPosition;
        targetPosition = outsideTargetPosition;

        guardManager = GetComponentInParent<GuardManager>();

        speed = guardManager.speed;

        player = guardManager.player;
        playerState = player.GetComponent<PlayerState>();

        cameraState = guardManager.cameraState;

        if (hasKey)
        {
            keys = guardManager.keys;
            key = guardManager.key;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            //Movement
            if (!cameraState.isRotating && !playerState.positionUpdating)
            {
                if (player.transform.position.x >= minRange.x - 1f && player.transform.position.x <= maxRange.x + 1f  && player.transform.position.z >= minRange.z - 1f && player.transform.position.z <= maxRange.z + 1f)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Front)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
                    }
                    targetPosition.x = player.transform.position.x;
                    targetPosition.z = player.transform.position.z;
                    outsideMinPosition.z = transform.position.z;
                    outsideMaxPosition.z = transform.position.z;
                    outsideTargetPosition.z = transform.position.z;
                }
                else
                {
                    if (Vector3.Distance(transform.position, outsideTargetPosition) < 0.1f)
                    {
                        if (outsideTargetPosition == outsideMinPosition)
                        {
                            outsideTargetPosition = outsideMaxPosition;
                        }
                        else
                        {
                            outsideTargetPosition = outsideMinPosition;
                        }
                    }
                    if (targetPosition != outsideTargetPosition)
                    {
                        targetPosition = outsideTargetPosition;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


            }
        }
        else
        {

        }
    }

    public void DestroyGuard()
    {
        if (hasKey)
        {
            GameObject newKey = Instantiate(key) as GameObject;
            newKey.transform.position = transform.position;
            newKey.transform.SetParent(keys);
        }

        Destroy(gameObject);
    }
}
