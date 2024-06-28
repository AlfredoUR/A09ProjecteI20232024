using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    public float moveDistance = 5.0f;
    public float moveSpeed = 2.0f;    
    public LayerMask groundLayer;     
    public Transform groundCheck;     

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private bool movingToTarget = true;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = new Vector2(initialPosition.x + moveDistance, initialPosition.y);
    }

    void Update()
    {
        if (IsGrounded())
        {
            MoveEnemy();
        }
        else
        {
            movingToTarget = !movingToTarget;
            Flip();
        }
    }

    void MoveEnemy()
    {
        if (movingToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                movingToTarget = false;
                Flip();
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
            {
                movingToTarget = true;
                Flip();
            }
        }
    }

    bool IsGrounded()
    {    
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 1.0f, groundLayer);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
