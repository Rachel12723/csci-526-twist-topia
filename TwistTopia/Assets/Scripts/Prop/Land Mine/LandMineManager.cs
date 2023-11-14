using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineManager : MonoBehaviour
{
    public Transform map;
    private Transform platformCubes;
    public List<Transform> landMineCubeList;
    public EnemyManager enemyManager;
    public GameObject player;
    private PlayerReturn playerReturn;
    public CameraState cameraState;
    public GameObject platformCube;
    public GameObject landMineCube;
    public GameObject landMineInHand;
    private GameObject inHandLandMine;
    private float xOffset = 0.5f;
    private float zOffset = -0.85f;
    public InputManager inputManager;
    public KeyCode pickUpKeyCode;
    public KeyCode setUpKeyCode;
    public int landMineCounter = 0;
    private Transform unactivatedLandMine = null;
    private Transform landMines;
    // Start is called before the first frame update
    void Start()
    {
        playerReturn = player.GetComponent<PlayerReturn>();
        platformCubes = map.Find("Platform Cubes");
        foreach (Transform platform in platformCubes)
        {
            foreach (Transform cube in platform)
            {
                if (cube.CompareTag("Land Mine"))
                {
                    landMineCubeList.Add(cube);
                }
            }
        }
        landMines = transform.Find("Land Mines");
    }

    // Update is called once per frame
    void Update()
    {
        pickUpLandMine();
        SetUpLandMine();
        if (unactivatedLandMine!=null)
        {
            Debug.Log("wait");
            ActivateLandMine();
        }
        TouchPlayer();
        if (enemyManager != null)
        {
            TouchEnemy();
        }
    }

    private void pickUpLandMine()
    {
        if (Input.GetKeyDown(pickUpKeyCode))
        {
            if (inputManager.GetAllowInteraction())
            {
                foreach(Transform landMineProp in landMines)
                {
                    if (landMineProp.gameObject.activeSelf)
                    {
                        if (cameraState.GetFacingDirection() == FacingDirection.Front)
                        {
                            if(Mathf.Abs(player.transform.position.x-landMineProp.position.x)<=0.5f
                                && Mathf.Abs(player.transform.position.y - landMineProp.position.y) <= 0.2f)
                            {
                                landMineProp.gameObject.SetActive(false);
                                inHandLandMine = Instantiate(landMineInHand, player.transform);
                                inHandLandMine.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                                landMineCounter++;
                                return;
                            }
                        }
                        else if(cameraState.GetFacingDirection() == FacingDirection.Up)
                        {
                            if (Mathf.Abs(player.transform.position.x - landMineProp.position.x) <= 0.5f
                                && Mathf.Abs(player.transform.position.z - landMineProp.position.z) <= 0.5f)
                            {
                                landMineProp.gameObject.SetActive(false);
                                inHandLandMine = Instantiate(landMineInHand, player.transform);
                                inHandLandMine.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                                landMineCounter++;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetUpLandMine()
    {
        if (Input.GetKeyDown(setUpKeyCode))
        {
            if (inputManager.GetAllowInteraction())
            {
                if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    foreach (Transform landMineProp in landMines)
                    {
                        if (!landMineProp.gameObject.activeSelf)
                        {
                            foreach (Transform platform in platformCubes)
                            {
                                foreach (Transform cube in platform)
                                {
                                    if (!cube.CompareTag("Land Mine")
                                        && Mathf.Abs(player.transform.position.x - cube.position.x) < 0.5f
                                        && Mathf.Abs(player.transform.position.z - cube.position.z) < 0.5f)
                                    {
                                        GameObject newLandMineCube = Instantiate(landMineCube) as GameObject;
                                        newLandMineCube.transform.position = cube.position;
                                        newLandMineCube.transform.SetParent(cube.parent);
                                        unactivatedLandMine = newLandMineCube.transform;

                                        Destroy(cube.gameObject);
                                        Destroy(landMineProp.gameObject);
                                        landMineCounter--;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void ActivateLandMine()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            landMineCubeList.Add(unactivatedLandMine);
            unactivatedLandMine = null;
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            Debug.Log("Up");
            Debug.Log(Mathf.Abs(player.transform.position.x - unactivatedLandMine.position.x));
            Debug.Log(Mathf.Abs(player.transform.position.z - unactivatedLandMine.position.z));
            if (Mathf.Abs(player.transform.position.x - unactivatedLandMine.position.x) >= 0.5f
                || Mathf.Abs(player.transform.position.z - unactivatedLandMine.position.z) >= 0.5f)
            {
                landMineCubeList.Add(unactivatedLandMine);
                unactivatedLandMine = null;
            }
        }
    }

    private void TouchPlayer()
    {
        foreach (Transform landMine in landMineCubeList)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating())
            {
                if (Mathf.Abs(landMine.position.x - player.transform.position.x) < 0.5f &&
                    Mathf.Abs(landMine.position.z - player.transform.position.z) < 0.5f)
                {
                    playerReturn.ResetPlayer();
                }
            }
        }
    }

    private void TouchEnemy()
    {
        // Stand on land mine
        if (cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating())
        {
            foreach (Transform landMine in landMineCubeList)
            {
                foreach(Transform enemy in enemyManager.enemyList)
                {
                    if (Mathf.Abs(landMine.position.x - enemy.position.x) < 0.5f &&
                        Mathf.Abs(landMine.position.z - enemy.position.z) < 0.5f)
                    {
                        GameObject newPlatformCube = Instantiate(platformCube) as GameObject;
                        newPlatformCube.transform.position = landMine.position;
                        newPlatformCube.transform.SetParent(landMine.parent);

                        landMineCubeList.Remove(landMine);
                        Destroy(landMine.gameObject);
                        enemyManager.DestroyEnemy(enemy);
                        return;
                    }
                }
            }
        }
    }
}
