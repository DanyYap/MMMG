using System.Collections.Generic;
using UnityEngine;

// Abstraction for managing a collection of health status components
public interface IHealthManager
{
    float GetInitialTotalHealths();
    float GetCurrentTotalHealths();
    void RegisterNewHealth(Health status);
    void UnregisterExistedHealth(Health status);
    void ClearAllHealths();
}

// Manages health status components efficiently
public class HealthManager : IHealthManager
{
    private readonly HashSet<Health> objectHealths = new(); // Use HashSet for efficient lookups
    private float initialTotalHealths;

    public float GetInitialTotalHealths()
    {
        float totalMaxHealth = 0f;

        // Loop through each health object to accumulate maximum health
        foreach (var health in objectHealths)
        {
            totalMaxHealth += health.GetMaxHealth();
        }

        initialTotalHealths = totalMaxHealth;

        return totalMaxHealth;
    }

    public float GetCurrentTotalHealths()
    {
        float totalHealth = 0f;

        // Loop through each health object to accumulate current health
        foreach (var health in objectHealths)
        {
            totalHealth += health.CurrentHealth;
        }

        // Calculate percentage based on initial total health
        float initialTotal = GetInitialTotalHealths();

        // Return 0% if there are no health objects or initial health is 0
        if (initialTotal <= 0f) return 0f;

        // Calculate current health percentage
        return Mathf.Clamp((totalHealth / initialTotal) * 100f, 0f, 100f);
    }

    public void RegisterNewHealth(Health status)
    {
        objectHealths.Add(status); // Add returns false if already exists, no need for check
    }

    public void UnregisterExistedHealth(Health status)
    {
        objectHealths.Remove(status); // Remove does nothing if the item is not present
    }

    public void ClearAllHealths()
    {
        objectHealths.Clear(); 
    }
}