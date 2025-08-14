using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject owner;
    public bool isClone = false;

    private int hitsLandedByCircle1 = 0;
    private int hitsLandedByCircle2 = 0;

    [SerializeField, Range(0f, 1f)]
    private float mineSpawnChance = 0.20f;

    [SerializeField]
    private int critDamage = 5;

    // Урон Circle2
    private int baseDamageCircle2 = 1;
    private int currentDamageCircle2;

    public int HitsLandedByCircle1 => hitsLandedByCircle1;
    public int HitsLandedByCircle2 => hitsLandedByCircle2;
    public float MineSpawnChance => mineSpawnChance;
    public int CritDamage => critDamage;

    public int CurrentDamageCircle2 => currentDamageCircle2; // <-- Для UI

    void Start()
    {
        currentDamageCircle2 = baseDamageCircle2;
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Circle") && collision.gameObject != owner)
        {
            GameObject target = collision.gameObject;
            DamageReceiver targetReceiver = target.GetComponent<DamageReceiver>();
            if (targetReceiver == null) return;

            bool attackerIsCircle1 = owner != null && owner.name.Contains("Circle1");
            bool targetIsCircle2 = target.name.Contains("Circle2");

            if (attackerIsCircle1 && targetIsCircle2)
            {
                hitsLandedByCircle1++;

                float critChance = 0.1f + (hitsLandedByCircle1 * 0.01f);
                if (Random.value <= critChance)
                {
                    targetReceiver.TakeDamage(critDamage, owner);
                    targetReceiver.FlashRed(0.2f);
                }
                else
                {
                    targetReceiver.TakeDamage(1, owner);
                }
            }

            bool attackerIsCircle2 = owner != null && owner.name.Contains("Circle2");
            bool targetIsCircle1 = target.name.Contains("Circle1");

            if (attackerIsCircle2 && targetIsCircle1)
            {
                targetReceiver.TakeDamage(currentDamageCircle2, owner);

                hitsLandedByCircle2++;
                if (hitsLandedByCircle2 % 4 == 0) // каждые 3 удара +1 урон
                {
                    currentDamageCircle2++;
                }
            }

            Weapon otherWeapon = collision.GetComponent<Weapon>();
            if (otherWeapon != null)
            {
                WeaponRotation myRotation = GetComponentInParent<WeaponRotation>();
                WeaponRotation otherRotation = otherWeapon.GetComponentInParent<WeaponRotation>();

                if (myRotation != null) myRotation.FlipDirection();
                if (otherRotation != null) otherRotation.FlipDirection();
            }
        }
    }
}
