using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    public float speed;

    public GameObject player;

    public CameraState cameraState;

    public Transform map;
    public List<Transform> landMines;

    public Transform keys;
    public GameObject key;

    public GameObject platformCube;

    void Start()
    {
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
}
