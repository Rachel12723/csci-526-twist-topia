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
        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            Time.timeScale = 1f;
        }
        else
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            Time.timeScale = 0f;
        }
    }
}
