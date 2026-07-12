using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // Instance static agar bisa dipanggil langsung: SFXManager.instance.PlaySFX(...)
    public static SFXManager instance;

    [Header("Komponen Audio Source")]
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Daftar Aset MP3 Efek Suara")]
    public AudioClip jumpSound;
    public AudioClip coinSound; // <--- INI DIA TAMBAHAN UNTUK SUARA KOIN
    public AudioClip victorySound;
    public AudioClip gameOverSound;
    public AudioClip trapSound;

    private void Awake()
    {
        // Setup Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Biar suara tidak putus saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi utama untuk memutar efek suara satu kali (OneShot)
    public void PlaySFX(AudioClip clip)
    {
        if (sfxAudioSource != null && clip != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }
}