using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Invincibility Frames")]
    public bool hasIFrames = false;
    public float iFrameDuration = 0.8f;

    [Header("Events")]
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;
    public UnityEvent onHit;

    public bool IsAlive => currentHealth > 0f;
    public float HealthPercent => currentHealth / maxHealth;

    private bool _isInvincible = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive || _isInvincible) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log($"[Health] {gameObject.name} took {amount} damage. " +
                  $"HP: {currentHealth}/{maxHealth}");

        onHit?.Invoke();
        onHealthChanged?.Invoke(HealthPercent);

        if (currentHealth <= 0f)
            Die();
        else if (hasIFrames)
            StartCoroutine(IFrames());
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(HealthPercent);
        Debug.Log($"[Health] {gameObject.name} healed {amount}. " +
                  $"HP: {currentHealth}/{maxHealth}");
    }

    public void SetInvincible(bool state)
    {
        _isInvincible = state;
    }

    void Die()
    {
        Debug.Log($"[Health] {gameObject.name} died!");
        onDeath?.Invoke();
    }

    System.Collections.IEnumerator IFrames()
    {
        _isInvincible = true;
        Debug.Log("[Health] Invincibility frames active!");
        yield return new WaitForSeconds(iFrameDuration);
        _isInvincible = false;
        Debug.Log("[Health] Invincibility frames ended.");
    }
}