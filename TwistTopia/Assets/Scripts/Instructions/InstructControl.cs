using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InstructControl : MonoBehaviour
{
    //public GameObject panel;
    //public GameObject wasd;
    //public GameObject ad;
    public CameraState cameraState;
    public GameObject f;
    public GameObject level;

    public GameObject shift;
    

    public GameObject bagkey;

    private bool show = false;
    private string sceneName;
    //private FacingDirection direction;
    // Start is called before the first frame update
    void Start()
    {
        /*wasd.SetActive(false);
        ad.SetActive(true);*/


        StartCoroutine(HidePanelAfterSeconds(5f));


    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            bagkey.SetActive(true);
            f.SetActive(true);
            shift.SetActive(true);
        }
            
    }
    IEnumerator HidePanelAfterSeconds(float seconds)
    {
        bagkey.SetActive(true);
        f.SetActive(true);
        shift.SetActive(true);
        level.SetActive(true);
        yield return new WaitForSeconds(seconds);
        //bagkey.SetActive(false);
        f.SetActive(false);
        shift.SetActive(false);
        level.SetActive(false);
    }

    public void OnButtonClick()
    {
        show = !show;
        Debug.Log(show);
        if (!show)
        {
            //wasd.SetActive(false);
            //ad.SetActive(false);
            bagkey.SetActive(false);
            f.SetActive(false);
            shift.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}
