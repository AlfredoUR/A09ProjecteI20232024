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
    public float textSpeed;
    public int tutorialStage;
    private int index;

    public string zeroTutorial;
    public string firstTutorial;
    public string secondTutorial;
    public string thirdTutorial;
    public string fourthTutorial;
    // Start is called before the first frame update
    void Start()
    {
        tutorialCanvas.gameObject.SetActive(false);
        //index = 0;
        //zeroTutorial = "This is the first stage of the Tutorial, here you will learn how to move";
        //firstTutorial = "Burgers and junk food will make you go slower";
        //secondTutorial = "This is the secondTutorial";
        //thirdTutorial = "This is the third Tutorial";


        //switch (tutorialStage)
        //{
        //    case 0:
        //       tutorialText.text = zeroTutorial;
        //        StartDialogue();
        //        break;
        //    case 1:
        //        tutorialText.text = firstTutorial; 
        //        StartDialogue(); 
        //        break;
        //    case 2:
        //        tutorialText.text = secondTutorial; 
        //        StartDialogue(); 
        //        break;
        //    case 3:
        //        tutorialText.text = thirdTutorial;
        //        StartDialogue();
        //        break;
        //    case 4:
        //        tutorialText.text = fourthTutorial;
        //        StartDialogue();
        //        break;
        //    default:
        //        break;

        //}
        //tutorialText.text = string.Empty;
        //StartDialogue();
    }

    public void SetTutorialText(string[] tutorialLines)
    {
        lines = tutorialLines;
        StartDialogue();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
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

    public void StartDialogue()
    {
        index = 0;
        //tutorialText.text = zeroTutorial;
        tutorialText.text = string.Empty;
        tutorialCanvas.gameObject.SetActive(true);
        StartCoroutine(Typeline());
    }

    IEnumerator Typeline()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            tutorialText.text = string.Empty;
            StartCoroutine(Typeline());
        }
        else
        {
            gameObject.SetActive(false);
            tutorialCanvas.gameObject.SetActive(false);
            gameManager.ResumeGame();
        }
    }
}
