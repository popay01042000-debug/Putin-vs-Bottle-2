using UnityEngine;

public class Mine : MonoBehaviour
{
    [Tooltip("����, ������� ������� ����")]
    public int damage = 3; // ������ ������������ � ����������

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ��� ������ � Circle1
        if (other.gameObject.name == "Circle1")
        {
            DamageReceiver receiver = other.GetComponent<DamageReceiver>();
            if (receiver != null)
            {
                // �������� true, ����� ������������ �������� ������������
                receiver.TakeDamage(damage, gameObject, true);
            }

            // ���������� ���� ����� ���������
            Destroy(gameObject);
        }
    }
}