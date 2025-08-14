using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleMovement : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(5f, 3f);
    public float minSpeed = 7f;
    public float maxSpeed = 15f;
    public float speedAdjustRate = 5f;

    public AudioClip hitSoundClip;

    private Rigidbody2D rb;

    // Пул аудиоисточников
    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private int poolSize = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Отключаем гравитацию и обнуляем скорость для задержки движения
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.SetOwner(this.gameObject);
        }

        InitializeAudioSourcePool();

        // Запускаем задержку старта движения на 2 секунды
        StartCoroutine(StartMovementAfterDelay(2f));
    }

    IEnumerator StartMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Включаем гравитацию обратно (если у тебя другой gravityScale, замени 1f)
        rb.gravityScale = 1f;

        // Задаём начальную скорость
        rb.linearVelocity = initialVelocity;
    }

    void InitializeAudioSourcePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject audioGO = new GameObject("AudioSourcePool_" + i);
            audioGO.transform.parent = transform;
            audioGO.transform.localPosition = Vector3.zero;

            AudioSource aSource = audioGO.AddComponent<AudioSource>();
            aSource.clip = hitSoundClip;
            aSource.playOnAwake = false;
            aSource.spatialBlend = 1f; // 3D звук
            aSource.volume = 1f;

            audioSourcePool.Add(aSource);
        }
    }

    void FixedUpdate()
    {
        float speed = rb.linearVelocity.magnitude;

        if (speed < minSpeed && speed > 0.01f)
        {
            Vector2 targetVelocity = rb.linearVelocity.normalized * minSpeed;
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, speedAdjustRate * Time.fixedDeltaTime);
        }
        else if (speed > maxSpeed)
        {
            Vector2 targetVelocity = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, speedAdjustRate * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Circle"))
        {
            Vector2 normal = collision.contacts[0].normal;
            float speedAlongNormal = Vector2.Dot(rb.linearVelocity, normal);

            if (Mathf.Abs(speedAlongNormal) < 0.1f && collision.gameObject.CompareTag("Wall"))
            {
                rb.linearVelocity += normal * minSpeed * 0.5f;
            }

            PlayHitSound();
        }
    }

    void PlayHitSound()
    {
        if (hitSoundClip == null) return;

        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                source.transform.position = transform.position;
                source.Play();
                return;
            }
        }

        Debug.LogWarning("Все аудиоисточники заняты, звук удара пропущен");
    }

    public void StartHitPause(float duration)
    {
        StartCoroutine(HitPauseRoutine(duration));
    }

    private IEnumerator HitPauseRoutine(float duration)
    {
        Vector2 savedVelocity = rb.linearVelocity;
        RigidbodyConstraints2D originalConstraints = rb.constraints;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        WeaponRotation weaponRotation = GetComponentInChildren<WeaponRotation>();
        if (weaponRotation != null)
        {
            weaponRotation.PauseRotation(duration);
        }

        yield return new WaitForSeconds(duration);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = savedVelocity.magnitude < 0.1f
            ? Random.insideUnitCircle.normalized * minSpeed
            : savedVelocity;
    }
}