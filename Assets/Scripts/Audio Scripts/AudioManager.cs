using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;

    [Header("Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.3f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetupAudioSources();
        PlayBackgroundMusic();
    }

    void SetupAudioSources()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float vol)
    {
        musicVolume = Mathf.Clamp01(vol);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

}
