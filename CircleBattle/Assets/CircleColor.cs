using UnityEngine;

public class CircleColor : MonoBehaviour
{
    [HideInInspector] public Color baseColor;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) baseColor = sr.color;
    }

    public void FlashWhite(float duration)
    {
        if (sr != null) StartCoroutine(FlashRoutine(duration));
    }

    private System.Collections.IEnumerator FlashRoutine(float duration)
    {
        sr.color = Color.white;
        yield return new WaitForSeconds(duration);
        sr.color = baseColor;
    }
}
