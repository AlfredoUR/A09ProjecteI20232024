using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveDistance = 5.0f; 
    public float moveSpeed = 2.0f;    
    public bool moveHorizontally = true; 

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private bool movingToTarget = true;

    void Start()
    {
        initialPosition = transform.position;
        if (moveHorizontally)
        {
            targetPosition = new Vector2(initialPosition.x + moveDistance, initialPosition.y);
        }
        else
        {
            targetPosition = new Vector2(initialPosition.x, initialPosition.y + moveDistance);
        }

        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
        }
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        if (movingToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                movingToTarget = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
            {
                movingToTarget = true;
            }
        }
    }
}