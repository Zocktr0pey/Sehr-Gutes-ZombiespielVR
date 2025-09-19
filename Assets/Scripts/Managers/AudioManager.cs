using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Sound Clips")]
    public AudioClip[] playerDamageClips;
    public AudioClip[] shootClips;

    // Erlaubt anderen Klassen auf den Manager zuzugreifen
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Sorgt dafür das immer nur eine Instanz dieses Managers existiert
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
        
    }

    public void UpdateVolume(float newVolume)
    {
        sfxSource.volume = newVolume;
        musicSource.volume = newVolume;
    }

    public void PlayerDamage()
    {
        
        int len = playerDamageClips.Length;
        if (len > 0 )
        {
           sfxSource.PlayOneShot(playerDamageClips[Random.Range( 0, len )]);
        }
    }
}
