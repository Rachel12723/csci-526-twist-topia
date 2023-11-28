using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrame : MonoBehaviour
{
    // player action keyCode
    private Transform platformCubes;
    public Transform map;
    public List<Transform> landMineCubeList;
    public Transform keypons;

    public KeyCode pickUpFrameCode;
    public KeyCode dropOffFrameCode;
    public int frameCounter = 0;
    public Transform frame;
    //public UnityEngine.UI.Text frameText;
    public CameraState cameraState;
    public float WorldUnit = 1.000f;
    public int maxZ;
    private float yOffset = 1.15625f;
    private FrameAction frameAction;
    private Vector3 originalPos;
    public GameObject player;
    private PlayerState playerState;

    public InputManager inputManager;

    //
    private Transform landMines;

    void Start()
    {
        // Debug.Log(" x"+frame.position.x+" y"+frame.position.y+" z"+frame.position.z);
        originalPos = frame.position;
        playerState = player.GetComponent<PlayerState>();
        frameAction = frame.gameObject.GetComponent<FrameAction>();

        //
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
        //frameText.text = "Frame: " + frameCounter;
        int estate = PlayerPrefs.GetInt("estate");
        int state = PlayerPrefs.GetInt("state");
        if (frameCounter == 0 && estate == 3)
        {
            PickUpFrame();
        }
        else if (state == 2 && frameCounter > 0 && estate == 1)
        {
            DropOffFrame();
        }

        /*if (inputManager.GetAllowInteraction())
        {
            if (!playerState.GetFrontIsDropping())
            {
                int state = PlayerPrefs.GetInt("state");
                if (Input.GetKeyDown(pickUpFrameCode))
                {
                    Debug.Log(111);
                    if (frameCounter == 0)
                    {
                        PickUpFrame();
                    }
                    else if (state == 2 && frameCounter > 0)
                    {
                        DropOffFrame();
                    }
                }
            }
        }*/
    }

	private void PickUpFrame()
    {
        if (frame)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                
               
                if (frame.gameObject.activeSelf && Mathf.Abs(frame.position.x - player.transform.position.x) < WorldUnit + 0.5f)
                {
                    frame.gameObject.SetActive(false);
                    frameCounter++;
                    //                   
                    int framenum = PlayerPrefs.GetInt("Frame");
                    PlayerPrefs.SetInt("Frame", framenum + 1);
                    //
                    PlayerPrefs.SetString("add", "frame");
                    // break;
                }
            }
        }
        PlayerPrefs.SetInt("estate", 0);
    }

	private void DropOffFrame(){
        Debug.Log("¿ÉÒÔdropÁË£¡");
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            /*if (landMines != null)
            {
                foreach (Transform landMineProp in landMines)
                {
                    *//* Debug.Log("JInlail");*//*
                    if (landMineProp.gameObject.activeSelf)
                    {

                        if (cameraState.GetFacingDirection() == FacingDirection.Front)
                        {
                            if (Mathf.Abs(player.transform.position.x - landMineProp.position.x) <= 0.5f
                                && Mathf.Abs(player.transform.position.y - landMineProp.position.y) <= 0.2f)
                            {
                                PlayerPrefs.SetInt("landstate", 0);
                                return;
                            }
                        }

                    }
                }
            }
            int landstate = PlayerPrefs.GetInt("landstate");
            if (landstate == 1)
            {
                PlayerPrefs.SetInt("landstate", 0);
                return;
            }
            if (keypons != null)
            {
                foreach (Transform keypon in keypons)
                {
                    if (Mathf.Abs(keypon.position.y - player.transform.position.y) < WorldUnit + 0.5f &&
                        Mathf.Abs(keypon.position.x - player.transform.position.x) < WorldUnit + 0.5f)
                    {

                        PlayerPrefs.SetInt("keystate", 0);
                        return;
                    }
                }
            }
            int keystate = PlayerPrefs.GetInt("keystate");
            if (keystate == 1)
            {
                PlayerPrefs.SetInt("keystate", 0);
                return;
            }*/
            // Transform frame = frames.GetChild(0);
            Vector3 playerCurrPos = player.transform.position;
            frame.position = new Vector3(playerCurrPos.x, playerCurrPos.y + yOffset - 1, maxZ);
            frame.gameObject.SetActive(true);
            frameCounter--;
            //                   
            int framenum = PlayerPrefs.GetInt("Frame");
            PlayerPrefs.SetInt("Frame", framenum - 1);
            //
            PlayerPrefs.SetString("minus", "frame");
            frameAction.ReleaseEnemy(true);
        }
        PlayerPrefs.SetInt("estate", 0);
    }

    public void ResetFrame()
    {
        // frameAction.ReleaseEnemy(true);
        frame.position = originalPos;
        frame.gameObject.SetActive(true);
        frameCounter = 0;
        //                   
        PlayerPrefs.SetInt("Frame", 0);
        //
        PlayerPrefs.SetString("minus", "frame");
    }
}
