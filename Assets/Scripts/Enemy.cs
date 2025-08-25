// Enemy.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed of the enemy (units/sec).")]
    public float moveSpeed = 2f;

    [Tooltip("Starting health of the enemy.")]
    public int health = 3;

    [Header("Audio")]
    public AudioClip deathSound;

    protected Transform target;
    protected Rigidbody2D rb;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) target = playerObj.transform;

        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 2D
    }

    protected virtual void Update()
    {
        if (!target || !rb) return;

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.velocity = dir * moveSpeed;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            if (deathSound) audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, 0.1f); // small delay to let sound play
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<PlayerHealth>();
        if (hp) hp.TakeDamage(10);
        Destroy(gameObject);
    }
}
