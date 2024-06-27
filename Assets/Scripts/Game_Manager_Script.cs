using UnityEngine;
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
    public bool isPaused;
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
    }

    void Update()
    {
        if ( !isPaused)
        {
            addScore();
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

    private void addScore()
    {
        if (!isPaused)
        {
            score++;
            scoreText.text = ("Score: " + score.ToString());
        }
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
                tutorialLines = new string[] { "Tina: Ara t'ensenyarï¿½ a moure't amb la teva nova taula. De moment ella marcarï¿½ el camï¿½ sola. (Clica a qualsevol lloc per continuar)" };
                break;
            case 1:
                tutorialLines = new string[] { "Evita les hamburgueses, et faran anar mï¿½s lent.(Clica a qualsevol lloc per continuar)" };
                break;
            case 2:
                tutorialLines = new string[] { "En Sergi ha preparat aquestes begudes mï¿½giques et poden ajudar a seguir endavant. Per exemple aquesta t'augmenta la velï¿½locitat." };
                break;
            case 3:
                tutorialLines = new string[] { "Tina: Un obstacle! Prem l'espai per saltar just quan el tinguis davant" };
                break;
            case 4:
                tutorialLines = new string[] { "Tina: Compte, un enemic! Per embestir-lo prem el shift esquerre un cop estigui en el teu rang. Ho sabràs quan canvïi de color" };
                break;
            default:
                tutorialLines = new string[] { "Default tutorial text" };
                break;
        }
        PauseGame();
        tutorialScript.SetTutorialText(tutorialLines);
    }
}
