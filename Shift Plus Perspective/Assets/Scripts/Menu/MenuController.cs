using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; 

    void Start()
    {
        
        menuPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        
        menuPanel.SetActive(!menuPanel.activeSelf);
    }
}
