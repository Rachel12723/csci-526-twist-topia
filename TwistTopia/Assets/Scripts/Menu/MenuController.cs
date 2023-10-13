using UnityEngine;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    //private bool allowInput = true;
    void Start()
    {
        
        menuPanel.SetActive(false);
    }

    /*void Update()
    {
        if (!allowInput)
        {
            return;
        }
    }*/
    public void ToggleMenu()
    {
        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            Time.timeScale = 1f;
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
