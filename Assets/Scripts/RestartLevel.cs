using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "GameOverScene")
        {
            string lastScene = PlayerPrefs.GetString("LastScene", "Tutorial");

            if (Application.CanStreamedLevelBeLoaded(lastScene))
            {
                SceneManager.LoadScene(lastScene);
            }
            else
            {
                Debug.LogWarning("No valid scene loading Tutorial.");
                SceneManager.LoadScene("Tutorial");
            }
        }
        else
        {
            SceneManager.LoadScene(currentSceneName);
        }

        Time.timeScale = 1.0f; 
    }

}
