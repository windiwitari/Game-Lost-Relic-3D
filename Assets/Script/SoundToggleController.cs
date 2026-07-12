using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundToggleController : MonoBehaviour, IPointerClickHandler
{
    [Header("Referensi Komponen UI (Raw Image)")]
    [SerializeField] private RawImage buttonRawImage; // Tarik komponen Raw Image tombol ke sini
    
    [Header("Referensi Texture Ikon Suara")]
    [SerializeField] private Texture soundOnTexture;  // Gambar ikon suara aktif
    [SerializeField] private Texture soundOffTexture; // Gambar ikon suara disenyapkan (mute)

    [Header("Referensi Audio Source")]
    [SerializeField] private AudioSource bgmAudioSource; // Tarik objek _AudioManager ke sini

    private bool isMuted = false;

    private void Start()
    {
        // Sinkronisasi awal: Cek jika AudioSource bawaan dalam kondisi mati/menyala
        if (bgmAudioSource != null)
        {
            isMuted = bgmAudioSource.mute;
        }

        UpdateVisualButton();
    }

    // Mendeteksi klik mouse / sentuhan jari langsung pada tombol suara
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleSound();
    }

    private void ToggleSound()
    {
        if (bgmAudioSource == null) return;

        // Balikkan status mute
        isMuted = !isMuted;
        bgmAudioSource.mute = isMuted; // Perintahkan AudioSource untuk mute/unmute

        UpdateVisualButton();
    }

    private void UpdateVisualButton()
    {
        if (buttonRawImage == null) return;

        if (isMuted)
        {
            if (soundOffTexture != null) buttonRawImage.texture = soundOffTexture; // Ubah logo ke suara mati
        }
        else
        {
            if (soundOnTexture != null) buttonRawImage.texture = soundOnTexture;  // Ubah logo ke suara menyala
        }
    }
}