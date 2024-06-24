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

    public void LoadLevel1(Scene tutorial)
    {
        SceneManager.LoadScene("GameMechanics");
    }

    public void MainMenu(Scene gameOverScene)
    {
        SceneManager.LoadScene("MainMenu");
    }

    public string GetCurrentSceneName()
    {
        // Obtenim l'escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        // Retornem el nom de l'escena actual
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