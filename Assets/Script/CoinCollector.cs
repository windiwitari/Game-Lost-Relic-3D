using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    [Header("Sistem Skor Statue")]
    public int coinsCollected = 0;
    public int totalCoinsInLevel = 5;
    public TextMeshProUGUI coinText;

    [Header("Sistem Armor Player")]
    public int playerLives = 3;
    public TextMeshProUGUI heartText;

    [Header("Sistem Titik Aman")]
    public Transform spawnPoint;

    [Header("UI Game Over")]
    public Canvas gameOverCanvas;    
    public Button restartButton;     
    public Button exitButton;        

    [Header("FITUR PENILAIAN 1: Sistem Waktu")]
    public float timeRemaining = 90f; 
    public TextMeshProUGUI timerText; 
    private bool isGameFinished = false;

    [Header("FITUR PENILAIAN 2: UI Menang (Victory)")]
    public Canvas victoryCanvas;     // Hubungkan objek 'Canvas Victory' yang non-ceklis ke sini
    public Button nextButton;        // Hubungkan objek 'NextButton' di dalam Canvas Victory

    private void Start()
    {
        UpdateCoinUI();
        UpdateHeartUI();
        isGameFinished = false;

        // Trik Anti-Stuck: Paksa aktifkan GameObject-nya sebentar, lalu matikan visual komponen Canvas-nya saja
        if (gameOverCanvas != null) 
        {
            gameOverCanvas.gameObject.SetActive(true); 
            gameOverCanvas.enabled = false; 
        }

        if (victoryCanvas != null)
        {
            victoryCanvas.gameObject.SetActive(true);
            victoryCanvas.enabled = false;
        }

        // Daftarkan fungsi klik tombol secara otomatis
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
        if (nextButton != null) nextButton.onClick.AddListener(LoadNextLevel);
    }

    private void Update()
    {
        if (!isGameFinished)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; 
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                UpdateTimerUI();
                GameOverAction(); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameFinished) return;

        // 1. Ambil Patung (Statue)
        if (other.CompareTag("Coin"))
        {
            coinsCollected++;
            UpdateCoinUI();
            Destroy(other.gameObject);
        }

        // 2. Masuk Portal (Syarat Menang)
        if (other.CompareTag("Portal") && coinsCollected >= totalCoinsInLevel)
        {
            VictoryAction(); 
        }

        // 3. Kena Jebakan Kota
        if (other.CompareTag("Trap"))
        {
            DecreaseLife();
        }
    }

    void DecreaseLife()
    {
        playerLives--;
        UpdateHeartUI();

        if (playerLives > 0)
        {
            RespawnPlayer();
        }
        else
        {
            GameOverAction();
        }
    }

    void RespawnPlayer()
    {
        if (spawnPoint != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            if (cc != null) cc.enabled = true;
        }
    }

    void GameOverAction()
    {
        isGameFinished = true;
        Debug.Log("Game Over!");
        if (gameOverCanvas != null) gameOverCanvas.enabled = true;
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    void VictoryAction()
    {
        isGameFinished = true;
        Debug.Log("Selamat! Kamu Menang!");
        
        if (victoryCanvas != null) victoryCanvas.enabled = true;
        
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }

    // Mengubah tampilan teks koin menjadi Statue
    void UpdateCoinUI()
    {
        if (coinText != null) coinText.text = "Statue: " + coinsCollected + " / " + totalCoinsInLevel;
    }

    // Mengubah tampilan teks nyawa menjadi Armor
    void UpdateHeartUI()
    {
        if (heartText != null) heartText.text = "Armor: " + playerLives;
    }

    void UpdateTimerUI()
    {
        if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining) + "s";
    }
}