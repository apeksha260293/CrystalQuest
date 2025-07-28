using UnityEngine;

/// <summary>
/// Basic enemy behaviour for Crystal Quest.  Enemies seek the player
/// continuously by moving towards their position.  On collision the
/// enemy deals damage to the player and is destroyed.  Each enemy has
/// a health value and can be damaged by projectiles.  When destroyed
/// the GameManager updates the score.
/// </summary>
public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed of the enemy in units per second.")]
    public float moveSpeed = 2f;
    [Tooltip("Starting health of the enemy.")]
    public int health = 1;

    private Transform target;
    private Rigidbody2D rb;
    private GameManager gameManager;

    void Start()
    {
        // Cache the target (player) and required components
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Seek the player each frame.  Normalize the vector so speed is
        // constant regardless of distance.
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    /// <summary>
    /// Applies damage to the enemy.  When health reaches zero the enemy
    /// increments the player’s score and destroys itself.
    /// </summary>
    /// <param name="damage">Amount of damage applied.</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameManager != null)
            {
                // Add a point for destroying an enemy
                gameManager.AddScore(1);
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // When colliding with the player, deal damage then destroy this enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}