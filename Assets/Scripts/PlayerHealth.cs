using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int max = 100;
    int current;                                 // <- private so Inspector won't freeze it

    [Header("UI")]
    [SerializeField] TMP_Text healthText;        // drag GameCanvas/HealthText here

    void Awake()
    {
        if (!healthText)
        {
            var go = GameObject.Find("HealthText");
            if (go) healthText = go.GetComponent<TMP_Text>();
        }
        ResetHealth();
    }

    public void ResetHealth()
    {
        current = max;                            // <- always start full
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        current = Mathf.Clamp(current - Mathf.Abs(amount), 0, max);
        UpdateUI();
        if (current <= 0)
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm) gm.GameOver();
        }
    }

    public void Heal(int amount)
    {
        current = Mathf.Clamp(current + Mathf.Abs(amount), 0, max);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthText) healthText.text = $"Health : {current}";
    }
}
