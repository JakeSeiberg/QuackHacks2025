using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Enemy Sounds")]
    public AudioClip[] movementSounds;
    public AudioClip[] attackSounds;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 0.7f;
    public float minDistance = 5f;
    public float maxDistance = 50f;

    void Start()
    {
        SetupAudioSource();
    }

    void SetupAudioSource()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = volume;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
    }

    public void PlayMovementSound()
    {
        PlayRandomSound(movementSounds);
    }

    public void PlayAttackSound()
    {
        PlayRandomSound(attackSounds);
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length > 0 && audioSource != null)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
