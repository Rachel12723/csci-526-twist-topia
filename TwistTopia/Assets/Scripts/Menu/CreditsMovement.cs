using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMovement : MonoBehaviour
{
    public float speed = 10f;
    public float maxZ = 0f;
    public List<Transform> credits;
    private List<Vector3> original;
    // Start is called before the first frame update
    void Start()
    {
        original = new List<Vector3>();
        foreach(Transform platform in credits)
        {
            original.Add(platform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            ShowCredits();
        }
    }

    private void ShowCredits()
    {
        bool isShowing = false;
        foreach (Transform platform in credits)
        {
            if (platform.position.z < maxZ)
            {
                isShowing = true;
                if (!platform.gameObject.activeSelf)
                {
                    platform.gameObject.SetActive(true);
                }
            }
            else
            {
                if (platform.gameObject.activeSelf)
                {
                    platform.gameObject.SetActive(false);
                }
            }
            platform.position = new Vector3(platform.position.x, platform.position.y, platform.position.z + speed * Time.deltaTime);
        }
        if (!isShowing)
        {
            transform.parent.gameObject.GetComponent<MainMenuController>().ShowMainPanel();
            for(int i = 0; i < credits.Count; i++)
            {
                credits[i].position = original[i];
            }
        }
    }
}
