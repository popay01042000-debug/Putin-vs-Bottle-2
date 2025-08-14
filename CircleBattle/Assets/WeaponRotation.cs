using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public float rotationSpeed = 360f;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    private int direction = 1;

    void Update()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                ResumeRotation();
            }
            return;
        }

        transform.Rotate(0f, 0f, rotationSpeed * direction * Time.deltaTime);
    }

    public void PauseRotation(float duration)
    {
        isPaused = true;
        pauseTimer = duration;
    }

    public void ResumeRotation()
    {
        isPaused = false;
        pauseTimer = 0f;
    }

    public void FlipDirection()
    {
        direction *= -1;
    }

    public int GetDirection()
    {
        return direction;
    }
}