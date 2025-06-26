using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Color originalColor;
    public Color alertColor;
    public float detectionRange = 5.0f;

    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private bool isDetectingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            isDetectingPlayer = true;
            spriteRenderer.color = alertColor;
        }
        else
        {
            isDetectingPlayer = false;
            spriteRenderer.color = originalColor;
        }
    }

    public bool IsDetectingPlayer()
    {
        return isDetectingPlayer;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDetectingPlayer = true;
            spriteRenderer.color = alertColor;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDetectingPlayer = false;
            spriteRenderer.color = originalColor;
        }
    }
}