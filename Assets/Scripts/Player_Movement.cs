using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private float baseSpeed = 4.0f;
    public float speed;
    private float cameraSpeed;
    public float maxSpeed = 6.0f;
    float jumpForce = 1000f;
    float gravity = 0.9f;
    bool slowing;

    public Rigidbody2D rb;
    public GameObject ground;
    public GameObject player;
    public GameObject platform;
    public GameObject platformTrigger;
    public GameObject currentPlatform;
    public GameObject gameManager;
    public GameManager_Script gameManagerScript;
    private Tutorial tutorialScript;
    public Transform cameraTransform;
   
    public int levelIndex;
    public bool isTutorial;
    bool platformFound;
    public bool canTeleport;
    public bool isGrounded;
    public bool isSprinting;
    public bool endLevel;
    public bool gameOver;
    private bool gamePaused;
    Vector2 scaleX;
    public float posX;
    public float posY;
    public bool playerScaled;
    private float minPlayerWidth = 0.5f;
    private float regularPlayerWidth = 1.0f;
    private float maxPlayerWidth = 2.0f;
    float gravityScale;

    void Start()
    {
        gameManager = GameObject.FindWithTag("Game_Manager");
        gameManagerScript = FindObjectOfType<GameManager_Script>();
        levelIndex = gameManager.GetComponent<SceneChanger>().GetLevelIndex();
        speed = baseSpeed;
        gravityScale = rb.gravityScale;
        isGrounded = false;
        endLevel = false;
        slowing = false;
        gamePaused = gameManager.GetComponent<GameManager_Script>().isPaused;
        //cameraSpeed = cameraTransform.GetComponent<CameraScript>().cameraSpeed;  

        ground = GameObject.FindWithTag("Ground");
        rb = GetComponent<Rigidbody2D>();
        isSprinting = false;

        platform = GameObject.FindWithTag("Platform");
        platformFound = false;
        canTeleport = false;

        if (gameManagerScript == null)
        {
            Debug.LogError("GameManager_Script not found in the scene!");
        }
        else
        {
            tutorialScript = gameManagerScript.GetComponent<Tutorial>();
            if (tutorialScript == null)
            {
                Debug.LogError("Tutorial script not found on GameManager");
            }
        }
    }
    void Update()
    {
        posX = rb.position.x;
        posY = rb.position.y;

        if (!gameOver && !gamePaused)
        {
            if (!endLevel)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                EndLevel();
            }
        }
        else
        {
            GameOver();
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, jumpForce * speed * gravity));
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y * gravity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                gameOver = true;
                SceneManager.LoadScene("GameOver");
                break;
            case "Goal":
                endLevel = true;
                break;
            case "Ground":
            case "Platform":
                isGrounded = true;
                break;
            case "JunkFood":
                PlayerMaxDeform();
                Destroy(collision.gameObject);
                playerScaled = false;
                break;
            case "GoodFood":
                PlayerMinDeform();
                Destroy(collision.gameObject);
                playerScaled = false;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            isGrounded = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Goal")
        {
            endLevel = true;
        }
        if (other.gameObject.tag == "PlatformTrigger")
        {
            transform.Translate(new Vector2(rb.position.x, rb.position.y + 8.0f));
        }
        if (other.gameObject.tag == "TutorialTrigger")
        {
            
            gameManagerScript.PauseGame();
            other.gameObject.GetComponent<Tutorial>().StartDialogue();
        }
    }


    private void PlayerMaxDeform()
    {
        if (!playerScaled && transform.localScale.x > minPlayerWidth)
        {
            scaleX = transform.localScale;
            scaleX.x += 0.5f;
            transform.localScale = scaleX;
            Debug.Log("Scaling");
            speed--;
        }
    }


    private void PlayerMinDeform()
    {
        gradualSlow();
        if (!playerScaled && transform.localScale.x < maxPlayerWidth)
        {
            scaleX = transform.localScale;
            if (transform.localScale.x != regularPlayerWidth)
            {
                scaleX.x = regularPlayerWidth;
                transform.localScale = scaleX;
                Debug.Log("Scaling");
                playerScaled = true;
            }

            if (speed <= maxSpeed)
            {
                speed++;
                slowing = true;
            }
        }
    }

    private void gradualSlow()
    {
        while (slowing)
        {
            Debug.Log("Gradually slowing speed ");
            Debug.Log("Speed is : " + speed);
            if (speed == baseSpeed)
            {
                slowing = false;
            }
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    private void EndLevel()
    {
        switch (levelIndex)
        {
            case 0:
                SceneManager.LoadScene("Level1");
                break;
            default:
                SceneManager.LoadScene("GameOverScene");
                break;
        }
    }
}
