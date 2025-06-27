using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SoundManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static SoundManagerScript Instance;

    public AudioSource soundSource;

    public AudioClip dashSound;
    public AudioClip teleportSound;
    public AudioClip destroySound;
    public AudioClip jumpSound;
    public AudioClip boostSound;
    public AudioClip eatSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDash() => soundSource.PlayOneShot(dashSound);
    public void PlayTeleport() => soundSource.PlayOneShot(teleportSound);
    public void PlayDestroy() => soundSource.PlayOneShot(destroySound);
    public void PlayJump() => soundSource.PlayOneShot(jumpSound);
    public void PlayBoost() => soundSource.PlayOneShot(boostSound);
    public void PlayEat() => soundSource.PlayOneShot(eatSound);
}
