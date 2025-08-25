using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject crystalPrefab;

    [Header("Screen Clamp (world units)")]
    [Tooltip("Padding from the visible screen edges.")]
    public float edgePadding = 0.5f;

    [Header("Safety")]
    [Tooltip("Minimum distance from the player when spawning.")]
    public float minDistanceFromPlayer = 1.5f;

    [Tooltip("Maximum attempts to find a valid spot this frame.")]
    public int maxTries = 20;

    Transform player;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    /// <summary>
    /// Spawns one crystal at a random on-screen position.
    /// Returns the instance or null on failure.
    /// </summary>
    public GameObject SpawnOne()
    {
        if (!crystalPrefab)
        {
            Debug.LogError("[CrystalSpawner] No crystalPrefab assigned.");
            return null;
        }

        if (!cam)
        {
            cam = Camera.main;
            if (!cam)
            {
                Debug.LogError("[CrystalSpawner] No Camera.main found.");
                return null;
            }
        }

        // Visible world rect from orthographic camera, centered at the camera position
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float minX = cam.transform.position.x - halfW + edgePadding;
        float maxX = cam.transform.position.x + halfW - edgePadding;
        float minY = cam.transform.position.y - halfH + edgePadding;
        float maxY = cam.transform.position.y + halfH - edgePadding;

        Vector3 pos = Vector3.zero;
        bool found = false;

        for (int i = 0; i < Mathf.Max(1, maxTries); i++)
        {
            pos.x = Random.Range(minX, maxX);
            pos.y = Random.Range(minY, maxY);
            pos.z = 0f; // keep on gameplay plane

            if (player == null || Vector2.Distance(pos, player.position) >= minDistanceFromPlayer)
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            // Fallback: clamp the camera center into the visible rect
            pos = new Vector3(
                Mathf.Clamp(cam.transform.position.x, minX, maxX),
                Mathf.Clamp(cam.transform.position.y, minY, maxY),
                0f
            );
        }

        var go = Instantiate(crystalPrefab, pos, Quaternion.identity);

        // Ensure it renders above backgrounds if needed
        var sr = go.GetComponentInChildren<SpriteRenderer>();
        if (sr)
        {
            if (string.IsNullOrEmpty(sr.sortingLayerName)) sr.sortingLayerName = "Default";
            sr.sortingOrder = Mathf.Max(sr.sortingOrder, 10); // push in front, tweak as you like
        }

        // Make sure a trigger collider exists (so the player can collect it)
        var col = go.GetComponent<Collider2D>();
        if (col) col.isTrigger = true;

        Debug.Log($"[CrystalSpawner] Spawned crystal at ({pos.x:F2}, {pos.y:F2})");
        return go;
    }
}
