using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    void Start()
    {
        // Saat game dimulai, pastikan menu utama aktif & panel setting sembunyi
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Fungsi untuk Tombol PLAY -> Pindah ke Level 1 (Scene TPS)
    public void PlayGame()
    {
        // Memuat Scene dengan Index 1 (TPS) yang sudah di-setup di Build Profiles
        SceneManager.LoadScene(1); 
    }

    // Fungsi untuk Tombol SETTING -> Membuka Panel Setting
    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Fungsi untuk Tombol BACK di Panel Setting -> Kembali ke Menu Utama
    public void CloseSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // Fungsi untuk Tombol QUIT -> Keluar dari Game
    public void QuitGame()
    {
        Debug.Log("Keluar dari Game...");
        Application.Quit();
    }
}