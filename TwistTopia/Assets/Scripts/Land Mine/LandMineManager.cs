using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineManager : MonoBehaviour
{
    public Transform map;
    public List<Transform> landMineList;
    public EnemyManager enemyManager;
    public GameObject player;
    private PlayerReturn playerReturn;
    public CameraState cameraState;
    public GameObject platformCube;
    // Start is called before the first frame update
    void Start()
    {
        playerReturn = player.GetComponent<PlayerReturn>();
        Transform platformCubes = map.Find("Platform Cubes");
        foreach (Transform platform in platformCubes)
        {
            foreach (Transform cube in platform)
            {
                if (cube.CompareTag("Land Mine"))
                {
                    landMineList.Add(cube);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TouchPlayer();
        if (enemyManager != null)
        {
            TouchEnemy();
        }
    }

    private void TouchPlayer()
    {
        foreach (Transform landMine in landMineList)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Up)
            {
                if (Mathf.Abs(landMine.position.x - player.transform.position.x) <= 0.5f &&
                    Mathf.Abs(landMine.position.z - player.transform.position.z) <= 0.5f)
                {
                    playerReturn.ResetPlayer();
                }
            }
        }
    }

    private void TouchEnemy()
    {
        // Stand on land mine
        if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            Transform platformCubes = map.Find("Platform Cubes");
            foreach (Transform landMine in landMineList)
            {
                foreach(Transform enemy in enemyManager.enemyList)
                {
                    if (Mathf.Abs(landMine.position.x - enemy.position.x) <= 0.5f &&
                        Mathf.Abs(landMine.position.z - enemy.position.z) <= 0.5f)
                    {
                        GameObject newPlatformCube = Instantiate(platformCube) as GameObject;
                        newPlatformCube.transform.position = landMine.position;
                        newPlatformCube.transform.SetParent(landMine.parent);

                        landMineList.Remove(landMine);
                        Destroy(gameObject);
                        enemyManager.DestoryEnemy(enemy);
                        return;
                    }
                }
            }
        }
    }
}
