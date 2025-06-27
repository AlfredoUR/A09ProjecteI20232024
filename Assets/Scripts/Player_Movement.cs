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

    //Jump
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f; 
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f; 

    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    private bool wasGroundedLastFrame;
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
    public float boostSpeed = 15.0f;
    private float speedBeforeBoost;
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
        if (gameManager == null)
        {
            gameManager = GameObject.FindWithTag("Game_Manager");
        }

        if (gameManagerScript == null && gameManager != null)
        {
            gameManagerScript = gameManager.GetComponent<GameManager_Script>();
        }

        if (cameraTransform == null) { 
            cameraTransform = Camera.main != null ? Camera.main.transform : null;
        }

        levelIndex = gameManager.GetComponent<SceneChanger>().GetLevelIndex();
        speed = baseSpeed;
        previousSpeed = speed;


        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        if (gravityScale == 0) gravityScale = 1f;

        isGrounded = false;
        endLevel = false;
        isJumping = false;
        gamePaused = gameManagerScript.IsGamePaused();

        ground = GameObject.FindWithTag("Ground");
        isDashing = false;

        platform = GameObject.FindWithTag("Platform");
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
                tutorialScript = FindObjectOfType<Tutorial>();
            }
        }
        levelIndex = gameManagerScript != null ? gameManagerScript.GetComponent<SceneChanger>()?.GetLevelIndex() ?? -1 : -1;
    }
    void Update()
    {
        posX = rb.position.x;
        posY = rb.position.y;

        CheckEnemyDetection();

        if (!gameOver && !gamePaused)
        {
            if (!endLevel)
            {
                HandleJump();
                HandleDash();
                HandlePowerUps();
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

        wasGroundedLastFrame = isGrounded;

    }
    void FixedUpdate()
    {
        if (!gameOver && !gamePaused && !endLevel)
        {
            if (!isDashing)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }

            ApplyBetterGravity();
        }
    }
    void HandleJump()
    {
        // Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !isJumping)
        {
            Jump();
            jumpBufferCounter = 0f; 
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f; 
        }
        if (isGrounded && !wasGroundedLastFrame)
        {
            isJumping = false;
        }
    }
    void HandleDash()
    {
        if (isGrounded && Input.GetKeyUp(KeyCode.LeftShift) && !isDashing && canDash)
        {
            StartDash();
            SoundManagerScript.Instance.PlayDash();
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

    void CheckEnemyDetection()
    {
        EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
        bool anyEnemyDetecting = false;

        foreach (EnemyScript enemy in enemies)
        {
            if (enemy.IsDetectingPlayer())
            {
                anyEnemyDetecting = true;
                break;
            }
        }

        canDash = anyEnemyDetecting;
    }



    void Jump()
    {
        SoundManagerScript.Instance.PlayJump();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        coyoteTimeCounter = 0f;

    }

    void ApplyBetterGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void HandlePowerUps()
    {
        if (isInvulnerable && Time.time >= invulnerabilityEndTime)
        {
            isInvulnerable = false;
            SetColor(originalColor);
        }

        if (isSpeedBoosted && Time.time >= speedBoostEndTime)
        {
            EndSpeedBoost();
        }

        if (canTeleport && Time.time >= teleportEndTime)
        {
            canTeleport = false;
            SetColor(originalColor);
        }

        if (canTeleport && Input.GetKeyDown(KeyCode.Z))
        {
            SoundManagerScript.Instance.PlayTeleport();
            Teleport();
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

    public bool GetIsInvulnerable()
    {
        return isInvulnerable;
    }

    public void ActivateSpeedBoost(float boost, float duration)
    {
        if (transform.localScale.x != regularPlayerWidth)
        {
            scaleX.x = regularPlayerWidth;
        }
        boostSpeed = boost;
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
                    SoundManagerScript.Instance.PlayDestroy();
                }
                break;
            //case "Goal":
            //    endLevel = true;
            //    break;
            case "Ground":
            case "Platform":
                isGrounded = true;
                isJumping = false;
                break;
            case "JunkFood":
                PlayerMaxDeform();
                Destroy(collision.gameObject);
                SoundManagerScript.Instance.PlayEat();
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
                //other.gameObject.GetComponent<Tutorial>().StartDialogue();
                break;
            case "Enemy":
                if (isDashing || isInvulnerable)
                {
                    Destroy(other.gameObject);
                    ScoreManager.Instance.AddScore(50);
                    SoundManagerScript.Instance.PlayDestroy();
                }
                else
                {
                    gameOver = true;
                }
                break;
            case "Obstacle":
                if (isDashing || isInvulnerable)
                {
                    Destroy(other.gameObject);
                    SoundManagerScript.Instance.PlayDestroy();
                }
                else
                {
                    gameOver = true;
                }
                break;
            case "SpeedBoost":
                PlayerMinDeform();
                float speedBoostDuration = 3.0f;
                ModifySpeed(1.6f, speedBoostDuration);
                SoundManagerScript.Instance.PlayBoost();
                Destroy(other.gameObject, 0.1f);
                //playerScaled = false;
                break;
        }

    }

    // SpeedBoost
    public void ModifySpeed(float speedBoostAmount, float speedBoostDuration)
    {
        if (isSpeedBoosted) return;
        speedBeforeBoost = speed;

        float newSpeed = speed + speedBoostAmount;
        speed = Mathf.Min(maxSpeed, newSpeed);

        isSpeedBoosted = true;
        speedBoostEndTime = Time.time + speedBoostDuration;

        Debug.Log($"Speed: {speedBeforeBoost}, to {speed}. Duration: {speedBoostDuration}s");
        SetColor(Color.cyan);


    }

    private void EndSpeedBoost()
    {
        isSpeedBoosted = false;
        speed = speedBeforeBoost > 0 ? speedBeforeBoost : baseSpeed;
        SetColor(originalColor);

        Debug.Log($"Speed boost ended, current speed: {speed}");
    }



    private void PlayerMaxDeform()
    {
        if (!playerScaled && transform.localScale.x > minPlayerWidth)
        {
            scaleX = transform.localScale;

            scaleX.x += 0.5f;
            transform.localScale = scaleX;
            speed = Mathf.Max(1.0f, speed - 1);
        }
        playerScaled = true;
    }


    private void PlayerMinDeform()
    {
        if (!playerScaled && transform.localScale.x < maxPlayerWidth)
        {
            scaleX = transform.localScale;
            if (transform.localScale.x != regularPlayerWidth)
            {
                scaleX.x = regularPlayerWidth;
                transform.localScale = scaleX;
                Debug.Log("Scaling back to normal");
                playerScaled = true;
            }

            if (speed < maxSpeed)
            {
                speed = Mathf.Min(maxSpeed, speed + 1);
            }
        }
    }



    private void GameOver()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
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
