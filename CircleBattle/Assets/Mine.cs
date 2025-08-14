using UnityEngine;

public class Mine : MonoBehaviour
{
    [Tooltip("Урон, который наносит мина")]
    public int damage = 3; // Теперь регулируется в инспекторе

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что объект — Circle1
        if (other.gameObject.name == "Circle1")
        {
            DamageReceiver receiver = other.GetComponent<DamageReceiver>();
            if (receiver != null)
            {
                // Передаем true, чтобы игнорировать проверку неуязвимости
                receiver.TakeDamage(damage, gameObject, true);
            }

            // Уничтожаем мину после активации
            Destroy(gameObject);
        }
    }
}