using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 2f;
    public int damage = 1;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // Called immediately after Instantiate
    public void Init(Vector2 dir)
    {
        rb.velocity = dir.normalized * speed;
        CancelInvoke();
        Invoke(nameof(SelfDestruct), lifetime);
    }

    void SelfDestruct() { if (this) Destroy(gameObject); }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var e = other.GetComponent<Enemy>();
            if (e) e.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
