using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public Color originalColor;
    public Color alertColor;
    public float detectionRange = 5.0f;
    private GameObject player;
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            playerMovement.SetCanDash(true);
            spriteRenderer.color = alertColor;
        }
        else
        {
            playerMovement.SetCanDash(false);
            spriteRenderer.color = originalColor;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerMovement != null)
        {
            playerMovement.SetCanDash(true);
            spriteRenderer.color = Color.red;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerMovement != null)
        {
            playerMovement.SetCanDash(false);
            spriteRenderer.color = Color.white;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}