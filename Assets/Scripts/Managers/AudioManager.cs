using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private float startVolume = 0.3f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Sound Clips")]
    public AudioClip[] playerDamageClips;
    public AudioClip[] playerStepClips;
    public AudioClip[] zombieDamageClips;
    public AudioClip[] zombieDeathClips;
    public AudioClip[] zombieIdleClips;
    public AudioClip[] shootPistolClips;
    public AudioClip[] reloadPistolClips;
    public AudioClip[] emptyPistolClips;

    // Erlaubt anderen Klassen auf den Manager zuzugreifen
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Sorgt daf�r das immer nur eine Instanz dieses Managers existiert
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        
        SetVolume(startVolume);
    }

    public void SetVolume(float newVolume)
    {
        sfxSource.volume = newVolume;
        musicSource.volume = newVolume;
    }

    public void PlayerDamage()
    {
        int len = playerDamageClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(playerDamageClips[Random.Range(0, len)]);
        }
    }
    
    public void PlayerStep()
    {
        int len = playerStepClips.Length;
        if (len > 0 )
        {
           sfxSource.PlayOneShot(playerStepClips[Random.Range( 0, len )]);
        }
    }

    public void ZombieDamage()
    {
        int len = zombieDamageClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(zombieDamageClips[Random.Range(0, len)]);
        }
    }

    public void ZombieDeath()
    {
        int len = zombieDeathClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(zombieDeathClips[Random.Range(0, len)]);
        }
    }

    public void ZombieIdle()
    {
        int len = zombieIdleClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(zombieIdleClips[Random.Range(0, len)]);
        }
    }

    public void ShootPistol()
    {
        int len = shootPistolClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(shootPistolClips[Random.Range(0, len)]);
        }
    }

    public void ReloadPistol()
    {
        int len = reloadPistolClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(reloadPistolClips[Random.Range(0, len)]);
        }
    }

    public void EmptyPistol()
    {
        int len = emptyPistolClips.Length;
        if (len > 0)
        {
            sfxSource.PlayOneShot(emptyPistolClips[Random.Range(0, len)]);
        }
    }
}
