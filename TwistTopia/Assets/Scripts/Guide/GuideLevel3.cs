using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLevel3 : MonoBehaviour
{

    private GameObject panel;

    // Portal
    public GameObject portal;
    public bool portalIsShowed = false;
    public CameraState cameraState;
    public Transform player;
    public Transform firstPortal;
    public float WorldUnit = 1f;

    // Enemy
    public GameObject enemy;
    public bool enemyIsShowed = false;
    public Vector3 enemy1;
    public Vector3 enemy2;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.Find("Guide Panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!portalIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (CloseToPortal())
                {
                    portal.SetActive(true);
                    panel.SetActive(true);
                    portalIsShowed = true;
                    Time.timeScale = 0f;
                }
            }
        }
        if (!enemyIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (CloseToEnemy())
                {
                    enemy.SetActive(true);
                    panel.SetActive(true);
                    enemyIsShowed = true;
                    Time.timeScale = 0f;
                }
            }
        }
    }

    private bool CloseToPortal()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            if (player.position.y - firstPortal.position.y < WorldUnit + 0.1f &&
                player.position.y - firstPortal.position.y > 0 &&
                Mathf.Abs(firstPortal.position.x - player.position.x) < WorldUnit * 2f)
            {
                return true;
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            if (Mathf.Abs(firstPortal.position.z - player.position.z) < WorldUnit * 2f &&
                Mathf.Abs(firstPortal.position.x - player.position.x) < WorldUnit * 2f)
            {
                return true;
            }
        }
        return false;
    }

    private bool CloseToEnemy()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            if (player.position.y - enemy1.y < WorldUnit + 0.1f &&
                player.position.y - enemy1.y > 0 &&
                Mathf.Abs(enemy1.x - player.position.x) < WorldUnit)
            {
                Debug.Log(111);
                return true;
            }
            if (player.position.y - enemy2.y < WorldUnit + 0.1f &&
                player.position.y - enemy2.y > 0 &&
                Mathf.Abs(enemy2.x - player.position.x) < WorldUnit)
            {
                Debug.Log(222);
                return true;
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            if (Mathf.Abs(enemy1.z - player.position.z) < WorldUnit &&
                Mathf.Abs(enemy1.x - player.position.x) < WorldUnit)
            {
                Debug.Log(333);
                return true;
            }
            if (Mathf.Abs(enemy2.z - player.position.z) < WorldUnit &&
                Mathf.Abs(enemy2.x - player.position.x) < WorldUnit)
            {
                Debug.Log(444);
                return true;
            }
        }
        return false;
    }
}
