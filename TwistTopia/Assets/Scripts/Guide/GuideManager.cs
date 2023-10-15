using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{

    public KeyCode continueKeyCode;
    private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        panel = transform.Find("Guide Panel").gameObject;
        panel.SetActive(false);
        for(int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(false);
        }
        panel.transform.Find("Continue").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(continueKeyCode))
        {
            for (int i = 0; i < panel.transform.childCount; i++)
            {
                Transform child = panel.transform.GetChild(i);
                if (child.gameObject.activeSelf&& child.name != "Continue")
                {
                    child.gameObject.SetActive(false);
                }
            }
            panel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
