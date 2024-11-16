using System.Collections.Generic;

// Abstraction for managing a collection of health status components
public interface IHealthStatusManager
{
    float CalculateTotalHealth();
    void RegisterHealthStatus(Health status);
    void UnregisterHealthStatus(Health status);
    void ClearAllHealthStatuses();
}

// Manages health status components efficiently
public class HealthStatusManager : IHealthStatusManager
{
    private readonly HashSet<Health> healthStatusComponents = new(); // Use HashSet for efficient lookups

    public float CalculateTotalHealth()
    {
        float totalHealth = 0f;
        foreach (var healthStatus in healthStatusComponents)
        {
            totalHealth += healthStatus.CurrentHealth; 
        }
        return totalHealth; 
    }

    public void RegisterHealthStatus(Health status)
    {
        healthStatusComponents.Add(status); // Add returns false if already exists, no need for check
    }

    public void UnregisterHealthStatus(Health status)
    {
        healthStatusComponents.Remove(status); // Remove does nothing if the item is not present
    }

    public void ClearAllHealthStatuses()
    {
        healthStatusComponents.Clear(); 
    }
}