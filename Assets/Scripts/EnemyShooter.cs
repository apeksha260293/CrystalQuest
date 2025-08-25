using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyShooter : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject projectilePrefab;     // assign EnemyProjectile prefab
    public float projectileSpeed = 8f;
    [Tooltip("How far in front of the enemy the bullet is spawned.")]
    public float spawnDistance = 0.8f;      // a bit larger so we never spawn inside the enemy

    [Header("Shooting")]
    public float fireRate = 0.8f;           // base shots/sec (reduce spam)
    [Range(0f, 0.75f)] public float cooldownJitter = 0.35f; // randomize cadence
    public float range = 7f;                // only shoot if player is within this distance
    [Tooltip("Max bullets this enemy can have alive at the same time.")]
    public int maxLiveProjectiles = 2;

    Transform player;
    float nextShotTime;
    readonly List<GameObject> live = new();

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void OnEnable()
    {
        ScheduleNextShot();
        live.Clear();
    }

    void Update()
    {
        if (!player || !projectilePrefab) return;

        // only shoot if in range
        Vector2 toPlayer = (player.position - transform.position);
        if (toPlayer.sqrMagnitude > range * range) return;

        // prune destroyed bullets
        for (int i = live.Count - 1; i >= 0; i--)
            if (!live[i]) live.RemoveAt(i);

        if (live.Count >= maxLiveProjectiles) return;   // cap spam

        if (Time.time >= nextShotTime)
        {
            Shoot(toPlayer.normalized);
            ScheduleNextShot();
        }
    }

    void Shoot(Vector2 dir)
    {
        Vector3 spawnPos = (Vector2)transform.position + dir * spawnDistance;
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);

        var go = Instantiate(projectilePrefab, spawnPos, rot);
        live.Add(go);

        // Hand it its velocity right away – prevents “stuck” look on spawn frames.
        if (go.TryGetComponent<Rigidbody2D>(out var rb))
            rb.velocity = dir * projectileSpeed;

        if (go.TryGetComponent<EnemyProjectile>(out var ep))
            ep.Launch(dir, projectileSpeed);
    }

    void ScheduleNextShot()
    {
        // randomize around fireRate to avoid all enemies syncing
        float baseCd = 1f / Mathf.Max(0.0001f, fireRate);
        float jitter = Random.Range(-cooldownJitter, cooldownJitter) * baseCd;
        nextShotTime = Time.time + baseCd + jitter;
    }
}
