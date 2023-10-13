using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons; 

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i;
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));

        }
    }

    void LoadLevel(int levelIndex)
    {
       
        if (levelIndex == 1)
        {
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }
        
    }
}
