using UnityEngine;

public class GateController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var gm = Object.FindObjectOfType<GameManager>();
        gm?.OnPlayerEnterGate();
    }
}
