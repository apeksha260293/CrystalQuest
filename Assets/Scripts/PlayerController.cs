using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [Header("Shooting")]
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float fireRate = 0.25f;

    [Header("Bomb")]
    public KeyCode bombKey = KeyCode.B;

    Vector2 movement;
    float nextFireTime;
    GameManager gameManager;

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        gameManager = FindObjectOfType<GameManager>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 0.01f)
            transform.right = movement.normalized; // face where we move

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyDown(bombKey))
            gameManager?.UseBomb();
    }

    void FixedUpdate()
    {
        if (!rb) return;
        rb.velocity = movement.normalized * moveSpeed;
    }

    void Shoot()
    {
        if (!projectilePrefab || !projectileSpawnPoint) return;
        var p = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        p.transform.right = transform.right;
    }

    // called by enemies on collision
    public void TakeDamage(int amount)
    {
        gameManager?.PlayerTakeDamage(amount);
    }
}
