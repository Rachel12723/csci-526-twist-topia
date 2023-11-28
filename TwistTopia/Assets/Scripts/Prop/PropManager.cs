using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

public class PropManager : MonoBehaviour
{
    //public TMPro.TextMeshProUGUI bagtext;
    private float xOffset = 0.8f;
    private float yOffset = -0.2f;
    private float zOffset = -0.5f;
    private float xOffset_landmine = 0.4f;
    private float yOffset_landmine = -0.3f;
    private float zOffset_landmine = -0.6f;
    private float xOffset_frame = 0.53f;
    private float yOffset_frame = 0.029f;
    private float zOffset_frame = -0.81f;
    public GameObject keyponInHand;
    private GameObject inHandKeypon;
    public GameObject frameInHand;
    private GameObject inHandFrame;
    public FrameAction frameAction;
    public Material frameEmpty; // Assign this material in the Inspector
    public Material frameCaught; 
    public GameObject LandMineInHand;
    private GameObject inHandLandMine;
    public Transform player;
    //public GameObject player;
    //public GameObject bagtext;
    string [] bagArray = new string[4];
    public InputManager inputManager;
    public static int itemidx;
    public Sprite key;
    public Sprite keyup;
    public Sprite frameEmptySprite;
    public Sprite frameCaughtSprite;
    public Sprite landmine;
    public Sprite none;
    public Button[] bagbutton;
    public Button[] bagbuttonselect;
    public CameraState cameraState;
    public List<KeyValuePair<string, int>> mybaglist;
    Color oldColor = new Color(200f / 255f, 139f / 255f, 73f / 255f);
    Color newColor = new Color(239f / 255f, 237f / 255f, 87f / 255f);

    public Transform keypons;
    public Transform frame;
    public Transform map;
    public Transform landMines;
    private Transform platformCubes;
    public List<Transform> landMineCubeList;
    //public CameraState cameraState;
    public KeyCode Ekey;
    private PlayerState playerState;
    //public GameObject playerr;
    public float WorldUnit = 1.000f;
    // Start is called before the first frame update
    void Start()
    {
        playerState = player.GetComponent<PlayerState>();
        itemidx = 0;

        PlayerPrefs.SetInt("state", 0);//���棿
        PlayerPrefs.SetInt("estate", 0);
        PlayerPrefs.SetInt("landstate", 0);
        PlayerPrefs.SetInt("keystate", 0);
        //0:circuit; 1:keypon; 2:frame
        PlayerPrefs.SetInt("None", 1);
        PlayerPrefs.SetInt("Keypon", 0);
        PlayerPrefs.SetInt("Frame", 0);
        PlayerPrefs.SetInt("Landmine", 0);
        bagArray[0] = "None";
        bagArray[1] = "Keypon";
        bagArray[2] = "Frame";
        bagArray[3] = "Landmine";
        //bagtext.color = Color.green;

        PlayerPrefs.SetString("add", "");
        PlayerPrefs.SetString("minus", "");
        mybaglist = new List<KeyValuePair<string, int>>();

        for (int i = 0; i < bagbutton.Length; i++)
        {
            int buttonIndex = i; // ����һ���ֲ����������水ť����
            bagbutton[i].onClick.AddListener(() => OnButtonClick(buttonIndex));
        }

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
        //landMines = GameObject.Find("Land Mines").transform;



    }

    // Update is called once per frame
    void Update()
    {
        int state = PlayerPrefs.GetInt("state");
        int num = PlayerPrefs.GetInt(bagArray[state]);

        //select
        if (inputManager.GetAllowInteraction())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnButtonClick(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnButtonClick(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnButtonClick(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                OnButtonClick(3);
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                int i;
                for(i = 0; i < 4; i++)
                {
                  
                    Image buttonImage = bagbuttonselect[i].GetComponent<Image>();
                    if (buttonImage.color == newColor)
                    {
                        //Debug.Log(i);
                        OnButtonClick((i + 1) % 4);
                        break;
                    }
                }
                if (i == 4)
                {
                    OnButtonClick(0);
                }
                //OnButtonClick(3);
            }
        }

        //keypon
        if (state == 1 && num == 0 && inHandKeypon != null)
        {
            Destroy(inHandKeypon);
        }
        else if(state != 1 && inHandKeypon != null)
        {
            Destroy(inHandKeypon);
        }
        else if(state == 1 && num > 0 && inHandKeypon == null)
        {
            inHandKeypon = Instantiate(keyponInHand, player.transform);
            inHandKeypon.GetComponent<PropAnimation>().enabled = false;
            inHandKeypon.transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
        }
        //landmine
        if (state == 3 && num == 0 && inHandLandMine != null)
        {
            Destroy(inHandLandMine);
        }
        else if (state != 3 && inHandLandMine != null)
        {
            Destroy(inHandLandMine);
        }
        else if (state == 3 && num > 0 && inHandLandMine == null)
        {
            inHandLandMine = Instantiate(LandMineInHand, player.transform);
            inHandLandMine.GetComponent<PropAnimation>().enabled = false;
            inHandLandMine.transform.localPosition = new Vector3(xOffset_landmine, yOffset_landmine, zOffset_landmine);
        }
        //frame
        if (state == 2 && num == 0 && inHandFrame != null)
        {
            Destroy(inHandFrame);
        }
        else if (state != 2 && inHandFrame != null)
        {
            Destroy(inHandFrame);
        }
        else if (state == 2 && num > 0 && inHandFrame == null)
        {
            inHandFrame = Instantiate(frameInHand, player.transform);
            Renderer instanceRenderer = inHandFrame.gameObject.GetComponent<Renderer>();
            instanceRenderer.material = frameAction.frameState ? frameCaught : frameEmpty;
            inHandFrame.transform.localPosition = new Vector3(xOffset_frame, yOffset_frame, zOffset_frame);
        }

        string item = PlayerPrefs.GetString("add");
        if (item != "")
        {
            additem(item);
            PlayerPrefs.SetString("add", "");
            Debug.Log("gaga");

        }

        string useitem = PlayerPrefs.GetString("minus");
        if (useitem != "")
        {
            minusitem(useitem);
            PlayerPrefs.SetString("minus", "");
            Debug.Log("haha");

        }

        //keyup or front
        for (int i = 0; i < 4; i++)
        {
            Image image = bagbutton[i].GetComponent<Image>();
            if (image.sprite == key || image.sprite == keyup)
            {
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    image.sprite = key;
                }
                else
                {
                    image.sprite = keyup;
                }
            }
                    
        }

        if (inputManager.GetAllowInteraction() && Input.GetKeyDown(Ekey))
        {
            if (!playerState.GetFrontIsDropping())
            {
                int flag1 = 0;
                int flag2 = 0;
                int flag3 = 0;
                int flag4 = 0;
                //选中画框时放画框
                if (state == 2)
                {
                    //PlayerPrefs.SetInt("estate", 1);
                    flag1 = 1;
                }

                //拿起keypon
                if (cameraState.GetFacingDirection() == FacingDirection.Front && keypons != null)
                {
                    Debug.Log("pickupkeypon");
                    foreach (Transform keypon in keypons)
                    {
                        if (Mathf.Abs(keypon.position.y - player.transform.position.y) < WorldUnit + 0.5f &&
                            Mathf.Abs(keypon.position.x - player.transform.position.x) < WorldUnit + 0.5f)
                        {
                            flag2 = 1;
                            Debug.Log("!!!yes");
                            break;
                        }
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up && keypons != null)
                {
                    foreach (Transform keypon in keypons)
                    {
                        Debug.Log(Mathf.Abs(keypon.position.z - player.transform.position.z));
                        Debug.Log(Mathf.Abs(keypon.position.x - player.transform.position.x));
                        if (Mathf.Abs(keypon.position.z - player.transform.position.z) < WorldUnit + 0.25f &&
                            Mathf.Abs(keypon.position.x - player.transform.position.x) < WorldUnit + 0.25f)
                        {
                            flag2 = 1;
                            Debug.Log("!!!yes");
                            break;
                        }
                    }
                }

                //拿起画框
                if (frame)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Front)
                    {


                        if (frame.gameObject.activeSelf && Mathf.Abs(frame.position.x - player.transform.position.x) < WorldUnit + 0.5f)
                        {
                            flag3 = 1;
                        }
                    }
                }
                //拿起地雷
                if (landMines != null)
                {
                   // Debug.Log("lanmind!!");
                    foreach (Transform landMineProp in landMines)
                    {
                        if (landMineProp.gameObject.activeSelf)
                        {
                           
                            if (cameraState.GetFacingDirection() == FacingDirection.Front)
                            {
                                Debug.Log(Mathf.Abs(player.transform.position.x - landMineProp.position.x));
                                Debug.Log(Mathf.Abs(player.transform.position.y - landMineProp.position.y));
                                if (Mathf.Abs(player.transform.position.x - landMineProp.position.x) <= 0.5f
                                    && Mathf.Abs(player.transform.position.y - landMineProp.position.y) <= 0.2f)
                                {
                                    Debug.Log("lanmind!!");
                                    flag4 = 1;
                                }
                            }
                            else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                            {
                                if (Mathf.Abs(player.transform.position.x - landMineProp.position.x) <= 0.5f
                                    && Mathf.Abs(player.transform.position.z - landMineProp.position.z) <= 0.5f)
                                {
                                    flag4 = 1;
                                }
                            }
                        }
                    }
                }

                //
                if(flag1 == 1)
                {
                    PlayerPrefs.SetInt("estate", 1);
                }
                else if (flag2 == 1)
                {
                    PlayerPrefs.SetInt("estate", 2);
                }
                else if (flag3 == 1)
                {
                    PlayerPrefs.SetInt("estate", 3);
                }
                else if (flag4 == 1)
                {
                    PlayerPrefs.SetInt("estate", 4);
                }
                Debug.Log("estate");
                Debug.Log(PlayerPrefs.GetInt("estate"));
              



            }
        }
    }

   

    

    public void additem(string item)
    {
      
        int i = 0;
        if (mybaglist != null)
        {
            for (i = 0; i < mybaglist.Count; i++)
            {
                Debug.Log(i);
                KeyValuePair<string, int> kvp = mybaglist[i];
                //Console.WriteLine("Key: " + kvp.Key + ", Value: " + kvp.Value);
                if (kvp.Key == item)
                {
                    kvp = new KeyValuePair<string, int>(kvp.Key, kvp.Value + 1);
                    mybaglist[i] = kvp;
                    TextMeshProUGUI buttonText = bagbutton[i].GetComponentInChildren<TextMeshProUGUI>();
                    buttonText.text = kvp.Value.ToString();
                    Image image = bagbutton[i].GetComponent<Image>();
                    OnButtonClick(i);
                    
                    if (item == "key")
                    {
                        image.sprite = key;
                    }
                    else if (item == "frame")
                    {
                        image.sprite = frameAction.frameState ? frameCaughtSprite : frameEmptySprite;
                    }
                    else if (item == "landmine")
                    {
                        image.sprite = landmine;
                    }
                    Color tempColor = image.color;
                    tempColor.a = 1f; // 1f ��ʾ��͸��
                    image.color = tempColor;
                    return;
                }
            }
        }
        mybaglist.Add(new KeyValuePair<string, int>(item, 1));
        TextMeshProUGUI buttonTextt = bagbutton[i].GetComponentInChildren<TextMeshProUGUI>();
        buttonTextt.text = "1";
        Image imagee = bagbutton[i].GetComponent<Image>();
        OnButtonClick(i);
        if (item == "key")
        {
            imagee.sprite = key;
        }
        else if (item == "frame")
        {
            imagee.sprite = frameAction.frameState ? frameCaughtSprite : frameEmptySprite;
        }
        else if (item == "landmine")
        {
            imagee.sprite = landmine;
        }
        Color tempColorr = imagee.color;
        tempColorr.a = 1f; // 1f ��ʾ��͸��
        imagee.color = tempColorr;

        Image buttonImage = bagbuttonselect[i].GetComponent<Image>();
        if (buttonImage.color == newColor)
        {
            if (item == "key")
            {
                inHandKeypon = Instantiate(keyponInHand, player.transform);
                inHandKeypon.GetComponent<PropAnimation>().enabled = false;
                inHandKeypon.transform.localPosition = new Vector3(xOffset, yOffset, zOffset);
                PlayerPrefs.SetInt("state", 1);
            }
            else if (item == "frame")
            {
                PlayerPrefs.SetInt("state", 2);
            }
            else if (item == "landmine")
            {
                inHandLandMine = Instantiate(LandMineInHand, player.transform);
                inHandLandMine.GetComponent<PropAnimation>().enabled = false;
                inHandLandMine.transform.localPosition = new Vector3(xOffset_landmine, yOffset_landmine, zOffset_landmine);
                PlayerPrefs.SetInt("state", 3);
            }
        }



    }

    public void minusitem(string item)
    {
        int i = 0;
        int idx = 0;
        for (i = 0; i < mybaglist.Count; i++)
        {
            Debug.Log(i);
            KeyValuePair<string, int> kvp = mybaglist[i];
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
            if (kvp.Key == item)
            {
                idx = i;
                kvp = new KeyValuePair<string, int>(kvp.Key, kvp.Value - 1);
                Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
                mybaglist[i] = kvp;
                /*TextMeshProUGUI buttonText = bagbutton[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = kvp.Value.ToString();*/
                //Image image = bagbutton[i].GetComponent<Image>();
                //if (kvp.Value == 0 && kvp.Key != "frame")
                if (kvp.Value == 0)
                {
                    
                    mybaglist.RemoveAt(i);

                    //image.sprite = none; 
                    //Color tempColor = image.color;
                    //tempColor.a = 0f; // 1f ��ʾ��͸��
                   // image.color = tempColor;
                    //buttonText.text = "";
                  /*  Image buttonImage = bagbuttonselect[i].GetComponent<Image>();
                    buttonImage.color = new Color(200f / 255f, 139f / 255f, 73f / 255f);*/
                }
                break;
            }
        }
        for (i = 0; i < 4; i++)
        {
            Debug.Log(i + "baglist:" + mybaglist.Count);
            Image image = bagbutton[i].GetComponent<Image>();
            TextMeshProUGUI buttonText = bagbutton[i].GetComponentInChildren<TextMeshProUGUI>();
            //buttonText.text = kvp.Value.ToString();
            if (i < mybaglist.Count)
            {
                KeyValuePair<string, int> kvpp = mybaglist[i];
                
                kvpp = mybaglist[i];
                buttonText.text = kvpp.Value.ToString();
                if (kvpp.Key == "key")
                {
                    image.sprite = key;
                }
                else if (kvpp.Key == "frame")
                {
                    image.sprite = frameAction.frameState ? frameCaughtSprite : frameEmptySprite;
                }
                else if (kvpp.Key == "landmine")
                {
                    image.sprite = landmine;
                }
                Color tempColor = image.color;
                tempColor.a = 1f; // 1f ��ʾ��͸��
                image.color = tempColor;
            }
            else
            {
                Debug.Log("ao");
                image.sprite = none;
                buttonText.text = "";
                Color tempColorr = image.color;
                tempColorr.a = 0f; // 1f ��ʾ��͸��
                image.color = tempColorr;
            }
        }
        OnButtonClick(idx);
    }

    void OnButtonClick(int buttonIndex)
    {
        /*Color oldColor = new Color(200f / 255f, 139f / 255f, 73f / 255f);
        Color newColor = new Color(239f / 255f, 237f / 255f, 87f / 255f);*/
        for(int i = 0; i <= 3; i++)
        {
            Image buttonImage = bagbuttonselect[i].GetComponent<Image>();
            if (i == buttonIndex)
            {
                buttonImage.color = newColor;
            }
            else
            {
                buttonImage.color = oldColor;
            }
           
        }
        if (buttonIndex < mybaglist.Count)
        {
            Image image = bagbutton[buttonIndex].GetComponent<Image>();
            if (image.sprite == key || image.sprite == keyup)
            {
                PlayerPrefs.SetInt("state", 1);
            }
            else if(image.sprite == frameCaughtSprite || image.sprite == frameEmptySprite)
            {
                PlayerPrefs.SetInt("state", 2);
            }
            else if (image.sprite == landmine)
            {
                PlayerPrefs.SetInt("state", 3);
            }
        }
        else
        {
            PlayerPrefs.SetInt("state", 0);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

}
