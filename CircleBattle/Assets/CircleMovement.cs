using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public Vector2 initialVelocity;
    public float minSpeed = 7f;
    public float maxSpeed = 15f;
    public float speedAdjustRate = 0.5f;
    

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = initialVelocity;
    }

    void FixedUpdate()
    {
        float speed = rb.linearVelocity.magnitude;

        if (speed < minSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * (speed + speedAdjustRate * Time.fixedDeltaTime);
        else if (speed > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * (speed - speedAdjustRate * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
       
    }
}
