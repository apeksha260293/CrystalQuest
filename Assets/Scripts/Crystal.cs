using UnityEngine;

/// <summary>
/// Represents a collectible crystal.  When the player enters the
/// trigger, the crystal notifies the GameManager so that the score and
/// crystal count can be updated, then destroys itself.
/// </summary>
public class Crystal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.CollectCrystal(gameObject);
            }
        }
    }
}