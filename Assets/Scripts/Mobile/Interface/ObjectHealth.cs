using UnityEngine;

public class ObjectHealth : HealthBase
{
    [SerializeField] private float objectMaxHealth = 100f; // Custom max health for the object.

    protected override void Awake()
    {
        base.Awake(); // Call base logic to initialize health.
        maxHealth = objectMaxHealth; // Set a new maximum health specific to the object.
    }

    public override void ReceiveDamage(float damageAmount)
    {
        base.ReceiveDamage(damageAmount); // Use base behavior to handle damage.
        Debug.Log($"Object-specific logic: Play a hit animation or sound.");
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        Debug.Log($"Object-specific logic: Play a heal animation or sound.");
    }

    protected override void OnDeath()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        // Add object-specific death logic here (e.g., destroy object or trigger animation).
        Destroy(gameObject); // Example: Destroy the object when health reaches 0.
    }
}
