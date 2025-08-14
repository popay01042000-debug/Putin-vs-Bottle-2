using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public float rotationSpeed = 360f; // градусов в секунду
    private int direction = 1;
    private bool isPaused = false;

    void Update()
    {
        if (!isPaused)
        {
            transform.Rotate(0f, 0f, rotationSpeed * direction * Time.deltaTime);
        }
    }

    public void FlipDirection()
    {
        direction *= -1;
    }

    public void PauseRotation(float duration)
    {
        if (!isPaused) StartCoroutine(PauseRoutine(duration));
    }

    private System.Collections.IEnumerator PauseRoutine(float duration)
    {
        isPaused = true;
        yield return new WaitForSeconds(duration);
        isPaused = false;
    }
}
