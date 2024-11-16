using UnityEngine;
using System.Collections.Generic;

// Interface for status management
public interface IHealthStatus
{
    void UpdateHealth(List<float> affectedValues);
}

public class Health : MonoBehaviour, IHealthStatus
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minHealth = 0f;

    private float currentHealth;
    private FireableBase fireableBase;

    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        fireableBase = GetComponent<FireableBase>(); // Ensure FireableBase is attached
    }

    private void Update()
    {
        // Collect multiple health-affecting values
        List<float> healthModifiers = new()
        {
            - fireableBase.GetFlammableValue() * 0.001f,
            GetOtherHealthModifiers() // Example method for other modifiers
        };

        UpdateHealth(healthModifiers); // Call the updated method

        Debug.Log(currentHealth);
    }

    // Update health based on multiple values
    public void UpdateHealth(List<float> affectedValues)
    {
        float totalEffect = 0f;
        foreach (var value in affectedValues)
        {
            totalEffect += value; // Sum all health-modifying values
        }

        if (totalEffect != 0 && currentHealth > minHealth) // Only update if there is an effect and health is above minimum
        {
            currentHealth = Mathf.Clamp(currentHealth + totalEffect, minHealth, maxHealth); // Clamp health
        }
    }

    private float GetOtherHealthModifiers()
    {
        // Placeholder for other health-modifying logic
        return 0f; // Replace with actual logic
    }
}