using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource actionSource;

    [Header("Action Sounds")]
    public AudioClip gunFireSound;
    public AudioClip reloadSound;
    public AudioClip emptyGunSound;

    [Header("Settings")]
    [Range(0f, 1f)] public float actionVolume = 0.8f;

    void Start()
    {
        SetupAudioSources();
    }

    void SetupAudioSources()
    {
        if (actionSource == null)
            actionSource = gameObject.AddComponent<AudioSource>();

        actionSource.volume = actionVolume;
        actionSource.spatialBlend = 0f;
    }

    public void PlayGunFire()
    {
        if (gunFireSound != null && actionSource != null)
            actionSource.PlayOneShot(gunFireSound, actionVolume);
    }

    public void PlayReload()
    {
        if (reloadSound != null && actionSource != null)
            actionSource.PlayOneShot(reloadSound, actionVolume);
    }

    public void PlayEmptyGun()
    {
        if (emptyGunSound != null && actionSource != null)
            actionSource.PlayOneShot(emptyGunSound, actionVolume * 0.6f);
    }
}
