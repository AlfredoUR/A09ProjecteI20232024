using System.Numerics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour
{
    //public static GameManager_Script Instance;

    public Text scoreText;
    public Tutorial tutorialScript;
    public int score;
    int levelIndex;
    bool isTutorial;
    public int tutorialIndex;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject gameUIPanel;
    public bool isPaused;
    public Text pauseText;
    public bool pauseTime = false;
    private bool isTutorial1;
    private bool isTutorial2;
    private bool isTutorial4;

    void Start()
    {
        score = 0;
        scoreText.text = ("Score: " + score.ToString());
        levelIndex = GetComponent<SceneChanger>().GetLevelIndex();
        isPaused = false;

        if (tutorialScript == null)
        {
            tutorialScript = FindObjectOfType<Tutorial>();
            if (tutorialScript == null)
            {
                Debug.LogError("Tutorial script not found in the scene!");
            }
        }
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (gameUIPanel != null)
        {
            gameUIPanel.SetActive(true); 
        }

    }

    void Update()
    {
        if ( !isPaused)
        {
            addScore();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            //TogglePauseGame();
        }


        //if (pauseTime)
        //{
        //    PauseGame();
        //}
        //switch (levelIndex)
        //{
        //    case 0:
        //        isTutorial = true;
        //        break;
        //    case 1:
        //        isTutorial = false;
        //        break;
        //}

        //if (isTutorial)
        //{
        //    switch (tutorialIndex)
        //    {
        //        case 0:
        //            isTutorial1 = true;
        //            break;
        //        case 1:
        //            isTutorial1 = false;
        //            isTutorial2 = true;
        //            break;
        //        case 2:
        //            isTutorial2 = false;
        //            isTutorial4 = true;
        //            break;
        //        case 3:
        //            isTutorial4 = false;
        //            isTutorial4 = true;
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
    public void TogglePauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; 
            ShowPauseOptions(true); 
        }
        else
        {
            Time.timeScale = 1.0f; 
            ShowPauseOptions(false);
        }
    }
    void ShowPauseOptions(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);


            if (show)
            {
                pauseText.text = "Game Paused";
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f; 
    }
    public void addScore()
    {
        score++;
        scoreText.text = ("Score: " + score.ToString());    
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        //pauseTime = false;
    }

    public void TriggerTutorial(int stage)
    {
        string[] tutorialLines;

        switch (stage)
        {
            case 0:
                tutorialLines = new string[] { "Tina: Ara t'ensenyar� a moure't amb la teva nova taula. De moment ella marcar� el cam� sola. (Clica a qualsevol lloc per continuar)" };
                break;
            case 1:
                tutorialLines = new string[] { "Evita les hamburgueses, et faran anar m�s lent." };
                break;
            case 2:
                tutorialLines = new string[] { "En Sergi ha preparat aquestes begudes m�giques que et poden ajudar a seguir endavant. Per exemple aquesta t'augmenta la vel�locitat." };
                break;
            case 3:
                tutorialLines = new string[] { "Un obstacle! Prem l'espai per saltar just quan el tinguis davant." };
                break;
            case 4:
                tutorialLines = new string[] { "Compte, un enemic! Per embestir-lo prem el shift esquerre un cop estigui en el teu rang. Ho sabr�s quan canv�i de color." };
                break;
            case 5:
                tutorialLines = new string[] { "Encara que sembli que no podr�s passar per aqu� no et preocupis. El poder d'aquesta llauna et far� intangible durant uns segons." };
                break;
            case 6:
                tutorialLines = new string[] { "Forats? No passa res, ho ten�em tot calculat. Amb aquesta �ltima llauna. Apreta la tecla Z  podr�s teletransportar-te a una ubicaci� superior i esquivar perills." };
                break;
            case 7:
                tutorialLines = new string[] { "I fins aqu� la guia, ara pots sortir a salvar la ciutat. " };
                break;

            //Nivell1

            case 8:
                tutorialLines = new string[] { "Ara controla tu per on passes, per desgr�cia en Walter sen's ha avan�at i ha col�locat c�pies de les llaunes on no t'afavoreixen." };
                break;
            case 9:
                tutorialLines = new string[] { "Per� recorda!\nBlanc: Velocitat extra.\nRosa: Teletransportaci� amb la tecla Z.\nTaronja: Invencibilitat" };
                    break;
            case 10:
                tutorialLines = new string[] { "Ara per desgr�cia en Walter sen's ha avan�at i ha col�locat c�pies de les llaunes on no t'afavoreixen." };
                break; 
            case 11:
                tutorialLines = new string[] { "Ara  desgr�cia en Walter sen's ha avan�at i ha col�locat c�pies de les llaunes on no t'afavoreixen." };
                break;
            default:
                tutorialLines = new string[] { "Default tutorial text" };
                break;
        }
        PauseGame();
        tutorialScript.SetTutorialText(tutorialLines);
    }
}
