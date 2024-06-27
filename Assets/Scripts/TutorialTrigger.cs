using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TutorialTrigger : MonoBehaviour
{
    public int tutorialStage; //Fase
    public GameManager_Script gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Pausa el joc i mostrar el tutorial
            gameManager.TriggerTutorial(tutorialStage);
        }
    }
}


