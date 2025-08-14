using UnityEngine;

public class BloodFade : MonoBehaviour
{
    public float lifeTime = 8f;       // Время до начала исчезновения
    public float fadeDuration = 2f;   // Время плавного исчезновения

    private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("BloodFade: Нет SpriteRenderer на объекте");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isFading && timer >= lifeTime)
        {
            isFading = true;
            timer = 0f;
        }

        if (isFading && spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteRenderer.color = color;

            if (timer >= fadeDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}