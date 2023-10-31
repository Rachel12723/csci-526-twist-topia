using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructControl : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HidePanelAfterSeconds(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator HidePanelAfterSeconds(float seconds)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(seconds); 
        panel.SetActive(false); 
    }
}
