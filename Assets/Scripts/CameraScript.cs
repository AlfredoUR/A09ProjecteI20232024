using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class CameraScript : MonoBehaviour
{
    public Transform Player; 
    public float followSpeed ; 
    public Vector3 offset; 

    private PlayerMovement playerMovement;
    private bool playerIsGrounded;
    void Start()
    {
        followSpeed = 0.5f;
        offset = transform.position - Player.position;
        playerMovement = Player.GetComponent<PlayerMovement>();
        playerIsGrounded = Player.GetComponent<PlayerMovement>().isGrounded;
    }

    void LateUpdate()
    {
        if (!playerMovement.endLevel && !playerMovement.gameOver)
        {
            if (!playerIsGrounded) 
            {
                followSpeed = 10.0f;
            }
            else
            {
                followSpeed = 0.5f;
            }
            Vector3 desiredPosition = Player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}

