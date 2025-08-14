using UnityEngine;

public class WeaponThrower : MonoBehaviour
{
    public GameObject weaponPrefab;     // Префаб оружия для копий
    public Transform weaponPivot;       // Точка спавна копий оружия
    public GameObject enemy;            // Противник (Circle2)
    public float throwForce = 10f;      // Сила броска
    public float throwInterval = 1f;    // Интервал броска в секундах

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= throwInterval)
        {
            timer = 0f;
            ThrowWeaponClone();
        }
    }

    void ThrowWeaponClone()
    {
        if (weaponPrefab == null || weaponPivot == null || enemy == null)
            return;

        // Создаём копию оружия в позиции weaponPivot с тем же вращением
        GameObject clone = Instantiate(weaponPrefab, weaponPivot.position, weaponPivot.rotation);

        // Назначаем владельца и помечаем как НЕ клон (чтобы наносил урон)
        Weapon weaponComp = clone.GetComponent<Weapon>();
        if (weaponComp != null)
        {
            weaponComp.SetOwner(gameObject);
            weaponComp.isClone = false;  // Важно! Чтобы клон наносил урон
        }

        // Добавляем Rigidbody2D, если нет
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = clone.AddComponent<Rigidbody2D>();
        }

        // Вычисляем направление на противника и задаём скорость броска
        Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;
        rb.linearVelocity = directionToEnemy * throwForce;

        rb.gravityScale = 0f;          // Отключаем гравитацию для копии
        rb.angularVelocity = 360f;     // Задаём вращение копии (угловая скорость)
    }
}