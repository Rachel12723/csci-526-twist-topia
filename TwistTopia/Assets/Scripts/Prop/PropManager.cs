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
    private float xOffset = 1.2f;
    private float zOffset = -0.56f;
    private float xOffset_landmine = 0.5f;
    private float zOffset_landmine = -0.85f;
    private float xOffset_frame = 0.67f;
    private float yOffset_frame = 0.029f;
    private float zOffset_frame = -0.81f;
    public GameObject keyponInHand;
    private GameObject inHandKeypon;
    public GameObject frameInHand;
    private GameObject inHandFrame;
    public GameObject LandMineInHand;
    private GameObject inHandLandMine;
    public Transform player;
    //public GameObject bagtext;
    string [] bagArray = new string[4];
    public InputManager inputManager;
    public static int itemidx;
    public Sprite key;
    public Sprite frame;
    public Sprite landmine;
    public Sprite none;
    public Button[] bagbutton;
    public Button[] bagbuttonselect;
    public List<KeyValuePair<string, int>> mybaglist;
    Color oldColor = new Color(200f / 255f, 139f / 255f, 73f / 255f);
    Color newColor = new Color(239f / 255f, 237f / 255f, 87f / 255f);

    // Start is called before the first frame update
    void Start()
    {
        itemidx = 0;

        PlayerPrefs.SetInt("state", 0);//���棿
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
            inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
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
            inHandLandMine.transform.localPosition = new Vector3(xOffset_landmine, 0, zOffset_landmine);
        }
        //frame
        if (state == 2 && num == 0 && inHandLandMine != null)
        {
            Destroy(inHandLandMine);
        }
        else if (state != 2 && inHandLandMine != null)
        {
            Destroy(inHandLandMine);
        }
        else if (state == 2 && num > 0 && inHandLandMine == null)
        {
            inHandFrame = Instantiate(frameInHand, player.transform);
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
                        image.sprite = frame;
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
            imagee.sprite = frame;
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
                inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
                PlayerPrefs.SetInt("state", 1);
            }
            else if (item == "frame")
            {
                PlayerPrefs.SetInt("state", 2);
            }
            else if (item == "landmine")
            {
                inHandLandMine = Instantiate(LandMineInHand, player.transform);
                inHandLandMine.transform.localPosition = new Vector3(xOffset_landmine, 0, zOffset_landmine);
                PlayerPrefs.SetInt("state", 3);
            }
        }



    }

    public void minusitem(string item)
    {
        int i = 0;
        for (i = 0; i < mybaglist.Count; i++)
        {
            Debug.Log(i);
            KeyValuePair<string, int> kvp = mybaglist[i];
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
            if (kvp.Key == item)
            {
                kvp = new KeyValuePair<string, int>(kvp.Key, kvp.Value - 1);
                Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
                mybaglist[i] = kvp;
                /*TextMeshProUGUI buttonText = bagbutton[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = kvp.Value.ToString();*/
                //Image image = bagbutton[i].GetComponent<Image>();
                if (kvp.Value == 0 && kvp.Key != "frame")
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
                    image.sprite = frame;
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
            if (image.sprite == key)
            {
                PlayerPrefs.SetInt("state", 1);
            }
            else if(image.sprite == frame)
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
