using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialStage = 0;

    public GameManager_Script gameManager;

    public bool triggerOnce = true;
    public bool debugMode = false;

    private bool hasTriggered = false;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager_Script>();

            if (gameManager == null)
            {
                Debug.LogError($"TutorialTrigger ({gameObject.name}): GameManager_Script not found in scene!");
            }
        }

        if (debugMode)
            Debug.Log($"TutorialTrigger ({gameObject.name}): Initialized for stage {tutorialStage}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggerOnce && hasTriggered)
            return;

        if (gameManager == null)
        {
            Debug.LogError($"TutorialTrigger ({gameObject.name}): Cannot trigger tutorial - GameManager is null!");
            return;
        }

        TriggerTutorial();
    }

    private void TriggerTutorial()
    {
        if (debugMode)
            Debug.Log($"TutorialTrigger ({gameObject.name}): Triggering tutorial stage {tutorialStage}");

        hasTriggered = true;
        gameManager.TriggerTutorial(tutorialStage);

        if (triggerOnce)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
        GetComponent<Collider2D>().enabled = true;

        if (debugMode)
            Debug.Log($"TutorialTrigger ({gameObject.name}): Reset");
    }

    [ContextMenu("Trigger Tutorial Manually")]
    public void TriggerManually()
    {
        if (gameManager != null)
        {
            TriggerTutorial();
        }
        else
        {
            Debug.LogError("Cannot trigger manually - GameManager is null!");
        }
    }
}