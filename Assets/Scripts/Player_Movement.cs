using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    //Base
    private float baseSpeed = 5.0f;
    public float speed;
    private float previousSpeed;
    private float cameraSpeed;
    public float maxSpeed = 7.0f;
    float jumpForce = 1000f;
    float gravity = 0.9f;
    bool slowing;
    //Dashing
    private bool isDashing = false;
    private float dashTime;
    public float dashSpeed = 20.0f;
    public float dashDuration = 0.9f;
    public float dashDistance = 20.0f;
    private bool canDash = false;
    private Vector2 dashStartPos;
    //Invulnerable
    private bool isInvulnerable = false;
    private float invulnerabilityEndTime;
    //Speeed +
    private bool isSpeedBoosted = false;
    private float speedBoostEndTime;
    public float boostSpeed = 7.0f;
    //Teleport
    private bool canTeleport = false;
    private float teleportX;
    private float teleportY;
    private float teleportEndTime;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
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
        previousSpeed = speed;
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

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

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


        if (!gamePaused)
        {
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
                if (Input.GetKeyUp(KeyCode.LeftShift) && !isDashing && canDash)
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
                SetColor(originalColor);
            }

            if (isSpeedBoosted && Time.time >= speedBoostEndTime)
            {
                isSpeedBoosted = false;
                speed = baseSpeed;
                SetColor(originalColor);
            }

            if (canTeleport && Time.time >= teleportEndTime)
            {
                canTeleport = false;
                SetColor(originalColor);
            }

            if (canTeleport && Input.GetKeyDown(KeyCode.Z))
            {
                Teleport();
            }
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
        previousSpeed = speed;
        speed *= 2.5f;
        isDashing = true;
        dashTime = dashDuration;
        dashStartPos = rb.position;
        rb.velocity = new Vector2(dashSpeed, rb.velocity.y);
    }

    private void EndDash()
    {
        speed = previousSpeed;
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
        SetColor(Color.yellow);
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
    public void ActivateSpeedBoost(float boost, float duration)
    {
        if (transform.localScale.x != regularPlayerWidth)
        {
            scaleX.x = regularPlayerWidth;
            //transform.localScale = scaleX;
        }
        boostSpeed = boost;
        //duration = 2.0f;
        isSpeedBoosted = true;
        speed = boostSpeed;
        speedBoostEndTime = Time.time + duration;

        SetColor(Color.cyan);
        
    }
    public void ActivateTeleport(float xTeleport, float yTeleport, float duration)
    {
        canTeleport = true;
        teleportX = xTeleport;
        teleportY = yTeleport;
        teleportEndTime = Time.time + duration;
        SetColor(Color.magenta);
    }
    public void Teleport()
    {
        transform.Translate(new Vector2(0.0f+ teleportX, 0.0f+ teleportY));
        canTeleport = false;
        SetColor(originalColor);
    }

    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                if (!isInvulnerable)
                {
                    gameOver = true;
                    GameOver();
                }
                else
                {
                    Destroy(collision.gameObject);
                }
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
        switch (other.gameObject.tag)
        {
            case "PlatformTrigger":
                transform.Translate(new Vector2(rb.position.x, rb.position.y + 8.0f));
                break;
            case "Goal":
                endLevel = true;
                break;
            case "TutorialTrigger":
                gameManagerScript.PauseGame();
                other.gameObject.GetComponent<Tutorial>().StartDialogue();
                break;
            case "Enemy":
            case "Obstacle":
                if (isDashing || isInvulnerable)
                {
                    Destroy(other.gameObject);
                }
                else
                {
                    gameOver = true;
                }
                break;
            case "GoodFood":
                PlayerMinDeform();
                Destroy(other.gameObject);
                playerScaled = false;
                break;
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
            case 1:
                SceneManager.LoadScene("MainMenu");
                break;
            case 2:
                SceneManager.LoadScene("Level3");
                break;
            default:
                SceneManager.LoadScene("GameOverScene");
                break;
        }
    }
}
