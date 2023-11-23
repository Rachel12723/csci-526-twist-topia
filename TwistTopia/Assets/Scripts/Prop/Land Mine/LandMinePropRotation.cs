using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMinePropRotation : MonoBehaviour
{
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
                body.transform.localPosition = Vector3.zero;
                down = true;
                body.transform.rotation = Quaternion.Euler(0, 0, 0);
                reset = false;
            }
            float yAngle = (body.transform.rotation.eulerAngles.y + 360f) % 360f;
            body.transform.rotation = Quaternion.Euler(0, yAngle + rotationSpeed * Time.deltaTime, 0);
            float yTranslation = translationSpeed * Time.deltaTime;
            if (down)
            {
                if (body.transform.localPosition.y - yTranslation < -0.3f)
                {
                    down = false;
                    body.transform.localPosition = new Vector3(0, body.transform.localPosition.y + yTranslation, 0);
                }
                else
                {
                    body.transform.localPosition = new Vector3(0, body.transform.localPosition.y - yTranslation, 0);
                }
            }
            else
            {
                if (body.transform.localPosition.y + yTranslation > 0f)
                {
                    down = true;
                    body.transform.localPosition = new Vector3(0, body.transform.localPosition.y - yTranslation, 0);
                }
                else
                {
                    body.transform.localPosition = new Vector3(0, body.transform.localPosition.y + yTranslation, 0);
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
