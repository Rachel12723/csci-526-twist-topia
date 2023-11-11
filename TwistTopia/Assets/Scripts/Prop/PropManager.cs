using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PropManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI bagtext;
    //public GameObject bagtext;
    string [] bagArray = new string[3];
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("state", 0);//ｻｺｴ譽ｿ
        //0:circuit; 1:keypon; 2:frame
        PlayerPrefs.SetInt("Circuit", 1);
        PlayerPrefs.SetInt("Keypon", 0);
        PlayerPrefs.SetInt("Frame", 0);
        bagArray[0] = "Circuit";
        bagArray[1] = "Keypon";
        bagArray[2] = "Frame";
        bagtext.color = Color.green;

    }

    // Update is called once per frame
    void Update()
    {
        int state = PlayerPrefs.GetInt("state");
        int num = PlayerPrefs.GetInt(bagArray[state]);
        if (num > 0)
        {
            bagtext.color = Color.green;
        }
        else
        {
            bagtext.color = Color.red;
        }

    }

    public void ChangeItem()
    {
        Debug.Log("click!");
        int itemidx = (PlayerPrefs.GetInt("state") + 1) % 3;
        bagtext.text = bagArray[itemidx];
        PlayerPrefs.SetInt("state", itemidx);
        int itemnum = PlayerPrefs.GetInt(bagtext.text);
        if (itemnum > 0)
        {
            bagtext.color = Color.green;
        }
        else
        {
            bagtext.color = Color.red;
        }
        EventSystem.current.SetSelectedGameObject(null);

    }
}
