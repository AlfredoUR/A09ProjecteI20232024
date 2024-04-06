using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float cameraSpeed;
    public Transform Goal;
    public GameObject Player;
    public Rigidbody2D cameraRb;
    private float playerX;
    //private playerScript;
    private bool endLevel;
    void Start()
    {
        //Player_Movement player_Movement; 
        //transform.Rotate(0, -28, 0);
        Goal = GameObject.FindWithTag("Goal").transform;
        Player = GameObject.FindWithTag("Player");
        cameraRb = GetComponent<Rigidbody2D>();
       // playerX = (float)Player.GetComponent<Position>();
        //cameraSpeed = Player.GetComponent<Player_Movement>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        cameraSpeed = Player.GetComponent<Player_Movement>().speed;
        endLevel = Player.GetComponent<Player_Movement>().endLevel;
        if (endLevel == false)
        {
           //cameraRb.velocity = new Vector2(cameraSpeed, cameraRb.velocity.y);
            //transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }

        //transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        //transform.Translate(new Vector2(cameraSpeed *Time.deltaTime, 0f));
        //new Vector2(speed, rb.velocity.y)
    }
}