using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

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
        if (lines == null || lines.Length == 0 || tutorialText == null) return;

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
        if (lines == null || lines.Length == 0 || tutorialText == null || tutorialCanvas == null)
        {
            Debug.LogError("Tutorial: Error a StartDialogue(). Falten referències.");
            return;
        }

        index = 0;
        tutorialText.text = string.Empty;
        tutorialCanvas.gameObject.SetActive(true);
        StartCoroutine(TypeLine());
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
