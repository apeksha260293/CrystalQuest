using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public Enemy[] enemies;
        public int count; // number of spawns per wave
        public float timeBetweenSpawns;
    }

    public Wave[] waves;
    public Transform[] spawnPoints; // points to where the waves will be spawned
    public float timeBetweenWaves;

    /* Private methods */
    private Wave currentWave;
    private int currentWaveIndex = 0;
    private Transform player;
    private bool spawningFinished;

    public GameObject boss;
    public Transform bossSpawnPoint;

    public Slider bossHealthBar;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CallNextWave(currentWaveIndex));
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.Log("LOST!");
        }
        else
        {
            if (spawningFinished == true && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                spawningFinished = false;
                if (waves.Length > 1 && currentWaveIndex != (waves.Length - 1))
                {
                    currentWaveIndex++;
                    StartCoroutine(CallNextWave(currentWaveIndex));
                }
                else
                {
                    // Instantiate boss
                    Instantiate(boss, bossSpawnPoint.position, bossSpawnPoint.rotation);
                    bossHealthBar.gameObject.SetActive(true);
                }
            }
        }
    }

    IEnumerator CallNextWave(int index)
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
    }

    IEnumerator SpawnWave(int index)
    {
        currentWave = waves[index];

        for (int i = 0; i < currentWave.count; i++)
        {
            if (player == null)
            {
                yield break;
            }

            Enemy randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpawnPoint.position, randomSpawnPoint.rotation);

            if (i == currentWave.count - 1)
            { // spawns per wave are over
                spawningFinished = true;
            }
            else
            {
                spawningFinished = false;
            }

            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
        }
    }

}
