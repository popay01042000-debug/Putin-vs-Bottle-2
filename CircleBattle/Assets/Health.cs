using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public TextMeshPro healthText; // присвоим через инспектор

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthText();

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
