using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAnimation : MonoBehaviour
{
    public bool rotation = false;
    public bool translation = false;
    public float minY = -0.3f;
    private bool reset = true;
    private bool down = true;
    private float rotationSpeed = 60f;
    private float translationSpeed = 0.2f;
    private Transform body;
    // Start is called before the first frame update
    void Start()
    {
        body = transform.Find("Body");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (reset)
            {
                if (rotation)
                {
                    body.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (translation)
                {
                    body.transform.localPosition = new Vector3(body.transform.localPosition.x, 0, body.transform.localPosition.z);
                    down = true;
                }
                reset = false;
            }
            if (rotation)
            {
                float yAngle = (body.transform.rotation.eulerAngles.y + 360f) % 360f;
                body.transform.rotation = Quaternion.Euler(0, yAngle + rotationSpeed * Time.deltaTime, 0);
            }
            if (translation)
            {
                float yTranslation = translationSpeed * Time.deltaTime;
                if (down)
                {
                    if (body.transform.localPosition.y - yTranslation < minY)
                    {
                        down = false;
                        body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y + yTranslation, body.transform.localPosition.z);
                    }
                    else
                    {
                        body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y - yTranslation, body.transform.localPosition.z);
                    }
                }
                else
                {
                    if (body.transform.localPosition.y + yTranslation > 0f)
                    {
                        down = true;
                        body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y - yTranslation, body.transform.localPosition.z);
                    }
                    else
                    {
                        body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y + yTranslation, body.transform.localPosition.z);
                    }
                }
            }
        }
        else
        {
            if (!reset)
            {
                reset = true;
            }
        }
    }
}
