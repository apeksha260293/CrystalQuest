using UnityEngine;

/// Basic enemy that seeks the player. Subclasses can override behaviour.
public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed of the enemy (units/sec).")]
    public float moveSpeed = 2f;

    [Tooltip("Starting health of the enemy.")]
    public int health = 1;

    // Protected so subclasses (e.g., EnemyWanderer) can access.
    protected Transform target;
    protected Rigidbody2D rb;
    protected GameManager gameManager;

    // Make Start/Update virtual so they can be overridden.
    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Update()
    {
        if (target == null || rb == null) return;
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.velocity = dir * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameManager != null) gameManager.AddScore(1);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            var player = c.gameObject.GetComponent<PlayerController>();
            if (player != null) player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
