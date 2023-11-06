using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public string sceneName;
    private int level;
    //public GameObject panel;

    void Start()
    {

        //int level;
       // StartCoroutine(ShowPanelAndLoadLevel);
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("Level", 0);
            level = 0;
        }



        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i;
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));

        }
    }

    void LoadLevel(int levelIndex)
    {
       /* if (level >= levelIndex)
        {
            if (levelIndex == 0)
            {
                SceneManager.LoadScene("Level_1");
                Time.timeScale = 1f;
            }
            else if (levelIndex == 1)
            {
                SceneManager.LoadScene("Level_2");
                Time.timeScale = 1f;
            }
            else if (levelIndex == 2)
            {
                SceneManager.LoadScene("Level_3");
                Time.timeScale = 1f;
            }
        }*/

        if (levelIndex == 0)
        {
            SceneManager.LoadScene("Tutorial_1");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 1)
        {
            SceneManager.LoadScene("Level_1");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 2)
        {
            SceneManager.LoadScene("Level_2");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 3)
        {
            SceneManager.LoadScene("Level_3");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 4)
        {
            SceneManager.LoadScene("Level_4");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 5)
        {
            SceneManager.LoadScene("Level_5");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 6)
        {
            SceneManager.LoadScene("Level_6");
            Time.timeScale = 1f;
        }


    }

   /* IEnumerator ShowPanelAndLoadLevel()
    {
        //panel.SetActive(true); 
        yield return new WaitForSeconds(3f); 

        panel.SetActive(false); 
        *//*SceneManager.LoadScene(sceneName); 
        Time.timeScale = 1f;*//* 
    }*/
}
