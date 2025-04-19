using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 5;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log("���� ����! ���� HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("�� ���!");
        Destroy(gameObject);
    }
}