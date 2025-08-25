using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyProjectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;
    public float lifetime = 3f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Make sure this behaves like a simple laser:
        rb.gravityScale = 0f;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;                 // IMPORTANT: avoid getting stuck on geometry
    }

    void OnEnable()
    {
        // If shooter didn’t call Launch, fly to the right by default.
        if (rb.velocity.sqrMagnitude < 0.01f)
            rb.velocity = transform.right * speed;

        Invoke(nameof(DestroySelf), lifetime);
    }

    /// Shooter calls this immediately after Instantiate
    public void Launch(Vector2 dir, float overrideSpeed = -1f)
    {
        Vector2 d = dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector2.right;
        float v = (overrideSpeed > 0f) ? overrideSpeed : speed;

        transform.right = d;                  // rotate sprite to travel direction
        rb.velocity = d * v;                  // give it velocity now
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var hp = other.GetComponent<PlayerHealth>();
            if (hp) hp.TakeDamage(damage);
            DestroySelf();
        }

        // Optional: if you have walls/obstacles on a layer tag them and destroy on hit:
        // if (other.CompareTag("Obstacle")) DestroySelf();
    }

    void OnBecameInvisible()  // bullet left the camera -> clean up
    {
        DestroySelf();
    }

    void DestroySelf()
    {
        if (this) Destroy(gameObject);
    }
}
