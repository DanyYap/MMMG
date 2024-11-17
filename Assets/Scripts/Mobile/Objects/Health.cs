using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);
}

public interface IHealable
{
    void Heal(float amount);
}

public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageCooldown = 1f; // Cooldown in seconds

    private readonly float MIN_HEALTH = 0f;

    private float currentHealth;
    private float lastDamageTime;
    private HealthModifier healthModifier;

    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        var fireableBase = GetComponent<FireableBase>();

        currentHealth = maxHealth;
        healthModifier = new HealthModifier(fireableBase);
    }

    private void Update()
    {
        // Check if enough time has passed since the last damage application
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            float damage = healthModifier.GetCurrentDamage();
            
            if (damage > 0)
            {
                TakeDamage(damage);
                lastDamageTime = Time.time; // Reset cooldown timer
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, MIN_HEALTH, maxHealth);
        Debug.Log($"Took damage: {amount}. Current health: {currentHealth}");
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, MIN_HEALTH, maxHealth);
        Debug.Log($"Healed: {amount}. Current health: {currentHealth}");
    }

    public float GetMaxHealth() => maxHealth;
}

public interface IHealthModifier
{
    float GetCurrentDamage();
}

public class HealthModifier : IHealthModifier
{
    private FireableBase fireableBase;

    public HealthModifier(FireableBase fireableBase)
    {
        this.fireableBase = fireableBase;
    }

    public float GetCurrentDamage()
    {
        return fireableBase.GetFlammableValue();
    }
}