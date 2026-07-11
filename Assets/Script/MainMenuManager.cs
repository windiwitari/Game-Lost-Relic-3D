using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Wajib ada untuk komponen Button

public class MainMenuManager : MonoBehaviour
{
    [Header("Referensi Objek UI Canvas (GameObject)")]
    // Tarik objek 'Papan Kayu Induk' (yang ada LOST RELIC 3D-nya) ke sini
    public GameObject mainMenuPanel; 
    
    // Tarik objek 'PanelTutorial' (Raw Image gambar caramu bermain) ke sini
    public GameObject tutorialPanel; 

    [Header("Referensi Tombol (Button)")]
    // Tarik Tombol PLAY kayu menu ke sini (opsional, untuk dicolok di Inspector)
    public Button playButton; 
    
    // Tarik Tombol 'StartGameButton' transparan ke sini
    public Button startGameButton; 
    
    // Tarik Tombol Silang (X) pojok tutorial ke sini
    public Button closeTutorialButton; 

    // TAMBAHAN: Tarik Tombol EXIT papan kayu menu ke sini
    public Button exitGameButton; 

    private void Start()
    {
        // Pengaman: Saat game dimulai, pastikan menu utama aktif, panel tutorial sembunyi
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false); // Pastikan tutorial tertutup di awal

        // Mendaftarkan fungsi klik tombol secara otomatis (lebih bersih dibanding OnClick Inspector)
        if (playButton != null) playButton.onClick.AddListener(BukaTutorial);
        if (startGameButton != null) startGameButton.onClick.AddListener(MulaiMasukGameLevel1);
        if (closeTutorialButton != null) closeTutorialButton.onClick.AddListener(TutupTutorial);
        
        // TAMBAHAN: Daftarkan fungsi tombol EXIT
        if (exitGameButton != null) exitGameButton.onClick.AddListener(QuitGame);
    }

    // Fungsi untuk Tombol PLAY -> Sekarang membuka Panel Tutorial dulu
    public void BukaTutorial()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false); // Sembunyikan menu utama
        if (tutorialPanel != null) tutorialPanel.SetActive(true);  // Memunculkan gambar pop-up tutorial
    }

    // Fungsi untuk Tombol Silang (X) pojok tutorial -> Tutup tutorial, kembali ke menu utama
    public void TutupTutorial()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);  // Munculkan menu utama kembali
        if (tutorialPanel != null) tutorialPanel.SetActive(false); // Sembunyikan gambar tutorial
    }

    // Fungsi untuk Tombol Transparan "MULAI PERMAINAN" di dalam panel tutorial
    public void MulaiMasukGameLevel1()
    {
        // Memuat Scene dengan Index 1 (TPS) yang sudah di-setup di Build Profiles
        SceneManager.LoadScene(1); 
    }

    // Fungsi untuk Tombol EXIT papan kayu -> Keluar dari Aplikasi
    public void QuitGame()
    {
        Debug.Log("Keluar dari Game Lost Relic 3D...");
        
        // Fungsi untuk keluar dari game (hanya berfungsi setelah game di-build .exe atau .apk)
        Application.Quit();

        // TAMBAHAN: Pengaman agar di Unity Editor game juga ikut berhenti saat EXIT diklik
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
}