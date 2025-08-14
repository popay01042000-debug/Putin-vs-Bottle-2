using System.Collections;
using TMPro;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public int health = 100;
    public float invincibilityDuration = 0.5f;
    private float lastDamageTime = -999f;
    public float hitPauseDuration = 0.2f;

    public TextMeshPro healthText;
    public AudioSource hitSoundSource;

    [SerializeField] public AudioClip critSoundClip;  // Звук крит удара
    [SerializeField] public GameObject bloodPrefab;   // Префаб пятна крови

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private AudioSource audioSource;

    private Coroutine critFlashCoroutine;

    // Кровотечение
    private int bleedDamagePerTick = 0;
    private bool bleedActive = false;

    void Start()
    {
        UpdateHealthUI();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public bool TakeDamage(int amount, GameObject attacker, bool ignoreInvincibility = false, bool isPoison = false)
    {
        if (!ignoreInvincibility && Time.time - lastDamageTime < invincibilityDuration)
            return false;

        health -= amount;
        lastDamageTime = Time.time;
        UpdateHealthUI();

        if (hitSoundSource != null)
            hitSoundSource.Play();

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        if (bloodPrefab != null)
        {
            Vector2 bloodPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
            Quaternion bloodRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(bloodPrefab, bloodPosition, bloodRotation);
        }

        StartCoroutine(HitPauseColorRoutine());

        // Обработка оружия
        if (attacker != null)
        {
            WeaponRotation weaponRotation = attacker.GetComponentInChildren<WeaponRotation>();
            if (weaponRotation != null)
            {
                // Меняем направление вращения
                weaponRotation.rotationSpeed *= -1;

                // Если удар не ядовитый — пауза
                if (!isPoison)
                    weaponRotation.PauseRotation(hitPauseDuration);
            }

            CircleMovement attackerMovement = attacker.GetComponent<CircleMovement>();
            if (attackerMovement != null && !isPoison)
                attackerMovement.StartHitPause(hitPauseDuration);
        }

        // Сам тоже паузится, если удар не ядовитый
        if (!isPoison)
        {
            CircleMovement selfMovement = GetComponent<CircleMovement>();
            if (selfMovement != null)
                selfMovement.StartHitPause(hitPauseDuration);
        }

        // Кровотечение при ударе оружием Circle2
        if (attacker != null && attacker.name.Contains("Circle2Weapon"))
        {
            bleedDamagePerTick += 1; // усиливаем кровотечение
            if (!bleedActive)
            {
                bleedActive = true;
                StartCoroutine(BleedDamageRoutine());
            }
        }

        if (health <= 0)
        {
            Debug.Log("Круг уничтожен!");
            Destroy(gameObject);
        }

        return true;
    }

    private IEnumerator BleedDamageRoutine()
    {
        while (bleedActive && health > 0)
        {
            yield return new WaitForSeconds(3f);
            health -= bleedDamagePerTick;
            UpdateHealthUI();

            if (bloodPrefab != null)
            {
                Vector2 bloodPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
                Quaternion bloodRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                Instantiate(bloodPrefab, bloodPosition, bloodRotation);
            }

            if (health <= 0)
            {
                Debug.Log("Круг умер от кровотечения!");
                Destroy(gameObject);
                yield break;
            }
        }
    }

    private IEnumerator HitPauseColorRoutine()
    {
        yield return new WaitForSeconds(hitPauseDuration);
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    // Метод для мигания красным при крите
    public void FlashRed(float duration)
    {
        if (spriteRenderer == null)
            return;

        if (critFlashCoroutine != null)
            StopCoroutine(critFlashCoroutine);

        critFlashCoroutine = StartCoroutine(FlashRedCoroutine(duration));
    }

    private IEnumerator FlashRedCoroutine(float duration)
    {
        spriteRenderer.color = Color.red;
        if (audioSource != null && critSoundClip != null)
            audioSource.PlayOneShot(critSoundClip);

        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
        critFlashCoroutine = null;
    }

    public void Heal(int amount)
    {
        health += amount;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = health.ToString();
    }
}
