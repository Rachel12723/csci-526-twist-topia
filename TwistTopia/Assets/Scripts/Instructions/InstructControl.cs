using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InstructControl : MonoBehaviour
{
    public GameObject panel;
    public GameObject wasd;
    public GameObject ad;
    public CameraState cameraState;
    public GameObject f1;
    public GameObject f2;
    public GameObject shift1;
    public GameObject shift2;

    public GameObject bagkey;

    private bool show = true;
    private string sceneName;
    //private FacingDirection direction;
    // Start is called before the first frame update
    void Start()
    {
        /*wasd.SetActive(false);
        ad.SetActive(true);*/


        StartCoroutine(HidePanelAfterSeconds(1.5f));


    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                wasd.SetActive(false);
                ad.SetActive(true);
            }
            else
            {
                wasd.SetActive(true);
                ad.SetActive(false);
            }
            bagkey.SetActive(true);
        }
            
    }
    IEnumerator HidePanelAfterSeconds(float seconds)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(seconds); 
        panel.SetActive(false); 
    }

    public void OnButtonClick()
    {
        show = !show;
        Debug.Log(show);
        if (!show)
        {
            wasd.SetActive(false);
            ad.SetActive(false);
            bagkey.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}
