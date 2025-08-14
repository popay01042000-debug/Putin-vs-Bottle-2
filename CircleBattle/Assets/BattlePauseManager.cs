using System.Collections;
using UnityEngine;

public class BattlePauseManager : MonoBehaviour
{
    public GameObject circle1;
    public GameObject circle2;

    private CircleMovement circle1Movement;
    private CircleMovement circle2Movement;

    private WeaponRotation circle1WeaponRotation;
    private WeaponRotation circle2WeaponRotation;

    private SpriteRenderer circle2SpriteRenderer;
    private Sprite circle2OriginalSprite;
    public Sprite circle2FreezeSprite; // сюда назначай спрайт заморозки через инспектор

    void Start()
    {
        if (circle1 != null)
        {
            circle1Movement = circle1.GetComponent<CircleMovement>();
            circle1WeaponRotation = circle1.GetComponentInChildren<WeaponRotation>();
        }

        if (circle2 != null)
        {
            circle2Movement = circle2.GetComponent<CircleMovement>();
            circle2WeaponRotation = circle2.GetComponentInChildren<WeaponRotation>();
            circle2SpriteRenderer = circle2.GetComponent<SpriteRenderer>();
            if (circle2SpriteRenderer != null)
            {
                circle2OriginalSprite = circle2SpriteRenderer.sprite;
            }
        }
    }

    // Заморозить движение и вращение и сменить спрайт Circle2
    public void FreezeBattle()
    {
        if (circle1Movement != null) circle1Movement.enabled = false;
        if (circle2Movement != null) circle2Movement.enabled = false;

        if (circle1WeaponRotation != null) circle1WeaponRotation.PauseRotation(9999f); // долго
        if (circle2WeaponRotation != null) circle2WeaponRotation.PauseRotation(9999f);

        if (circle2SpriteRenderer != null && circle2FreezeSprite != null)
        {
            circle2SpriteRenderer.sprite = circle2FreezeSprite;
        }
    }

    // Разморозить движение, вращение и вернуть спрайт Circle2
    public void UnfreezeBattle()
    {
        if (circle1Movement != null) circle1Movement.enabled = true;
        if (circle2Movement != null) circle2Movement.enabled = true;

        if (circle1WeaponRotation != null) circle1WeaponRotation.ResumeRotation();
        if (circle2WeaponRotation != null) circle2WeaponRotation.ResumeRotation();

        if (circle2SpriteRenderer != null)
        {
            circle2SpriteRenderer.sprite = circle2OriginalSprite;
        }
    }
}
