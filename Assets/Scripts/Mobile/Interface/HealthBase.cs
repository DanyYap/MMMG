using System;
using UnityEngine;

public interface IHealth
{
    float CurrentHealth { get; set; } // Tracks the current health.
    float MaxHealth { get; } // Maximum health value.
    void ReceiveDamage(float damageAmount); // Reduces health when damaged.
    void Heal(float healAmount); // Restores health.
}


public abstract class HealthBase : MonoBehaviour, IHealth
{
    [SerializeField] protected float maxHealth = 100f; // Maximum health.
    public float MaxHealth => maxHealth; // Implements the MaxHealth property.
    public virtual float CurrentHealth { get; set; } // Implements the CurrentHealth property.

    public event Action OnDeathEvent; // Event triggered when the object dies.

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

    protected virtual void OnDeath()
    {
        OnDeathEvent?.Invoke(); // Trigger the death event
        Debug.Log($"{gameObject.name} has died.");
    }

    // Public method to allow external scripts to subscribe to the death event
    public void AddOnDeathListener(Action listener)
    {
        OnDeathEvent += listener; // Add the listener to the OnDeathEvent
    }

    // Public method to allow external scripts to remove a listener
    public void RemoveOnDeathListener(Action listener)
    {
        OnDeathEvent -= listener; // Remove the listener from the OnDeathEvent
    }
}