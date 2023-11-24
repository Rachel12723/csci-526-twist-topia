using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMotion : MonoBehaviour
{
    private Transform flagTransform1;

    private Transform flagTransform2;
    // Start is called before the first frame update
    private float minY = 1.07f; 
    private float maxY = 1.1f; 
    private float speed = 0.03f;
    private Vector3 startingPosition1;
    private Vector3 startingPosition2;
    void Start()
    {
        // Access the child transforms
        if (transform != null)
        {
            // Find child1 and child2 under parentObject by name
            Transform flag1 = transform.Find("Flag1");
            Transform flag2 = transform.Find("Flag2");

            // Check if they were found
            if (flag1 != null && flag2 != null)
            {
                flagTransform1 = flag1;
                flagTransform2 = flag2;

                // 获取初始位置
                startingPosition1 = flagTransform1.localPosition;
                startingPosition2 = flagTransform2.localPosition;
                
            }
            else
            {
                Debug.LogError("One or both flag transforms not found!");
            }
        }
        else
        {
            Debug.LogError("Parent GameObject not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float newY1 = - Mathf.PingPong(Time.time * speed, maxY - minY) + maxY;

        // 设置child1的y轴位置
        if (flagTransform1 != null)
        {
            Vector3 newPosition1 = startingPosition1;
            newPosition1.y = newY1;
            flagTransform1.localPosition = newPosition1;
        }
        
        float newY2 = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;

        // 设置child1的y轴位置
        if (flagTransform2 != null)
        {
            Vector3 newPosition2 = startingPosition2;
            newPosition2.y = newY2;
            flagTransform2.localPosition = newPosition2;
        }
    }
}
