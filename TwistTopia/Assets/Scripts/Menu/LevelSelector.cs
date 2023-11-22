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
            SceneManager.LoadScene("Level_0(Tutorial)_1");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 1)
        {
            SceneManager.LoadScene("Level_0(Tutorial)_2");
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
            /*SceneManager.LoadScene("Level_6");
            Time.timeScale = 1f;*/
        }
        else if (levelIndex == 7)
        {
            SceneManager.LoadScene("Level_7");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 8)
        {
            SceneManager.LoadScene("Level_8");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 9)
        {
          /*  SceneManager.LoadScene("Level_9");
            Time.timeScale = 1f;*/
        }
        else if (levelIndex == 10)
        {
            SceneManager.LoadScene("Level_10");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 11)
        {
            SceneManager.LoadScene("Level_11");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 12)
        {
            SceneManager.LoadScene("Level_12");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 13)
        {
            /*SceneManager.LoadScene("Level_13");
            Time.timeScale = 1f;*/
        }
        else if (levelIndex == 14)
        {
            SceneManager.LoadScene("Level_14");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 15)
        {
            /*SceneManager.LoadScene("Level_15");
            Time.timeScale = 1f;*/
        }
        else
        {
            SceneManager.LoadScene("Main_Menu");
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
