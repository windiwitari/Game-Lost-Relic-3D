using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // WAJIB ADA

public class HUDPauseToggle : MonoBehaviour, IPointerClickHandler // Tambahkan interface deteksi klik langsung
{
    [Header("Referensi Komponen UI (Raw Image Tombol HUD)")]
    [SerializeField] private RawImage buttonRawImage; 
    
    [Header("Referensi Texture Emas (HUD)")]
    [SerializeField] private Texture pauseTexture; 
    [SerializeField] private Texture playTexture;  

    [Header("Referensi Panel Pop Up Pause")]
    [SerializeField] private GameObject canvasPauseObject; 
    [SerializeField] private Button continueButton;        
    [SerializeField] private Button exitButton;            

    private bool isGamePaused = false;

    private void Start()
    {
        // Daftarkan tombol yang ada di dalam panel pop-up secara otomatis
        if (continueButton != null) continueButton.onClick.AddListener(TogglePause);
        if (exitButton != null) exitButton.onClick.AddListener(ExitToMainMenu);

        if (buttonRawImage != null && pauseTexture != null) buttonRawImage.texture = pauseTexture;
        
        if (canvasPauseObject != null)
        {
            canvasPauseObject.SetActive(false);
        }
    }

    // FUNGSI BARU: Mendeteksi klik secara fisik menembus segala jenis bug UI Unity
    public void OnPointerClick(PointerEventData eventData)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f; 
            if (buttonRawImage != null && playTexture != null) buttonRawImage.texture = playTexture;
            if (canvasPauseObject != null) canvasPauseObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f; 
            if (buttonRawImage != null && pauseTexture != null) buttonRawImage.texture = pauseTexture;
            if (canvasPauseObject != null) canvasPauseObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    private void OnDisable()
    {
        Time.timeScale = 1f; 
    }
}