using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    public float Damage = 1.0f;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageCooldown = 1f; // Cooldown in seconds

    private readonly float MIN_HEALTH = 0f;

    private float currentHealth;
    private float lastDamageTime;
    private HealthEffector healthEffector;

    private event Action destroyEvent;
    private bool eventTriggered = false;

    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        var fire = GetComponent<Fire>();

        currentHealth = maxHealth;
        healthEffector = new HealthEffector(fire);

        destroyEvent += () => gameObject.GetComponent<ObjectFireable>().Extinguish();
    }

    private void Update()
    {
        if (currentHealth == 0 && !eventTriggered)
        {
            // TODO: invoke an event to hide object, or make it black with animation
            //gameObject.SetActive(false);
            destroyEvent.Invoke();
            eventTriggered = true;
            return;
        }

        // Check if enough time has passed since the last damage application
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            float damage = healthEffector.GetCurrentDamage();
            
            if (damage > 0)
            {
                ReceiveDamage(damage);
                lastDamageTime = Time.time; // Reset cooldown timer
            }
        }
    }

    public void ReceiveDamage(float damageAmount)
    {
        // If the health is already at 0, no further damage should be applied
        if (currentHealth == 0) return;

        // Calculate the new health after taking damage
        float newHealth = currentHealth - damageAmount;

        // Ensure that health stays within the bounds of MIN_HEALTH and maxHealth
        currentHealth = Mathf.Clamp(newHealth, MIN_HEALTH, maxHealth);
    }

    public float GetMaxHealth() => maxHealth;

    public void DealDamage(Health enemyHealth, float damageAmount)
    {
        enemyHealth.currentHealth -= damageAmount;
    }
}

public interface IHealthEffector
{
    float GetCurrentDamage();
}

public class HealthEffector : IHealthEffector
{
    private Fire fire;

    public HealthEffector(Fire fire)
    {
        this.fire = fire;
    }

    public float GetCurrentDamage()
    {
        if (fire == null) return 0f;
        return fire.HitDamage();
    }
}