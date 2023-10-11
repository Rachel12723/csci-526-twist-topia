using UnityEngine;
using System.Collections;

public class ReturnPlayer : MonoBehaviour
{

    public GameObject player;
    public float ReturnX;
    public float ReturnY;
    public float ReturnZ;
    public float MinY;
    public GameObject directionManager;

    void Update()
    {
        if (player.transform.position.y < MinY)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(ReturnX, ReturnY, ReturnZ);
            directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
