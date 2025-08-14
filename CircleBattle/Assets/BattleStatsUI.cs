using TMPro;
using UnityEngine;

public class BattleStatsUI : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI circle2DamageText; // Урон Circle2
    public TextMeshProUGUI critChanceText;    // Шанс крита Circle1

    [Header("Game Objects")]
    public GameObject circle1;
    public GameObject circle2;

    private Weapon circle1Weapon;
    private Weapon circle2Weapon;

    void Start()
    {
        if (circle1 != null)
            circle1Weapon = circle1.GetComponentInChildren<Weapon>();

        if (circle2 != null)
            circle2Weapon = circle2.GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        // Выводим актуальный урон Circle2
        if (circle2Weapon != null)
        {
            circle2DamageText.text = $"Damage: {circle2Weapon.CurrentDamageCircle2}";
        }

        // Выводим шанс крита Circle1
        if (circle1Weapon != null)
        {
            int hits = circle1Weapon.HitsLandedByCircle1;
            float critChance = 0.1f + (hits * 0.01f); // 10% + 1% за попадание
            if (critChance > 1f) critChance = 1f;

            critChanceText.text = $"Crit chance: {(critChance * 100f):F1}%";
        }
    }
}
