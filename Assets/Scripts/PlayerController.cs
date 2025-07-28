using UnityEngine;

/// <summary>
/// Handles player movement and shooting.  The player moves in four directions
/// using the Horizontal and Vertical input axes (WASD/Arrow keys by default).
/// Movement uses a Rigidbody2D so that collisions are handled by Unity's
/// physics engine.  Shooting instantiates a projectile prefab at the given
/// spawn point and enforces a simple fire rate cooldown so the player
/// cannot fire continuously without delay.  When the player takes damage
/// (e.g. colliding with an enemy) the GameManager is notified to update
/// health and check game over conditions.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Units per second the player moves when input is fully pressed.")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    [Tooltip("Prefab to spawn when the player shoots a projectile.")]
    public GameObject projectilePrefab;
    [Tooltip("Transform indicating where projectiles should spawn (e.g. at the front of the player sprite).")]
    public Transform projectileSpawnPoint;
    [Tooltip("Minimum time between shots in seconds.")]
    public float fireRate = 0.5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private float nextFireTime;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Gather raw input (-1, 0, 1) on each axis.  Unity's Input system
        // automatically maps WASD and arrow keys to the Horizontal and
        // Vertical axes.  This approach is recommended for top‑down
        // movement because it yields crisp four‑directional control【266688935483207†L61-L88】.
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Handle shooting when the fire button (left mouse or Ctrl) is pressed.
        // A timer ensures the player cannot shoot more often than allowed by
        // fireRate.
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // Normalize the movement vector so diagonal movement isn’t faster,
        // then apply it to the Rigidbody2D’s velocity.  Using velocity
        // directly is efficient for top‑down games and is a typical
        // approach【266688935483207†L61-L88】.
        rb.velocity = movement.normalized * moveSpeed;
    }

    /// <summary>
    /// Instantiates a projectile and optionally rotates it to align with the
    /// current aim direction.  Rotation can be set on the projectile
    /// prefab itself.  Called internally by the Update method when the
    /// player presses Fire1.
    /// </summary>
    private void Shoot()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }

    /// <summary>
    /// Called by an enemy when colliding with the player.  Passes damage
    /// information to the GameManager so that health can be decreased and
    /// the UI updated.
    /// </summary>
    /// <param name="damage">Amount of health lost.</param>
    public void TakeDamage(int damage)
    {
        if (gameManager != null)
        {
            gameManager.PlayerTakeDamage(damage);
        }
    }
}