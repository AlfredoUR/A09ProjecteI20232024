using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    //Base
    private float baseSpeed = 5.0f;
    public float speed;
    private float cameraSpeed;
    public float maxSpeed = 7.0f;
    float jumpForce = 1000f;
    float gravity = 0.9f;
    bool slowing;
    //Dashing
    private bool isDashing = false;
    private float dashTime;
    public float dashSpeed = 12.0f;
    public float dashDuration = 0.5f;
    public float dashDistance = 5.5f;
    private bool canDash = false;
    private Vector2 dashStartPos;
    //Invulnerable
    private bool isInvulnerable = false;
    private float invulnerabilityEndTime;
    //Speeed +
    private bool isSpeedBoosted = false;
    private float speedBoostEndTime;

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
        isDashing = false;

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
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
            {
                StartDash();
            }

            if (isDashing)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0 || Vector2.Distance(dashStartPos, rb.position) >= dashDistance)
                {
                    EndDash();
                }

            }
        }

        if (isInvulnerable && Time.time >= invulnerabilityEndTime)
        {
            isInvulnerable = false;
        }

        if (isSpeedBoosted && Time.time >= speedBoostEndTime)
        {
            isSpeedBoosted = false;
            speed = baseSpeed;
        }
    }
    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashStartPos = rb.position;
        rb.velocity = new Vector2(dashSpeed, rb.velocity.y);
    }

    private void EndDash()
    {
        isDashing = false;
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
    public void SetCanDash(bool value)
    {
        canDash = value;
    }

    public void ActivateInvulnerability(float duration)
    {
        isInvulnerable = true;
        invulnerabilityEndTime = Time.time + duration;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
    public void ActivateSpeedBoost(float duration)
    {
        isSpeedBoosted = true;
        speedBoostEndTime = Time.time + duration;
        speed *= 2; // Augmenta la velocitat
    }
    public void GainTeleport()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
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

        if (other.gameObject.tag == "Enemy" && isDashing)
        {
            Destroy(other.gameObject);

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
