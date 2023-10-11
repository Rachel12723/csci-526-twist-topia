using UnityEngine;
using System.Collections;

public class PlayerReturn : MonoBehaviour
{

    public GameObject player;
    public Vector3 checkPoint = Vector3.zero;
    public float MinY;
    public GameObject directionManager;

    void Update()
    {
        if (player.transform.position.y < MinY)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = checkPoint;
            directionManager.GetComponent<DirectionManager>().UpdateInvisibleCubes();
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    public void SetCheckPoint(Vector3 checkPoint)
    {
        this.checkPoint = checkPoint;
    }
}
