using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float playerSpeed;
    public float cameraSpeed;
    private float cameraDistance = 1.0f;
    //public Transform Goal;
    public Transform target;
    public Transform Player;
    public Rigidbody2D cameraRb;
    private float playerX;
    //private playerScript;
    private bool endLevel;
    private bool gameOver;
    void Start()
    {
        //Player_Movement player_Movement; 
        //transform.Rotate(0, -28, 0);
        target = Player;
        //Player = GameObject.FindWithTag("Player").transform;
        cameraRb = Player.GetComponent<Player_Movement>().rb;
        //playerX = (float)Player.GetComponent<Position>();
        playerSpeed = Player.GetComponent<Player_Movement>().speed;
        gameOver = Player.GetComponent<Player_Movement>().gameOver;

    }

    // Update is called once per frame
    void Update()
    {
       
        //cameraSpeed = Player.GetComponent<Player_Movement>().speed;
        //endLevel = Player.GetComponent<Player_Movement>().endLevel;
        if ((endLevel == false)&&(gameOver == false))
        {
            cameraSpeed = playerSpeed - cameraDistance;
            //cameraRb.velocity = new Vector2(cameraSpeed, cameraRb.velocity.y);
            if (playerSpeed == 3.0f)
            {
                cameraDistance = 2.0f;
            }
            transform.Translate(new Vector2(cameraSpeed,0f) *Time.deltaTime);
            //if (playerX > position.x)
            //Vector2 position = transform.position;
            //position.x = target.position.x;
            //transform.position = position;
            //cameraRb.velocity = new Vector2(cameraSpeed, cameraRb.velocity.y);
            //transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }

        //transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
       
        //new Vector2(speed, rb.velocity.y)
    }
}