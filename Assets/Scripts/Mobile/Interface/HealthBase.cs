using UnityEngine;

public interface IHealth
{
    float CurrentHealth { get; set; } // Tracks the current health.
    void ReceiveDamage(float damageAmount); // Reduces health when damaged.
    void Heal(float healAmount); // Restores health.
}

public abstract class HealthBase : MonoBehaviour, IHealth
{
    [SerializeField] protected float maxHealth = 100f; // Maximum health.
    public virtual float CurrentHealth { get; set; } // Implements the CurrentHealth property.

    protected virtual void Awake()
    {
        CurrentHealth = maxHealth; // Initialize health to max value by default.
    }

    public virtual void ReceiveDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0); // Prevent health from dropping below zero.
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current Health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            OnDeath(); // Call death logic when health reaches zero.
        }
    }

    public virtual void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Min(CurrentHealth, maxHealth); // Prevent health from exceeding max value.
        Debug.Log($"{gameObject.name} healed {healAmount}. Current Health: {CurrentHealth}");
    }

    protected abstract void OnDeath(); // Abstract method for custom death logic.
}