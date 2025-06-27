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

    public bool isPaused;
    public bool pauseTime = false;
    public int tutorialIndex;

    public bool debugMode = false;

    private int levelIndex;
    private bool isTutorial;
    private SceneChanger sceneChanger;
    public static string lastSceneBeforeGameOver;

    void Awake()
    {

        sceneChanger = GetComponent<SceneChanger>();
        if (sceneChanger == null)
        {
            Debug.LogError("GameManager: SceneChanger component not found!");
        }
    }

    void Start()
    {
        InitializeGame();
        SetupUI();
        FindDependencies();

        if (isTutorial && tutorialScript != null)
        {
            Invoke(nameof(StartTutorial), 0.1f);
        }
    }

    void Update()
    {
        UpdateUI();
        HandleInput();
        HandlePauseTime();
    }

    private void StartTutorial()
    {
        if (tutorialScript != null)
        {
            TriggerTutorial(0);
        }
        else
        {
            Debug.LogError("GameManager: Cannot start tutorial - Tutorial script is null!");
        }
    }

    private void InitializeGame()
    {
        isPaused = false;

        if (sceneChanger != null)
        {
            levelIndex = sceneChanger.GetLevelIndex();
            isTutorial = (levelIndex == 0);
        }
        else
        {
            levelIndex = -1;
            Debug.LogError("GameManager: Cannot determine level index!");
        }

        if (debugMode)
            Debug.Log($"GameManager: Initialized - Level: {levelIndex}, IsTutorial: {isTutorial}");
    }

    private void SetupUI()
    {
        SetPanelActive(pausePanel, false, "pausePanel");
        SetPanelActive(gameOverPanel, false, "gameOverPanel");
        SetPanelActive(gameUIPanel, true, "gameUIPanel");
    }

    private void SetPanelActive(GameObject panel, bool active, string panelName)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
        else if (debugMode)
        {
            Debug.LogWarning($"GameManager: {panelName} not assigned in inspector.");
        }
    }

    private void FindDependencies()
    {
        if (tutorialScript == null)
        {
            tutorialScript = GetComponent<Tutorial>();

            if (tutorialScript == null)
            {
                tutorialScript = FindObjectOfType<Tutorial>();
            }

            if (tutorialScript == null && isTutorial)
            {
                Debug.LogError("GameManager: No Tutorial script found in tutorial level.");
            }
        }

        if (debugMode && tutorialScript != null)
            Debug.Log($"GameManager: Tutorial script found: {tutorialScript.name}");
    }

    private void UpdateUI()
    {
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.score.ToString();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    private void HandlePauseTime()
    {
        if (pauseTime)
        {
            PauseGame();
            pauseTime = false;
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
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(1);
            if (scoreText != null)
            {
                scoreText.text = "Score: " + ScoreManager.Instance.score.ToString();
            }
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

            tutorialScript = FindObjectOfType<Tutorial>();

            if (tutorialScript == null)
            {
                Debug.LogError("GameManager: Still cannot find Tutorial script!");
                return;
            }
        }

        tutorialIndex = stage;
        string[] tutorialLines = GetTutorialLines(stage);

        if (tutorialLines == null || tutorialLines.Length == 0)
        {
            Debug.LogError($"GameManager: No tutorial lines found for stage {stage}");
            return;
        }

        PauseGame();
        tutorialScript.SetTutorialText(tutorialLines);

        if (debugMode)
            Debug.Log($"GameManager: Triggered tutorial stage {stage}");
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
                Debug.LogWarning($"GameManager: No tutorial text defined for stage {stage}");
                return new string[] { "Default tutorial text" };
        }
    }

    public int GetLevelIndex() => levelIndex;
    public bool IsTutorial() => isTutorial;
    public bool IsGamePaused() => isPaused;
}