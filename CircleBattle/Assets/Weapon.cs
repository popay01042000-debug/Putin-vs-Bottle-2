using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject owner; // �������� ������

    [Header("Knockback & Slow Motion")]
    public float knockbackForce = 5f;
    public float slowMotionScale = 0.2f;
    public float slowMotionDuration = 0.4f;

    [Header("Hit Effect & Sound")]
    public float hitFlashDuration = 0.2f; // ������������ ������ �����
    public AudioClip hitSound;
    private AudioSource audioSource;

    private void Awake()
    {
        // ���� AudioSource � ���������
        audioSource = owner.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������ � ����� ����
        if (collision.CompareTag("Circle") && collision.gameObject != owner)
        {
            // ������������
            Rigidbody2D targetRb = collision.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                Vector2 knockDir = (collision.transform.position - owner.transform.position).normalized;
                targetRb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            }

            // ���������� �������
            StartCoroutine(SlowMotion());

            // ���� �����
            if (audioSource != null && hitSound != null)
                audioSource.PlayOneShot(hitSound);

            // ����� ����
            CircleColor circleColor = collision.GetComponent<CircleColor>();
            if (circleColor != null)
                circleColor.FlashWhite(hitFlashDuration);

            // ������� 1 ����
            Health health = collision.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(1);

            // ������ ����������� �������� ������ ����������
            WeaponRotation rotation = owner.GetComponentInChildren<WeaponRotation>();
            if (rotation != null)
                rotation.FlipDirection();
        }

        // ������ vs ������
        if (collision.CompareTag("Weapon") && collision.gameObject != gameObject)
        {
            WeaponRotation myRotation = GetComponentInParent<WeaponRotation>();
            WeaponRotation otherRotation = collision.GetComponentInParent<WeaponRotation>();

            if (myRotation != null) myRotation.FlipDirection();
            if (otherRotation != null) otherRotation.FlipDirection();
        }
    }

    private System.Collections.IEnumerator SlowMotion()
    {
        Time.timeScale = slowMotionScale;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }
}
