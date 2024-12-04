using UnityEngine;

public class PlayerHealth : HealthBase
{
    [SerializeField] private float playerMaxHealth = 150f; // Custom max health for the player.

    protected override void Awake()
    {
        base.Awake(); // Call base logic to initialize health.
        maxHealth = playerMaxHealth; // Set a new maximum health specific to the player.
    }

    public override void ReceiveDamage(float damageAmount)
    {
        base.ReceiveDamage(damageAmount); // Use base behavior to handle damage.
        Debug.Log($"Player-specific logic: Play a damage animation or sound.");
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        Debug.Log($"Player-specific logic: Play a heal animation or sound.");
    }

    protected override void OnDeath()
    {
        Debug.Log($"{gameObject.name} has died! Trigger respawn or game over.");
        // Add player-specific death logic here (e.g., respawn or game over).
    }
}
