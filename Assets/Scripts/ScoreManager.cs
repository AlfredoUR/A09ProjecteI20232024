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

            score = 0;
            elapsedTime = 0f;

            Debug.Log("ScoreManager: Instance created and initialized");
        }
        else
        {
            Debug.Log("ScoreManager: Duplicate instance destroyed");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Instance == this)
        {
            Debug.Log($"ScoreManager: Started with score: {score}");
        }
    }

    private void Update()
    {
        if (Instance != this) return;

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
        if (Instance == this)
        {
            score += amount;
            Debug.Log($"ScoreManager: Added {amount} points. Total score: {score}");
        }
        else
        {
            Debug.LogWarning("ScoreManager: Attempting to add score from non-active instance!");
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        elapsedTime = 0f;
        Debug.Log("ScoreManager: Score reset to 0");
    }

    public static bool IsInstanceActive()
    {
        return Instance != null;
    }

    public static int GetCurrentScore()
    {
        return Instance != null ? Instance.score : 0;
    }
}