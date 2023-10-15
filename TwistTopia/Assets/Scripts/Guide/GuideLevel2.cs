using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLevel2 : MonoBehaviour
{

    private GameObject panel;

    // Find Key
    public GameObject findKey;
    public bool findKeyIsShowed = false;
    public CameraState cameraState;
    public Transform player;
    public Transform blockCubes;
    public float WorldUnit = 1f;

    // Pick Up Key
    public GameObject pickUpKey;
    public bool pickUpKeyIsShowed = false;
    public Transform keys;

    // Drop Key
    public GameObject dropKey;
    public bool dropKeyIsShowed = false;
    public KeyAndDoor keyAndDoor;

    // Open Door
    public GameObject openDoor;
    public bool openDoorIsShowed = false;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.Find("Guide Panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!findKeyIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (CloseToDoor())
                {
                    findKey.SetActive(true);
                    panel.SetActive(true);
                    findKeyIsShowed = true;
                    Time.timeScale = 0f;
                }
            }
        }
        if (!pickUpKeyIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (CloseToKey())
                {
                    pickUpKey.SetActive(true);
                    panel.SetActive(true);
                    pickUpKeyIsShowed = true;
                    findKeyIsShowed = true;
                    Time.timeScale = 0f;
                }

            }
        }
        if (!dropKeyIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (keyAndDoor.GetKeyCounter() > 0)
                {
                    dropKey.SetActive(true);
                    panel.SetActive(true);
                    dropKeyIsShowed = true;
                    Time.timeScale = 0f;
                }

            }
        }
        if (!openDoorIsShowed)
        {
            if (panel.activeSelf == false)
            {
                if (CloseToDoor() && keyAndDoor.GetKeyCounter()>0)
                {
                    openDoor.SetActive(true);
                    panel.SetActive(true);
                    openDoorIsShowed = true;
                    Time.timeScale = 0f;
                }
            }

        }
    }

    private bool CloseToDoor()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform block in blockCubes)
            {
                foreach (Transform blockCube in block)
                {
                    if (Mathf.Abs(blockCube.position.y - player.position.y) < WorldUnit - 0.5f &&
                        Mathf.Abs(blockCube.position.x - player.position.x) < WorldUnit * 2f)
                    {
                        return true;
                    }
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            foreach (Transform block in blockCubes)
            {
                foreach (Transform blockCube in block)
                {
                    if (Mathf.Abs(blockCube.position.z - player.position.z) < WorldUnit * 2f &&
                        Mathf.Abs(blockCube.position.x - player.position.x) < WorldUnit * 2f)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool CloseToKey()
    {
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform key in keys)
            {
                if (Mathf.Abs(key.position.y - player.position.y) < WorldUnit - 0.5f &&
                    Mathf.Abs(key.position.x - player.position.x) < WorldUnit * 2.5f)
                {
                    return true;
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            foreach (Transform key in keys)
            {
                if (Mathf.Abs(key.position.z - player.position.z) < WorldUnit * 2.5f &&
                    Mathf.Abs(key.position.x - player.position.x) < WorldUnit * 2.5f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
