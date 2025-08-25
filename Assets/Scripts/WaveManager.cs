using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Crystals")]
    public GameObject crystalPrefab;     // prefab with Crystal.cs + trigger collider
    public Vector2[] crystalPositions;   // list of positions to spawn

    [Header("Spawners")]
    public EnemySpawner[] spawners;      // optional enemy spawners to ramp difficulty

    private GameManager gameManager;

    public void SetGameManager(GameManager gm) => gameManager = gm;

    public void StartWave(int waveNumber)
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("[WaveManager] No GameManager found in scene.");
            return;
        }

        // Clear any existing crystals in the scene
        foreach (var c in FindObjectsOfType<Crystal>())
            if (c) Destroy(c.gameObject);

        // Spawn the new set of crystals
        int count = 0;
        if (crystalPrefab != null && crystalPositions != null)
        {
            foreach (var pos in crystalPositions)
            {
                Instantiate(crystalPrefab, (Vector3)pos, Quaternion.identity);
                count++;
            }
        }
        else
        {
            Debug.LogWarning("[WaveManager] crystalPrefab or crystalPositions not set.");
        }

        // Tell GameManager how many must be collected this wave
        gameManager.TotalCrystals = count;

        // Optional: ramp enemy spawn difficulty each wave
        if (spawners != null)
        {
            foreach (var s in spawners)
            {
                if (!s) continue;
                float newInterval = s.spawnInterval - 0.5f * (waveNumber - 1);
                s.spawnInterval = Mathf.Max(1f, newInterval);
            }
        }

        Debug.Log($"[WaveManager] Wave {waveNumber} started. Crystals spawned: {count}");
    }
}
