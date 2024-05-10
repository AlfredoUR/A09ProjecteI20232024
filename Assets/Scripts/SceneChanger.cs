using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Scene GameOverScene;
  
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver(Scene gameOverScene)
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void LoadLevel1(Scene level1)
    {
        SceneManager.LoadScene("GameMechanics");
    }

    public void MainMenu(Scene gameOverScene)
    {
        SceneManager.LoadScene("MainMenu");
    }
}