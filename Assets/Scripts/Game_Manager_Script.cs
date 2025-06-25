using System.Numerics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour
{

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
        score = 0;
        if (scoreText != null) {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("GameManager: where is scoreText.");
        }
        SceneChanger sceneChanger = GetComponent<SceneChanger>();
        if (sceneChanger != null)
        {
            levelIndex = sceneChanger.GetLevelIndex();
        }
        else
        {
            Debug.LogError("GameManager: SceneChanger no trobat!");
        }

        if (tutorialScript == null)
        {
            tutorialScript = FindObjectOfType<Tutorial>();
            if (tutorialScript == null)
                Debug.LogError("GameManager: No s'ha trobat cap Tutorial a l’escena.");
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
        else
            Debug.LogWarning("GameManager: pausePanel not assigned.");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            Debug.LogWarning("GameManager: gameOverPanel not assigned.");

        if (gameUIPanel != null)
            gameUIPanel.SetActive(true);
        else
            Debug.LogWarning("GameManager: gameUIPanel not assigned.");

        isPaused = false;
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
                tutorialLines = new string[] { "Tina: Ara t'ensenyaré a moure't amb la teva nova taula. De moment ella marcarà el camí sola. (Clica a qualsevol lloc per continuar)" };
                break;
            case 1:
                tutorialLines = new string[] { "Evita les hamburgueses, et faran anar més lent." };
                break;
            case 2:
                tutorialLines = new string[] { "En Sergi ha preparat aquestes begudes màgiques que et poden ajudar a seguir endavant. Per exemple aquesta t'augmenta la vel·locitat." };
                break;
            case 3:
                tutorialLines = new string[] { "Un obstacle! Prem l'espai per saltar just quan el tinguis davant." };
                break;
            case 4:
                tutorialLines = new string[] { "Compte, un enemic! Per embestir-lo prem el shift esquerre un cop estigui en el teu rang. Ho sabràs quan canvïi de color." };
                break;
            case 5:
                tutorialLines = new string[] { "Encara que sembli que no podràs passar per aquí no et preocupis. El poder d'aquesta llauna et farà intangible durant uns segons." };
                break;
            case 6:
                tutorialLines = new string[] { "Forats? No passa res, ho teníem tot calculat. Amb aquesta última llauna. Apreta la tecla Z  podràs teletransportar-te a una ubicació superior i esquivar perills." };
                break;
            case 7:
                tutorialLines = new string[] { "I fins aquí la guia, ara pots sortir a salvar la ciutat. " };
                break;

            //Nivell1

            case 8:
                tutorialLines = new string[] { "Ara controla tu per on passes, per desgràcia en Walter sen's ha avançat i ha col·locat còpies de les llaunes on no t'afavoreixen." };
                break;
            case 9:
                tutorialLines = new string[] { "Però recorda!\nBlanc: Velocitat extra.\nRosa: Teletransportació amb la tecla Z.\nTaronja: Invencibilitat" };
                    break;
            case 10:
                tutorialLines = new string[] { "Ara controla tu per on passes, per desgràcia en Walter sen's ha avançat i ha col·locat còpies de les llaunes on no t'afavoreixen." };
                break; 
            case 11:
                tutorialLines = new string[] { "Ara controla tu per on passes, per desgràcia en Walter sen's ha avançat i ha col·locat còpies de les llaunes on no t'afavoreixen." };
                break;
            default:
                tutorialLines = new string[] { "Default tutorial text" };
                break;
        }
        PauseGame();
        tutorialScript.SetTutorialText(tutorialLines);
    }
}
