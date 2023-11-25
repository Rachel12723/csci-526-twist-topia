using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject levelPanel;
    public GameObject instructionsPanel;
    public GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        mainPanel.SetActive(true);
        levelPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        levelPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowLevelPanel()
    {
        mainPanel.SetActive(false);
        levelPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowInstructionsPanel()
    {
        mainPanel.SetActive(false);
        levelPanel.SetActive(false);
        instructionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void ShowCreditsPanel()
    {
        mainPanel.SetActive(false);
        levelPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void LoadScene(int levelId)
    {
        SceneManager.LoadScene("Level_"+levelId);
    }
}
