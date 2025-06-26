using System.Collections;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public Canvas tutorialCanvas;
    public GameManager_Script gameManager;
    public string[] lines;
    public float textSpeed = 0.05f;
    public int tutorialStage;
    private int index;

    void Start()
    {
        // AUTOCONFIGURACIÓ DE DEPENDÈNCIES
        if (tutorialText == null)
        {
            tutorialText = FindObjectOfType<TextMeshProUGUI>();
            if (tutorialText == null)
                Debug.LogError("Tutorial: No s'ha trobat cap TextMeshProUGUI a l'escena.");
        }

        if (tutorialCanvas == null)
        {
            tutorialCanvas = FindObjectOfType<Canvas>();
            if (tutorialCanvas == null)
                Debug.LogError("Tutorial: No s'ha trobat cap Canvas a l'escena.");
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager_Script>();
            if (gameManager == null)
                Debug.LogError("Tutorial: No s'ha trobat GameManager_Script.");
        }

        if (tutorialCanvas != null)
            tutorialCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameManager.levelIndex != 0)
        {
            if (!tutorialCanvas.gameObject.activeSelf) return;
        }
        if (lines == null || lines.Length == 0 || tutorialText == null) return;

        if (textSpeed <= 0f) textSpeed = 0.03f;


        if (Input.GetMouseButtonDown(0))
        {
            if (tutorialText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                tutorialText.text = lines[index];
            }
        }
    }

    public void SetTutorialText(string[] tutorialLines)
    {
        lines = tutorialLines;
        StartDialogue();
    }

    public void StartDialogue()
    {
        Debug.Log("🟡 StartDialogue called");

        if (lines == null || lines.Length == 0 || tutorialText == null || tutorialCanvas == null)
        {
            Debug.LogError("🔴 Error a StartDialogue(). Falten referències.");
            return;
        }

        index = 0;
        tutorialText.text = "▶️ " + lines[index]; // mostra directament el text
        tutorialCanvas.gameObject.SetActive(true);

        //if (lines == null || lines.Length == 0 || tutorialText == null || tutorialCanvas == null)
        //{
        //    Debug.LogError("Tutorial: Error a StartDialogue(). Falten referències.");
        //    return;
        //}
        //index = 0;
        //tutorialText.text = "";
        //tutorialCanvas.gameObject.SetActive(true);
        //StartCoroutine(DelayedType());
    }

    IEnumerator DelayedType()
    {
        yield return null;
        yield return StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        tutorialText.text = "";
        foreach (char c in lines[index])
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (++index < lines.Length)
        {
            tutorialText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (tutorialCanvas != null)
                tutorialCanvas.gameObject.SetActive(false);

            gameObject.SetActive(false);

            if (gameManager != null)
                gameManager.ResumeGame();
        }
    }
}
