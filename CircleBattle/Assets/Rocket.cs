using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float initialSpeed = 2f;         // Начальная скорость ракеты
    public float acceleration = 5f;         // Ускорение ракеты
    public int damage = 20;                  // Урон, наносимый ракеты
    public float maxSpeed = 20f;             // Максимальная скорость ракеты

    private GameObject target;               // Цель (Circle1)
    private GameObject owner;                // Владелец (Circle2)

    private Rigidbody2D rb;
    private float currentSpeed;

    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentSpeed = initialSpeed;
    }

    void FixedUpdate()
    {
        if (target == null || hasHit)
            return;

        Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;

        // Увеличиваем скорость с ускорением
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);

        rb.linearVelocity = direction * currentSpeed;

        // Повернуть ракету по направлению движения
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    public void SetTargetAndOwner(GameObject target, GameObject owner)
    {
        this.target = target;
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
            return;

        if (collision.gameObject == target)
        {
            hasHit = true;

            // Наносим урон цели
            DamageReceiver targetDamage = target.GetComponent<DamageReceiver>();
            if (targetDamage != null)
            {
                targetDamage.TakeDamage(damage, owner, true);
            }

            // Здесь можно вызвать у владельца Circle2 какой-то метод, если нужно снять фриз

            // Уничтожаем ракету
            Destroy(gameObject);
        }
        else if (collision.gameObject != owner)
        {
            // Ракета столкнулась с чем-то другим — тоже уничтожаем
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
