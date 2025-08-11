using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Crystals")]
    public GameObject crystalPrefab;
    public Vector2[] crystalPositions;

    [Header("Spawners")]
    public EnemySpawner[] spawners;

    GameManager gameManager;

    public void SetGameManager(GameManager gm) => gameManager = gm;

    public void StartWave(int waveNumber)
    {
        if (gameManager == null) gameManager = FindObjectOfType<GameManager>();

        // clear old crystals
        foreach (Crystal c in FindObjectsOfType<Crystal>())
            Destroy(c.gameObject);

        // spawn new crystals
        int count = 0;
        if (crystalPrefab != null && crystalPositions != null)
        {
            foreach (var pos in crystalPositions)
            {
                Instantiate(crystalPrefab, (Vector3)pos, Quaternion.identity);
                count++;
            }
        }
        gameManager.totalCrystals = count;

        // ramp difficulty: spawn faster each wave
        if (spawners != null)
        {
            foreach (var s in spawners)
            {
                if (!s) continue;
                float newInterval = s.spawnInterval - 0.5f * (waveNumber - 1);
                s.spawnInterval = Mathf.Max(1f, newInterval);
            }
        }
    }
}
