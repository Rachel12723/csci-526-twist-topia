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
    private PlayerReturn playerReturn;

    private CameraState cameraState;

    private Transform map;

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
        playerReturn = player.GetComponent<PlayerReturn>();

        cameraState = guardManager.cameraState;

        map = guardManager.map;

        keys = guardManager.keys;
        key = guardManager.key;

        platformCube = guardManager.platformCube;
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
                    targetPosition.x = player.transform.position.x;
                    targetPosition.z = player.transform.position.z;
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

                // Reset player
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    if(Mathf.Abs(transform.position.x-player.transform.position.x)<= 1f)
                    {
                        playerReturn.ResetPlayer(FacingDirection.Front);
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 1f &&
                        Mathf.Abs(transform.position.z - player.transform.position.z) <= 1f)
                    {
                        playerReturn.ResetPlayer(FacingDirection.Up);
                    }
                }

                // Stand on land mine
                if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    Transform platformCubes = map.Find("Platform Cubes");
                    foreach(Transform landMine in guardManager.landMines)
                    {
                        if (Mathf.Abs(transform.position.x - landMine.position.x) <= 0.5f &&
                            Mathf.Abs(transform.position.z - landMine.position.z) <= 0.5f)
                        {
                            GameObject newKey = Instantiate(key) as GameObject;
                            newKey.transform.position = transform.position;
                            newKey.transform.SetParent(keys);

                            GameObject newPlatformCube = Instantiate(platformCube) as GameObject;
                            newPlatformCube.transform.position = landMine.position;
                            newPlatformCube.transform.SetParent(landMine.parent);

                            Destroy(gameObject);
                            guardManager.landMines.Remove(landMine);
                            Destroy(landMine.gameObject);
                            return;
                        }
                    }
                }
            }
        }
        else
        {

        }
    }
}
