using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    public float speed;

    public GameObject player;
    private PlayerReturn playerReturn;

    public CameraState cameraState;

    public Transform map;
    public List<Transform> landMines;

    public Transform keys;
    public GameObject key;

    public GameObject platformCube;

    void Start()
    {
        playerReturn = player.GetComponent<PlayerReturn>();
        Transform platformCubes = map.Find("Platform Cubes");
        foreach(Transform platform in platformCubes)
        {
            foreach (Transform cube in platform)
            {
                if(cube.CompareTag("Land Mine"))
                {
                    landMines.Add(cube);
                }
            }
        }
    }

    void Update()
    {
        foreach(Transform landMine in landMines)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Up)
            {
                if (Mathf.Abs(landMine.position.x - player.transform.position.x) <= 0.5f &&
                    Mathf.Abs(landMine.position.z - player.transform.position.z) <= 0.5f)
                {
                    playerReturn.ResetPlayer(FacingDirection.Up);
                }
            }
        }
    }
}
