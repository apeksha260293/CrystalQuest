using UnityEngine;

public class EnemyWanderer : Enemy
{
    public float changeDirectionInterval = 0.5f;

    float timer;
    Vector2 dir;

    protected override void Start()
    {
        base.Start();
        PickNewDir();
    }

    void Update()
    {
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
