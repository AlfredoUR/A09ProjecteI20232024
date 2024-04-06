using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player_Movement : MonoBehaviour
{

    public float speed = 4.0f;
    public float maxSpeed = 6.0f;
    float jumpForce = 1000f;
    float gravity = 0.9f;
    public Rigidbody2D rb;
    public GameObject ground;
    public GameObject player;
    public GameObject platform;
    public GameObject platformTrigger;
    public GameObject currentPlatform;
    bool platformFound;
    public bool canTeleport;
    public bool isGrounded ;
    public bool isSprinting ;
    public bool endLevel ;


    Vector2 scaleX;
    public float posX;
    public float posY;
    //Vector2 scaleY;
    public bool playerScaled;
    private float minPlayerWidth = 0.5f;

    private float maxPlayerWidth = 2.0f;

    float gravityScale ;
  

    // Start is called before the first frame update
    void Start()
    {
        float gravityScale = rb.gravityScale;
        isGrounded = false;
        endLevel = false;
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
        if (endLevel == false )
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
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
                speed --;
            }
        }

    }
    void PlayerMinDeform()
    {
        if (playerScaled == false)
        {
            //Deform player
            if (transform.localScale.x < maxPlayerWidth)
            {
                scaleX = transform.localScale;
                scaleX.x -= 0.5f;
                transform.localScale = scaleX;
                Debug.Log("Scaling");
                playerScaled = true;
                if (speed <= maxSpeed)
                {
                    speed = maxSpeed;
                }
                else
                {
                    speed ++;
                }
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
}
