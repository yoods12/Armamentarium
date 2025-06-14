using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("최대 체력")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 데미지를 받아 체력 감소
    /// </summary>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        // 사망 처리: 이펙트, 애니메이션 등 추가 가능
        Debug.Log($"{name} died.");
        Destroy(gameObject);
    }
}
