using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Crystal : MonoBehaviour
{
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"{name}: Crystal collider was not a Trigger, fixing now.");
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{name}: OnTriggerEnter2D with {other.name}, tag={other.tag}");
        if (!other.CompareTag("Player")) return;

        var gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            Debug.Log($"{name}: notifying GameManager.CollectCrystal");
            gm.CollectCrystal(gameObject);
        }
        else
        {
            Debug.LogError("GameManager not found in scene.");
        }
    }
}
