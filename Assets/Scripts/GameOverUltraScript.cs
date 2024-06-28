using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUltraScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int finalScore = ScoreManager.Instance.score;
        scoreText.text = "Final Score: " + finalScore.ToString();
    }
}
