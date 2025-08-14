using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage(int amount, GameObject attacker)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} получил {amount} урона. Осталось HP: {health}");
    }
}
