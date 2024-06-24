using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player_Movement : MonoBehaviour
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
    public GameObject GameManager;
    public Transform Camera;
    //public GameObject SceneChanger;
    public int levelIndex;
    public bool isTutorial;
    bool platformFound;
    public bool canTeleport;
    public bool isGrounded ;
    public bool isSprinting ;
    public bool endLevel ;
    public bool gameOver;

    Vector2 scaleX;
    public float posX;
    public float posY;
    //Vector2 scaleY;
    public bool playerScaled;
    private float minPlayerWidth = 0.5f;
    private float regularPlayerWidth = 1.0f;
    private float maxPlayerWidth = 2.0f;

    float gravityScale ;
  

    // Start is called before the first frame update
    void Start()
    {
        levelIndex = GameManager.GetComponent<SceneChanger>().GetLevelIndex();
        speed = baseSpeed;
        float gravityScale = rb.gravityScale;
        isGrounded = false;
        endLevel = false;
        slowing = false;
        // reload = GameManager.GetComponent<SceneChanger>();

        cameraSpeed = Camera.GetComponent<CameraScript>().cameraSpeed;

        //Physics.gravity = gravity;
        ground = GameObject.FindWithTag("Ground");
        rb = GetComponent<Rigidbody2D>();
        isSprinting = false;

        platform = GameObject.FindWithTag("Platform");
        platformFound = false;
        canTeleport = false;
        // platformTrigger = platform.GetComponentInChildren<GameObject>();
    }



    // Update is called once per frame
    void Update()
    {
        posX = rb.position.x;
        posY = rb.position.y;


        if (gameOver != false)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            //print(speed);
        }
        else
        {
            GameOver();
        }
        if (endLevel)
        {
            EndLevel();
        }
       

        if (isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, jumpForce *speed* gravity));
            }         
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y* gravity);
        }

        //if (Input.GetButton("Up"))
        //{

        //}


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Enemy"))
        {
            gameOver = true;
            endLevel = true;
            SceneManager.LoadScene("GameOver");
        }
        if (collision.gameObject.tag == ("Ground")|| collision.gameObject.tag == ("Platform"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.tag == ("JunkFood"))
        {
            PlayerMaxDeform();
            Destroy(collision.gameObject);
            playerScaled= false;
        }
        if (collision.gameObject.tag == ("GoodFood"))
        {
            PlayerMinDeform();
            Destroy(collision.gameObject);
            playerScaled= false;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Ground") || collision.gameObject.tag == ("Platform"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.tag == ("JunkFood"))
        {
            playerScaled = true;
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Goal"))
        {
            endLevel = true;
        }
        if (other.gameObject.tag == ("PlatformTrigger"))
        {
             transform.Translate(new Vector2 (rb.position.x,rb.position.y +8.0f));
            
        }
            //    platformTeleport();
            //    //if (platformFound==false)
            //    //{
            //    //    platformFound = true;
            //    //    currentPlatform = platformTrigger.transform.parent.gameObject;
            //    //    platformTeleport();
            //    //}
            //}


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Goal"))
        {
            endLevel = true;
        }
        //if (other.gameObject.tag == ("PlatformTrigger"))
        //{
        //    platformFound = false;
        //    platformTeleport();
        //}


    }
    void PlayerMaxDeform()
    {
        if (playerScaled == false)
        {
            //Deform player
            if (transform.localScale.x > minPlayerWidth)
            {
                scaleX = transform.localScale;
                scaleX.x += 0.5f;
                transform.localScale = scaleX;
                Debug.Log("Scaling");
                //playerScaled = true;
                speed--;
            }
        }

    }
    void PlayerMinDeform()
    {
        gradualSlow();
        if (playerScaled == false)
        {
            //Deform player
            if (transform.localScale.x < maxPlayerWidth)
            {
                scaleX = transform.localScale;
                bool regularScale = false;
                if (transform.localScale.x == regularPlayerWidth)
                {
                    regularScale = true;
                }

                if (regularScale)
                {
                }
                else
                {
                    scaleX.x = 1.0f;
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
        //if (transform.localScale.x < regularPlayerWidth)
        //{
        //    do
        //    {
        //        scaleX = transform.localScale;
        //        scaleX.x -= 0.5f;
        //        transform.localScale = scaleX;
        //        getMaxSpeed();
        //    }
        //    while (!slowing);
        //}
        //if (transform.localScale.x == regularPlayerWidth)
        //{
        //    playerScaled = true;
        //    Debug.Log("Slowing");
        //    slowing = true;
        //}


    }

    void gradualSlow()
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
    void getMaxSpeed()
    {
        while (speed <= maxSpeed)
        {
            speed++;
            cameraSpeed++;
            if (speed == maxSpeed)
            {
                slowing = true; break;
            }
        }

    }
    void platformTeleport()
    {
        ////currentPlatform = platformTrigger.transform.parent.gameObject;
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    posY += 8.0f;// currentPlatform.transform.position.y;
        //}


    }
    void FixedUpdate()
    {


        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1));
        //}

        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1));
        //}
    }

    void GameOver()
    {

       SceneManager.LoadScene("GameOverScene");
        
    }

    void EndLevel()
    {
        switch (levelIndex)
        {
            case 0:
                SceneManager.LoadScene("GameOverScene");
                break;
            case 1:
                SceneManager.LoadScene("GameOverScene");
                break;
            case 2:
                SceneManager.LoadScene("GameOverScene");
                break;
            case 3:
                SceneManager.LoadScene("GameOverScene");
                break;
            default:
                SceneManager.LoadScene("GameOverScene");
                break;
        }
    }
}
