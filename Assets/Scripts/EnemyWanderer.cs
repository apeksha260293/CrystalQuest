// Assets/Scripts/EnemyWanderer.cs
using UnityEngine;

public class EnemyWanderer : Enemy
{
    [Tooltip("Seconds between random direction changes.")]
    public float changeDirectionInterval = 0.5f;

    float timer;
    Vector2 dir;

    protected override void Start()
    {
        base.Start();
        PickNewDir();
    }

    protected override void Update()
    {
        // wander instead of chasing; reuse rb from base
        timer -= Time.deltaTime;
        if (timer <= 0f) PickNewDir();

        if (rb != null) rb.velocity = dir * moveSpeed;
    }

    void PickNewDir()
    {
        dir = Random.insideUnitCircle.normalized;
        timer = changeDirectionInterval;
    }
}

