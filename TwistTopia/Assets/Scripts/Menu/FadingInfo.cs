using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadingInfo : MonoBehaviour
{
    public float showTime = 2f;
    private float showLeft = 0f;
    public float fadingTime = 1f;
    private bool isShowed;
    private TextMeshProUGUI textMeshPro;
    private float r;
    private float g;
    private float b;
    private float a;
    // Start is called before the first frame update
    void Start()
    {
        isShowed = false;
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        r = textMeshPro.color.r;
        g = textMeshPro.color.g;
        b = textMeshPro.color.b;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowed)
        {
            textMeshPro.color = new Color(r, g, b, 1f);
            showLeft = showTime;
            isShowed = false;
        }
        if (showLeft > 0f)
        {
            showLeft -= Time.deltaTime;
        }
        else if (textMeshPro.color.a > 0)
        {
            textMeshPro.color = new Color(r, g, b, textMeshPro.color.a - 1 / fadingTime * Time.deltaTime);
        }
    }

    public void SetIsShowed(bool isShowed)
    {
        this.isShowed = isShowed;
    }
}
