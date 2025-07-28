using UnityEngine;
using TMPro;

/// <summary>
/// Central manager that tracks player health, score, collected crystals
/// and updates UI elements accordingly.  It also handles win and loss
/// conditions: collecting all crystals triggers a win state, while
/// health falling to zero triggers a game over.  The manager
/// exposes methods called by other scripts to modify state.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Starting health of the player.")]
    public int playerHealth = 3;

    [Header("Crystal Settings")]
    [Tooltip("Number of crystals that must be collected to win.")]
    public int totalCrystals = 0;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text timerText;
    public TMP_Text endText;

    private int collectedCrystals;
    private int score;
    private float startTime;
    private bool gameEnded;

    void Start()
    {
        startTime = Time.time;
        UpdateUI();
    }

    void Update()
    {
        // Update the timer display while the game is active
        if (!gameEnded)
        {
            float elapsed = Time.time - startTime;
            int minutes = (int)(elapsed / 60f);
            int seconds = (int)(elapsed % 60f);
            if (timerText != null)
            {
                timerText.text = $"Time: {minutes:00}:{seconds:00}";
            }
        }
    }

    /// <summary>
    /// Called by PlayerController when the player takes damage.  Decrements
    /// health and ends the game if health is depleted.
    /// </summary>
    /// <param name="damage">Damage amount.</param>
    public void PlayerTakeDamage(int damage)
    {
        if (gameEnded) return;
        playerHealth -= damage;
        UpdateUI();
        if (playerHealth <= 0)
        {
            EndGame(false);
        }
    }

    /// <summary>
    /// Called by Crystal when collected.  Increases the score and checks
    /// for win condition.
    /// </summary>
    /// <param name="crystal">The crystal GameObject collected.</param>
    public void CollectCrystal(GameObject crystal)
    {
        if (gameEnded) return;
        collectedCrystals++;
        // Destroy the crystal
        Destroy(crystal);
        // Each crystal adds 5 points
        AddScore(5);
        if (collectedCrystals >= totalCrystals)
        {
            EndGame(true);
        }
    }

    /// <summary>
    /// Adds to the player's score and updates the UI.
    /// </summary>
    /// <param name="amount">Points to add.</param>
    public void AddScore(int amount)
    {
        if (gameEnded) return;
        score += amount;
        UpdateUI();
    }

    /// <summary>
    /// Updates the on‑screen health and score labels.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        if (healthText != null)
        {
            healthText.text = $"Health: {playerHealth}";
        }
    }

    /// <summary>
    /// Ends the game, sets the end text, and stops further updates.  Use
    /// this method to display win/lose messages.  Additional logic
    /// (reloading scenes, stopping spawners) can be added here.
    /// </summary>
    /// <param name="won">True if the player collected all crystals.</param>
    private void EndGame(bool won)
    {
        gameEnded = true;
        if (endText != null)
        {
            endText.gameObject.SetActive(true);
            endText.text = won ? "You Win!" : "Game Over";
        }
    }
}