using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Invulnerability, SpeedBoost, Teleport }
    public PowerUpType powerUpType;



    public float duration = 5.0f;
    public Sprite invulnerabilitySprite;
    public Sprite speedBoostSprite;
    public Sprite teleportSprite;
    //Teleport
    public float valueX;
    public float valueY;
    //SpeedBoost
    public float speedBoostMultiplier = 1.2f;
    public float speedBoostDuration = 2.0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetPowerUpAppearance();
    }

    void SetPowerUpAppearance()
    {
        switch (powerUpType)
        {
            case PowerUpType.Invulnerability:
                spriteRenderer.sprite = invulnerabilitySprite;
                break;
            case PowerUpType.SpeedBoost:
                spriteRenderer.sprite = speedBoostSprite;
                break;
            case PowerUpType.Teleport:
                spriteRenderer.sprite = teleportSprite;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                ApplyPowerUp(playerMovement);
            }
            Destroy(gameObject); 
        }
    }

    void ApplyPowerUp(PlayerMovement playerMovement)
    {
        switch (powerUpType)
        {
            case PowerUpType.Invulnerability:
                playerMovement.ActivateInvulnerability(duration);
                break;
            case PowerUpType.SpeedBoost:
                playerMovement = playerMovement.GetComponent<PlayerMovement>();
                //playerMovement.ActivateSpeedBoost(speedBoostMultiplier, speedBoostDuration);
                playerMovement.ModifySpeed(speedBoostMultiplier, speedBoostDuration);
                break;
            case PowerUpType.Teleport:
                playerMovement.ActivateTeleport(valueX, valueY, duration);
                break;
        }
    }
}
