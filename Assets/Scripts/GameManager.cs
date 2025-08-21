using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerHealth = 3;
    public int playerLives = 3;
    public int bombs = 3;

    [Header("Gate / Waves")]
    public GameObject gatePrefab;
    public Transform gateSpawnPoint;
    public WaveManager waveManager;
    public int currentWave = 1;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI livesText;  // new
    public TextMeshProUGUI bombsText;  // new
    public TextMeshProUGUI waveText;   // new

    [HideInInspector] public int totalCrystals = 0;

    int score = 0;
    int collectedCrystals = 0;
    bool gameEnded = false;
    float startTime;
    int maxHealth;
    GameObject gateInstance;

    void Start()
    {
        startTime = Time.time;
        maxHealth = playerHealth;

        if (waveManager != null)
        {
            waveManager.SetGameManager(this);
            waveManager.StartWave(currentWave);
        }

        UpdateUI();
    }

    void Update()
    {
        if (gameEnded) return;
        if (timerText != null)
        {
            float elapsed = Time.time - startTime;
            int minutes = Mathf.FloorToInt(elapsed / 60f);
            int seconds = Mathf.FloorToInt(elapsed % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    public void AddScore(int amount)
    {
        if (gameEnded) return;
        score += amount;
        UpdateUI();
    }

    public void PlayerTakeDamage(int damage)
    {
        if (gameEnded) return;
        playerHealth -= damage;
        if (playerHealth <= 0)
            LoseLife();
        else
            UpdateUI();
    }

    void LoseLife()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            EndGame(false);
            return;
        }
        playerHealth = maxHealth; // restore health
        UpdateUI();
        // Optional: reposition player to a safe spawn if needed
    }

    public void UseBomb()
    {
        if (gameEnded || bombs <= 0) return;
        bombs--;
        BombSystem.ClearAllEnemies();
        UpdateUI();
    }

    public void CollectCrystal(GameObject crystal)
    {
        collectedCrystals++;
        Destroy(crystal);
        AddScore(5);

        if (collectedCrystals >= totalCrystals)
            SpawnGate();
    }

    void SpawnGate()
    {
        if (gatePrefab == null || gateSpawnPoint == null) return;
        if (gateInstance != null) return;
        gateInstance = Instantiate(gatePrefab, gateSpawnPoint.position, Quaternion.identity);
    }

    public void OnPlayerEnterGate()
    {
        if (gateInstance == null) return;
        NextWave();
    }

    void NextWave()
    {
        currentWave++;
        collectedCrystals = 0;
        if (gateInstance != null) Destroy(gateInstance);

        if (waveManager != null)
        {
            waveManager.StartWave(currentWave);
            UpdateUI();
        }
        else
        {
            EndGame(true);
        }
    }

    void UpdateUI()
    {
        if (scoreText)   scoreText.text = $"Score: {score}";
        if (healthText)  healthText.text = $"Health: {playerHealth}";
        if (livesText)   livesText.text = $"Lives: {playerLives}";
        if (bombsText)   bombsText.text = $"Bombs: {bombs}";
        if (waveText)    waveText.text = $"Wave: {currentWave}";
    }

    public void EndGame(bool win)
    {
        gameEnded = true;
        if (endText) endText.text = win ? "YOU WIN!" : "GAME OVER";
        Time.timeScale = 1f; // leave running for demo
    }
}
