using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLevel1 : MonoBehaviour
{

    private GameObject panel;

    // Welcome
    public GameObject welcome;
    public bool welcomeIsShowed = false;
    public Transform player;
    public Transform platformCubes;
    public float WorldUnit = 1.000f;

    // Main View Move
    public GameObject mainViewMove;
    public bool mainViewMoveIsShowed = false;
    public CameraState cameraState;

    // View Change
    public GameObject viewChange;
    public bool viewChangeIsShowed = false;
    public float viewChangeMinX = 12f;

    // Top Down View Move
    public GameObject topDownViewMove;
    public bool topDownViewMoveIsShowed = false;

    // Drop
    public GameObject drop;
    public bool dropIsShowed = false;
    public float dropMinX = 27f;
    public PlayerReturn playerReturn;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.Find("Panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!welcomeIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (OnPlatformCube())
                {
                    welcome.SetActive(true);
                    panel.SetActive(true);
                    welcomeIsShowed = true;
                }
            }
        }
        else
        {
            if (!mainViewMoveIsShowed)
            {
                if (panel.activeSelf == false)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Front && OnPlatformCube())
                    {
                        mainViewMove.SetActive(true);
                        panel.SetActive(true);
                        mainViewMoveIsShowed = true;
                    }
                }
            }
            if (!viewChangeIsShowed)
            {
                if (panel.activeSelf == false)
                {
                    if (player.position.x > viewChangeMinX)
                    {
                        viewChange.SetActive(true);
                        panel.SetActive(true);
                        viewChangeIsShowed = true;
                    }
                }
            }
            if (!topDownViewMoveIsShowed)
            {
                if (panel.activeSelf == false)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating())
                    {
                        topDownViewMove.SetActive(true);
                        panel.SetActive(true);
                        topDownViewMoveIsShowed = true;
                    }
                }
            }
            if (!dropIsShowed)
            {
                if (panel.activeSelf == false)
                {
                    if (player.position.x > dropMinX || playerReturn.dropCount > 0)
                    if (player.position.x > dropMinX || playerReturn.dropCount > 0)
                    {
                        drop.SetActive(true);
                        panel.SetActive(true);
                        dropIsShowed = true;
                    }
                }
            }
        }

    }

    private bool OnPlatformCube()
    {
        foreach (Transform platform in platformCubes)
        {
            for (int i = 0; i < platform.childCount; i++)
            {
                if (Mathf.Abs(platform.GetChild(i).position.x - player.position.x) < WorldUnit
                && Mathf.Abs(platform.GetChild(i).position.z - player.position.z) < WorldUnit
                && player.position.y - platform.GetChild(i).position.y < WorldUnit + 0.2f
                && player.position.y - platform.GetChild(i).position.y > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
