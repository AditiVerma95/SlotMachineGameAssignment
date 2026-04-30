using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Settings")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip buttonClick;
    public AudioClip spinSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    private void Awake()
    {
        // Singleton pattern to access from SpinManager
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

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            // PlayOneShot allows sounds to overlap without cutting off
            sfxSource.PlayOneShot(clip);
        }
    }
}