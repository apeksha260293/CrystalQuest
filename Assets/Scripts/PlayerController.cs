using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;          // drag Player/Sprite here (optional)

    [Header("Shooting")]
    public GameObject projectilePrefab;            // drag your Projectile prefab
    public Transform projectileSpawnPoint;         // child under Player
    public float fireRate = 6f;                    // shots/sec
    public Vector2 muzzleLocalOffset = new Vector2(0.35f, 0f); // offset from Player

    Rigidbody2D rb;
    Vector2 input;
    float nextFireTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Find OR create one spawn point (as a child)
        if (!projectileSpawnPoint)
        {
            var t = transform.Find("ProjectileSpawnPoint");
            if (!t)
            {
                var go = new GameObject("ProjectileSpawnPoint");
                t = go.transform;
                t.SetParent(transform);
            }
            projectileSpawnPoint = t;
        }

        // Ensure it's a clean transform (no physics/scripts on the spawn point)
        foreach (var c in projectileSpawnPoint.GetComponents<Component>())
        {
            if (c is Transform) continue;
            DestroyImmediate(c);
        }

        projectileSpawnPoint.localPosition = new Vector3(muzzleLocalOffset.x, muzzleLocalOffset.y, 0f);
        projectileSpawnPoint.localRotation = Quaternion.identity;
    }

    void Update()
    {
        // Movement input
        float x = 0f, y = 0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x = 1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  x = -1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    y = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  y = -1f;
        input = new Vector2(x, y).normalized;

        // Face the sprite
        if (spriteRenderer && Mathf.Abs(input.x) > 0.02f)
            spriteRenderer.flipX = input.x < 0f;

        // Keep spawn point parented and positioned in front of the face
        if (projectileSpawnPoint.parent != transform)
            projectileSpawnPoint.SetParent(transform);

        float dirSign = (spriteRenderer && spriteRenderer.flipX) ? -1f : 1f;
        projectileSpawnPoint.localPosition = new Vector3(muzzleLocalOffset.x * dirSign, muzzleLocalOffset.y, 0f);
        projectileSpawnPoint.right = (dirSign > 0f) ? Vector3.right : Vector3.left;

        // Fire (hold Space), rate-limited
        if (projectilePrefab && projectileSpawnPoint &&
            Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            var bullet = Instantiate(projectilePrefab,
                                     projectileSpawnPoint.position,
                                     projectileSpawnPoint.rotation);

            // Give velocity instantly (works even if Projectile.cs also sets it)
            if (bullet.TryGetComponent<Rigidbody2D>(out var rbBullet))
            {
                float projSpeed = 12f;
                if (bullet.TryGetComponent<Projectile>(out var p)) projSpeed = p.speed;
                rbBullet.velocity = projectileSpawnPoint.right * projSpeed;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = input * speed;
    }
}
