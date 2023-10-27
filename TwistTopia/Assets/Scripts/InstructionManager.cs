using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public Transform playerTransform;
    public float displayDistance = 5.0f;

    private GameObject[] instructionPanels;  // An array to hold references to the GameObjects of each instruction panel

    private void Start()
    {
        // Populate the instructionPanels array with the GameObjects of the children
        int childCount = transform.childCount;
        instructionPanels = new GameObject[childCount];
        
        for (int i = 0; i < childCount; i++)
        {
            instructionPanels[i] = transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        foreach (GameObject panel in instructionPanels)
        {
            float distance = Vector3.Distance(panel.transform.position, playerTransform.position);
            
            if (distance <= displayDistance)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
}