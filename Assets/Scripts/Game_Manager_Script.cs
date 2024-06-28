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

   public void addScore()
    {
        score++;
        scoreText.text = ("Score: " + score.ToString());    
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
                tutorialLines = new string[] { "Forats? No passa res, ho teníem tot calculat. Amb aquesta última llauna podràs teletransportar-te a una ubicació superior i esquivar perills." };
                break;
            case 7:
                tutorialLines = new string[] { "I fins aquí la guia, ara pots sortir a salvar la ciutat. " };
                break;
            default:
                tutorialLines = new string[] { "Default tutorial text" };
                break;
        }
        PauseGame();
        tutorialScript.SetTutorialText(tutorialLines);
    }
}
