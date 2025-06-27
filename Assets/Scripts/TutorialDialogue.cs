using System.Collections;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public Canvas tutorialCanvas;

    public float textSpeed = 0.05f;

    public bool debugMode = false;

    private GameManager_Script gameManager;
    private string[] lines;
    private int index;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    void Awake()
    {
        InitializeReferences();
    }

    void Start()
    {
        ValidateComponents();

        if (tutorialCanvas != null)
            tutorialCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isDialogueActive || lines == null || lines.Length == 0 || tutorialText == null)
            return;

        HandleInput();
    }

    private void InitializeReferences()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager_Script>();

        if (tutorialText == null)
        {
            tutorialText = GetComponentInChildren<TextMeshProUGUI>();

            if (tutorialText == null)
                tutorialText = FindObjectOfType<TextMeshProUGUI>();
        }

        if (tutorialCanvas == null)
        {
            tutorialCanvas = GetComponentInParent<Canvas>();

            if (tutorialCanvas == null)
                tutorialCanvas = FindObjectOfType<Canvas>();
        }
    }

    private void ValidateComponents()
    {
        if (debugMode)
        {
            if (tutorialText == null)
                Debug.LogError($"Tutorial ({gameObject.name}): tutorialText is null");

            if (tutorialCanvas == null)
                Debug.LogError($"Tutorial ({gameObject.name}): tutorialCanvas is null");

            if (gameManager == null)
                Debug.LogError($"Tutorial ({gameObject.name}): gameManager is null");
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                tutorialText.text = lines[index];
                isTyping = false;
            }
            else if (tutorialText.text == lines[index])
            {
                NextLine();
            }
        }
    }

    public void SetTutorialText(string[] tutorialLines)
    {
        if (tutorialLines == null || tutorialLines.Length == 0)
        {
            if (debugMode)
                Debug.LogWarning("Tutorial: Received null or empty tutorial lines");
            return;
        }

        lines = tutorialLines;

        if (tutorialText == null || tutorialCanvas == null)
        {
            InitializeReferences();
        }

        StartDialogue();
    }

    public void StartDialogue()
    {
        if (!ValidateDialogueStart())
            return;

        index = 0;
        isDialogueActive = true;
        tutorialText.text = string.Empty;
        tutorialCanvas.gameObject.SetActive(true);

        StartCoroutine(TypeLine());
    }

    private bool ValidateDialogueStart()
    {
        if (lines == null || lines.Length == 0)
        {
            if (debugMode)
                Debug.LogError("Tutorial: No tutorial lines available");
            return false;
        }

        if (tutorialText == null)
        {
            Debug.LogError("Tutorial: tutorialText is null. Cannot start dialogue.");
            return false;
        }

        if (tutorialCanvas == null)
        {
            Debug.LogError("Tutorial: tutorialCanvas is null. Cannot start dialogue.");
            return false;
        }

        return true;
    }

    IEnumerator TypeLine()
    {
        if (index >= lines.Length || tutorialText == null)
            yield break;

        isTyping = true;
        tutorialText.text = "";

        string currentLine = lines[index];
        foreach (char c in currentLine)
        {
            if (tutorialText == null) 
                yield break;

            tutorialText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            tutorialText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;

        if (tutorialCanvas != null)
            tutorialCanvas.gameObject.SetActive(false);

        if (gameManager != null)
        {
            gameManager.ResumeGame();
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        if (debugMode)
            Debug.Log("Tutorial dialogue ended");
    }

    public void ForceEndDialogue()
    {
        StopAllCoroutines();
        EndDialogue();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}