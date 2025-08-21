using UnityEngine;

/// <summary>
/// Represents a simple projectile fired by the player.  The projectile
/// travels in the direction given by its transform's right vector and
/// damages enemies on contact.  It self‑destructs after a configurable
/// lifetime to avoid lingering objects in the scene.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Tooltip("Speed of the projectile in units per second.")]
    public float speed = 10f;
    [Tooltip("Amount of damage inflicted on an enemy when hit.")]
    public int damage = 1;
    [Tooltip("Time in seconds before the projectile is automatically destroyed.")]
    public float lifetime = 5f;

    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move forward in the local right direction; multiply by deltaTime
        // to ensure frame‑rate independent motion
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When colliding with an enemy, apply damage and destroy the projectile
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            // Optionally destroy projectile when hitting walls or obstacles
            Destroy(gameObject);
        }
    }
}