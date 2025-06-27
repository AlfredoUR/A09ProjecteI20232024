using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int score;
    public float passiveIncreaseRate = 5f;
    private float elapsedTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
        {
            int increment = Mathf.FloorToInt(passiveIncreaseRate * elapsedTime);
            score += increment;
            elapsedTime = 0f;
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
    }
}
