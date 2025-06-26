
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour
{
    public Text scoreText;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject gameUIPanel;
    public Text pauseText;

    public Tutorial tutorialScript;

    public int score;
    public bool isPaused;
    public bool pauseTime = false;

    private int levelIndex;
    private bool isTutorial;
    public int tutorialIndex;
    public static string lastSceneBeforeGameOver;

    void Start()
    {
        InitializeGame();
        SetupUI();
        FindDependencies();
    }

    void Update()
    {
        if (!isPaused)
        {
            AddScore(); 
        }

        HandleInput();

        if (pauseTime)
        {
            PauseGame();
            pauseTime = false; 
        }
    }
    private void InitializeGame()
    {
        score = 0;
        isPaused = false;

        SceneChanger sceneChanger = GetComponent<SceneChanger>();
        if (sceneChanger != null)
        {
            levelIndex = sceneChanger.GetLevelIndex();
            isTutorial = (levelIndex == 0);
        }
        else
        {
            Debug.LogError("GameManager: SceneChanger component not found!");
            levelIndex = -1;
        }
    }

    private void SetupUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("GameManager: scoreText not assigned in inspector.");
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
        else
            Debug.LogWarning("GameManager: pausePanel not assigned in inspector.");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            Debug.LogWarning("GameManager: gameOverPanel not assigned in inspector.");

        if (gameUIPanel != null)
            gameUIPanel.SetActive(true);
        else
            Debug.LogWarning("GameManager: gameUIPanel not assigned in inspector.");
    }

    private void FindDependencies()
    {
        if (tutorialScript == null)
        {
            tutorialScript = FindObjectOfType<Tutorial>();
            if (tutorialScript == null && isTutorial)
            {
                Debug.LogError("GameManager: No Tutorial script found in tutorial level.");
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    public void TogglePauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            ShowPauseOptions(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            ShowPauseOptions(false);
        }
    }

    private void ShowPauseOptions(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);

            if (show && pauseText != null)
            {
                pauseText.text = "Game Paused";
            }
        }
    }

    public void AddScore()
    {
        score++;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.score = score; 
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;

    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    public void TriggerTutorial(int stage)
    {
        if (tutorialScript == null)
        {
            Debug.LogError("GameManager: Cannot trigger tutorial - Tutorial script is null!");
            return;
        }

        tutorialIndex = stage;
        string[] tutorialLines = GetTutorialLines(stage);

        PauseGame();
        tutorialScript.SetTutorialText(tutorialLines);
    }

    private string[] GetTutorialLines(int stage)
    {
        switch (stage)
        {
            case 0:
                return new string[] { "Tina: Ara t'ensenyaré a moure't amb la teva nova taula. De moment ella marcarà el camí sola. (Clica a qualsevol lloc per continuar)" };
            case 1:
                return new string[] { "Evita les hamburgueses, et faran anar més lent." };
            case 2:
                return new string[] { "En Sergi ha preparat aquestes begudes màgiques que et poden ajudar a seguir endavant. Per exemple aquesta t'augmenta la vel·locitat." };
            case 3:
                return new string[] { "Un obstacle! Prem l'espai per saltar just quan el tinguis davant. Quan més mantinguis la tecla més saltaràs" };
            case 4:
                return new string[] { "Compte, un enemic! Per embestir-lo prem el shift esquerre un cop estigui en el teu rang. Ho sabràs quan canvïi de color." };
            case 5:
                return new string[] { "Encara que sembli que no podràs passar per aquí, no et preocupis. El poder d'aquesta llauna et farà intangible durant uns segons." };
            case 6:
                return new string[] { "Forats? No passa res, ho teníem tot calculat. Amb aquesta última llauna. Apreta la tecla Z podràs teletransportar-te a una ubicació superior i esquivar perills." };
            case 7:
                return new string[] { "I fins aquí la guia, ara pots sortir a salvar la ciutat." };
            case 8:
                return new string[] { "Ara controla tu per on passes, per desgràcia en Walter sen's ha avançat i ha col·locat còpies de les llaunes on no t'afavoreixen." };
            case 9:
                return new string[] { "Però recorda!\nBlanc: Velocitat extra.\nRosa: Teletransportació amb la tecla Z.\nTaronja: Invencibilitat" };
            case 10:
            case 11:
                return new string[] { "En Walter sen's ha avançat i ha col·locat 2 còpies de les llaunes on no t'afavoreixen." };
            default:
                return new string[] { "Default tutorial text" };
        }
    }

    // Método para obtener información del nivel actual
    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public bool IsTutorial()
    {
        return isTutorial;
    }
    public bool IsGamePaused()
    {
        return isPaused;
    }
}