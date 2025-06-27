using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip level1Music;
    public AudioClip tutorialMusic;
    public AudioClip gameOverMusic;

    private AudioSource audioSource;
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Level1":
                ChangeMusic(level1Music);
                break;
            case "Tutorial":
                ChangeMusic(tutorialMusic);
                break;
            case "GameOverScene":
                ChangeMusic(gameOverMusic);
                break;
                default: 
                break;
                
        }
    }

    void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
