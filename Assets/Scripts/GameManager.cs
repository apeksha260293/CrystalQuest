// using UnityEngine;
// using UnityEngine.SceneManagement;   // for restart key
// using TMPro;

// public class GameManager : MonoBehaviour
// {
//     [Header("UI")]
//     public TMP_Text scoreText;      // assign ScoreText
//     public TMP_Text timerText;      // assign TimerText
//     public TMP_Text endText;        // optional EndText

//     [Header("Rules")]
//     [SerializeField] private int totalCrystals = 10;   // keep field private
//     public int TotalCrystals                            // expose safely for WaveManager
//     {
//         get => totalCrystals;
//         set
//         {
//             totalCrystals = Mathf.Max(0, value);
//             UpdateScoreUI(); // keep UI in sync if WaveManager changes the count
//         }
//     }

//     [Header("Refs")]
//     public CrystalSpawner spawner;  // assign the Spawner (with CrystalSpawner)

//     [Header("Audio")]
//     public AudioClip backgroundMusic;   // drag your music clip here
//     [Range(0f,1f)] public float musicVolume = 0.5f;
//     private AudioSource musicSource;

//     // state
//     int collected = 0;
//     int spawned   = 0;
//     float elapsed = 0f;
//     bool timerRunning = true;

//     void Awake()
//     {
//         if (spawner == null) spawner = FindObjectOfType<CrystalSpawner>();
//         if (scoreText == null) scoreText = FindByName<TMP_Text>("ScoreText");
//         if (timerText == null) timerText = FindByName<TMP_Text>("TimerText");
//         if (endText   == null) endText   = FindByName<TMP_Text>("EndText");

//         if (totalCrystals <= 0) totalCrystals = 10;
//         Time.timeScale = 1f;

//         // Music source setup
//         if (backgroundMusic != null)
//         {
//             musicSource = gameObject.AddComponent<AudioSource>();
//             musicSource.clip = backgroundMusic;
//             musicSource.loop = true;
//             musicSource.playOnAwake = false;
//             musicSource.spatialBlend = 0f;           // 2D
//             musicSource.volume = Mathf.Clamp01(musicVolume);
//         }

//         if (spawner == null) Debug.LogError("[GameManager] Spawner NOT assigned.");
//         if (timerText == null) Debug.LogWarning("[GameManager] TimerText NOT assigned.");
//         if (scoreText == null) Debug.LogWarning("[GameManager] ScoreText NOT assigned.");
//     }

//     void Start()
//     {
//         collected = 0;
//         spawned   = 0;
//         elapsed   = 0f;
//         timerRunning = true;

//         UpdateScoreUI();
//         Debug.Log("[GameManager] Start -> spawning first crystal");
//         SpawnNext();                        // sequential spawning: one at a time

//         // Start background music
//         if (musicSource != null) musicSource.Play();
//     }

//     void Update()
//     {
//         // Timer
//         if (timerRunning)
//         {
//             elapsed += Time.deltaTime;
//             if (timerText != null)
//             {
//                 int m = Mathf.FloorToInt(elapsed / 60f);
//                 int s = Mathf.FloorToInt(elapsed % 60f);
//                 timerText.text = $"Time : {m:00}:{s:00}";
//             }
//         }
//         else
//         {
//             // Restart key once game has ended (win or lose)
//             if (Input.GetKeyDown(KeyCode.R))
//             {
//                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//             }
//         }
//     }

//     // Called by Crystal.cs on pickup
//     public void CollectCrystal(GameObject crystal)
//     {
//         Debug.Log("[GameManager] CollectCrystal called");
//         if (crystal) Destroy(crystal);
//         AddScore(1);

//         if (collected < totalCrystals)
//             SpawnNext();
//     }

//     // Also used by Enemy.cs when you kill an enemy (kept for compatibility)
//     public void AddScore(int value)
//     {
//         collected += value;
//         UpdateScoreUI();
//         if (collected >= totalCrystals)
//             EndGame(true);
//     }

//     // For a goal/gate script if you use it
//     public void OnPlayerEnterGate()
//     {
//         EndGame(true);
//     }

//     void SpawnNext()
//     {
//         if (spawned >= totalCrystals)
//         {
//             Debug.Log("[GameManager] Not spawning: already spawned enough.");
//             return;
//         }
//         if (spawner == null)
//         {
//             Debug.LogError("[GameManager] Cannot spawn: spawner reference missing.");
//             return;
//         }

//         var go = spawner.SpawnOne();
//         if (go != null)
//         {
//             spawned++;
//             Debug.Log($"[GameManager] Spawned {spawned}/{totalCrystals}");
//         }
//     }

//     void UpdateScoreUI()
//     {
//         if (scoreText != null)
//             scoreText.text = $"Crystals : {collected}/{totalCrystals}";
//     }

//     // --- UPDATED ENDGAME ---
//     void EndGame(bool won)
//     {
//         if (!timerRunning) return;
//         timerRunning = false;

//         // Stop music when the game ends
//         if (musicSource != null && musicSource.isPlaying) musicSource.Stop();

//         if (endText != null)
//         {
//             endText.text = won ? "YOU WIN!" : "GAME OVER!";
//             endText.fontSize = 72;         // big
//             endText.color = Color.yellow;  // bright
//         }

//         Debug.Log(won ? "[GameManager] YOU WIN!" : "[GameManager] GAME OVER!");
//     }

//     public void GameOver() => EndGame(false);

//     // Utility to find by name if you forgot to wire a field
//     T FindByName<T>(string name) where T : Component
//     {
//         foreach (var t in FindObjectsOfType<T>(true))
//             if (t.name == name) return t;
//         return null;
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text endText;

    [Header("Rules")]
    [SerializeField] private int totalCrystals = 10;
    public int TotalCrystals
    {
        get => totalCrystals;
        set
        {
            totalCrystals = Mathf.Max(0, value);
            UpdateScoreUI();
        }
    }

    [Header("Refs")]
    public CrystalSpawner spawner;

    [Header("Audio")]
    public AudioClip backgroundMusic;
    [Range(0f,1f)] public float musicVolume = 0.5f;
    private AudioSource musicSource;

    [Header("Game Time Limit")]
    public float timeLimit = 60f;   // 60 seconds

    // state
    int collected = 0;
    int spawned   = 0;
    float elapsed = 0f;
    bool timerRunning = true;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        if (spawner == null) spawner = FindObjectOfType<CrystalSpawner>();
        if (scoreText == null) scoreText = FindByName<TMP_Text>("ScoreText");
        if (timerText == null) timerText = FindByName<TMP_Text>("TimerText");
        if (endText   == null) endText   = FindByName<TMP_Text>("EndText");

        if (totalCrystals <= 0) totalCrystals = 10;
        Time.timeScale = 1f;

        // Music setup
        if (backgroundMusic != null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.spatialBlend = 0f;
            musicSource.volume = Mathf.Clamp01(musicVolume);
        }
    }

    void Start()
    {
        collected = 0;
        spawned   = 0;
        elapsed   = 0f;
        timerRunning = true;

        UpdateScoreUI();
        SpawnNext();

        if (musicSource != null) musicSource.Play();
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsed += Time.deltaTime;

            // Update timer UI
            if (timerText != null)
            {
                int m = Mathf.FloorToInt(elapsed / 60f);
                int s = Mathf.FloorToInt(elapsed % 60f);
                timerText.text = $"Time : {m:00}:{s:00}";
            }

            // --- Lose condition if timer exceeded ---
            if (elapsed >= timeLimit && collected < totalCrystals)
            {
                EndGame(false);  // Game over
            }
        }
        else
        {
            // Allow restart after game ends
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void CollectCrystal(GameObject crystal)
    {
        if (crystal) Destroy(crystal);
        AddScore(1);

        if (collected < totalCrystals)
            SpawnNext();
    }

    public void AddScore(int value)
    {
        collected += value;
        UpdateScoreUI();
        if (collected >= totalCrystals)
            EndGame(true);  // Win
    }

    public void OnPlayerEnterGate() => EndGame(true);

    void SpawnNext()
    {
        if (spawned >= totalCrystals) return;
        if (spawner == null) return;

        var go = spawner.SpawnOne();
        if (go != null) spawned++;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Crystals : {collected}/{totalCrystals}";
    }

    void EndGame(bool won)
    {
        if (!timerRunning) return;
        timerRunning = false;

        if (musicSource != null && musicSource.isPlaying) musicSource.Stop();

        if (endText != null)
        {
            endText.text = won ? "YOU WIN!" : "GAME OVER!";
            endText.fontSize = 72;
            endText.color = won ? Color.yellow : Color.red;
        }

        Debug.Log(won ? "[GameManager] YOU WIN!" : "[GameManager] GAME OVER!");
    }

    public void GameOver() => EndGame(false);

    T FindByName<T>(string name) where T : Component
    {
        foreach (var t in FindObjectsOfType<T>(true))
            if (t.name == name) return t;
        return null;
    }
}
