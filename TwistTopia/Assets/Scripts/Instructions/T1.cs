using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1 : MonoBehaviour
{
    public GameObject shift1;
    public GameObject shift2;
    public GameObject f1;
    public GameObject f2;
    public GameObject tip;
    private int flag = 0;
    // Start is called before the first frame update
    void Start()
    {
        f1.SetActive(false);
        f2.SetActive(false);
        shift1.SetActive(false);
        shift2.SetActive(false);
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
            shift1.SetActive(true);
            shift2.SetActive(true);
        }
    }
}
