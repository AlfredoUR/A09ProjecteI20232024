using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraScript : MonoBehaviour
{
    public UnityEngine.Transform Player;

    public float smoothTime;
    public Vector3 offset = new Vector3(0, 2, -10);

    public float minY = -5f;
    public float maxY = 20f;

    public bool followYAxis = true;
    public float deadZoneY = 2f;

    private PlayerMovement playerMovement;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player").transform;
            if (Player == null)
            {
                Debug.LogError("Player not found.");
                return;
            }
        }

        playerMovement = Player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("Script PlayerMovement not found.");
        }

        Vector3 initialPos = Player.position + offset;
        initialPos.y = Mathf.Clamp(initialPos.y, minY, maxY);
        transform.position = initialPos;
    }

    void LateUpdate()
    {
        if (Player == null || playerMovement == null) return;
        if (playerMovement.endLevel || playerMovement.gameOver) return;

        Vector3 targetPosition = Player.position + offset;

        if (!followYAxis || Mathf.Abs(targetPosition.y - transform.position.y) < deadZoneY)
        {
            targetPosition.y = transform.position.y;
        }

        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        targetPosition.z = offset.z;

        float smoothTime = 0.15f;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        //transform.position = new Vector3((float)-91.54, 1.8f, -10f);
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    public void ResetCameraPosition()
    {
        if (Player != null)
        {
            Vector3 newPos = Player.position + offset;
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            transform.position = newPos;
            velocity = Vector3.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Player != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 targetPos = Player.position + offset;
            Gizmos.DrawWireSphere(targetPos, 0.5f);

            Gizmos.color = Color.red;
            Vector3 deadZoneMin = new Vector3(targetPos.x, targetPos.y - deadZoneY, targetPos.z);
            Vector3 deadZoneMax = new Vector3(targetPos.x, targetPos.y + deadZoneY, targetPos.z);
            Gizmos.DrawLine(deadZoneMin, deadZoneMax);
        }
    }
}