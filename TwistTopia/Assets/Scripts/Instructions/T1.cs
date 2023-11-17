using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1 : MonoBehaviour
{
    public GameObject shift;
    public GameObject f;
    
    public GameObject tip;
    private int flag = 0;
    // Start is called before the first frame update
    void Start()
    {
        f.SetActive(false);
       
        shift.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (tip.activeSelf)
        {
            flag += 1;
        }
        if (flag > 1)
        {
            shift.SetActive(true);
            
        }
    }
}
