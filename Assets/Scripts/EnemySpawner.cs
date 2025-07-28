using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns enemies at regular intervals within a defined radius.  The
/// coroutine approach allows flexible control over spawn timing and is
/// recommended for dynamic spawns【340535937812746†L82-L123】.  This spawner
/// continuously instantiates enemy prefabs around its transform.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Enemy prefab to spawn.  Prefab must have an Enemy script attached.")]
    public GameObject enemyPrefab;
    [Tooltip("Radius within which new enemies can spawn.  Set a value that suits your level size.")]
    public float spawnRadius = 5f;
    [Tooltip("Time between spawns in seconds.")]
    public float spawnInterval = 10f;
    private bool spawning = true;

    void Start()
    {
        // Begin the spawn coroutine at game start
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // Spawn enemies forever until deactivated
        while (spawning)
        {
            SpawnEnemy();
            // Wait for the specified interval before spawning the next
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Instantiates a new enemy at a random position around the spawner.  Uses
    /// Random.insideUnitCircle to find a position within a circle of the
    /// specified radius, and converts it to world space【340535937812746†L46-L70】.
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        // Random point within a circle around the spawner
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPos = (Vector2)transform.position + offset;
        // Instantiate the enemy
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}