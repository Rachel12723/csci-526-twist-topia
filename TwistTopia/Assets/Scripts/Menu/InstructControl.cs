using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructControl : MonoBehaviour
{
    public GameObject panel;
    public GameObject wasd;
    public GameObject ad;
    public CameraState cameraState;
    //private FacingDirection direction;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HidePanelAfterSeconds(1.5f));
    }

    // Update is called once per frame
    void Update()
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
            
    }
    IEnumerator HidePanelAfterSeconds(float seconds)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(seconds); 
        panel.SetActive(false); 
    }
}
