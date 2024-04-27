using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager_Script : MonoBehaviour
{

    public Text scoreText;
    //public Canvas scoreCanvas;
    public int score;

    //score = 0;
    // Start is called before the first frame update
    void Start()
    {
        //scoreCanvas = Canvas.FindWithTag;
        //scoreText = scoreCanvas.GetComponent<Text>();
        score = 0;
        scoreText.text = ("Score: "+score.ToString());

    }
    void Update()
    {
        addScore();
        //if (secondTimer >= 1f)
        //{
            
        //}
    }

    private void addScore()
    {
        score++;
        scoreText.text = score.ToString();
    }


    public void enemySpawner()
    {
       // GameObject[] enemies = GameObject.("Enemy");
    }
}
