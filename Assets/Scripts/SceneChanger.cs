using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Scene GameOverScene;
    public string sceneName;
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver(Scene gameOverScene)
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void LoadTutorial(Scene tutorial)
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void LoadLevel1(Scene level1)
    {
        SceneManager.LoadScene("GameMechanics");
    }

    public void MainMenu(Scene gameOverScene)
    {
        SceneManager.LoadScene("MainMenu");
    }

    public string GetCurrentSceneName()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name;
    }

    public int GetLevelIndex()
    {
        sceneName = GetCurrentSceneName();
        switch (sceneName)
        {
            case "Tutorial":
                return 0;
            case "Level1":
                return 1;
            case "Level2":
                return 2;
            case "Level3":
                return 3;
            case "GameOverScene":
            default: return -1;
        }
    }
}