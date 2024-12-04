using System.Collections.Generic;

public class HealthDataManager
{
    protected readonly Dictionary<string, Dictionary<string, float>> healthDataByType = new Dictionary<string, Dictionary<string, float>>(); // Stores health data for different entity types (player, object, enemy, etc.)

    // Registers health data for a specific entity under a health type (e.g., player, object)
    public void RegisterHealthData(string healthType, string entityName, float healthValue)
    {
        if (!healthDataByType.ContainsKey(healthType))
        {
            healthDataByType[healthType] = new Dictionary<string, float>(); // Create new dictionary for health type if it doesn't exist.
        }

        if (!healthDataByType[healthType].ContainsKey(entityName))
        {
            healthDataByType[healthType][entityName] = healthValue; // Register entity health data.
        }
    }

    // Unregister health data for a specific entity under a health type.
    public void UnregisterHealthData(string healthType, string entityName)
    {
        if (healthDataByType.ContainsKey(healthType) && healthDataByType[healthType].ContainsKey(entityName))
        {
            healthDataByType[healthType].Remove(entityName); // Remove entity health data.
        }
    }

    // Get the current health value for a specific entity by name and health type (e.g., player, object)
    public float GetHealthData(string healthType, string entityName)
    {
        if (healthDataByType.ContainsKey(healthType) && healthDataByType[healthType].ContainsKey(entityName))
        {
            return healthDataByType[healthType][entityName]; // Return health for the specific entity.
        }
        return 0f; // Return 0 if the entity is not found.
    }

    // UpdateFire the health value for a specific entity under a health type.
    public void UpdateHealthData(string healthType, string entityName, float newHealthValue)
    {
        if (healthDataByType.ContainsKey(healthType) && healthDataByType[healthType].ContainsKey(entityName))
        {
            healthDataByType[healthType][entityName] = newHealthValue; // UpdateFire health for the entity.
        }
    }

    // Get all health values for a specific health type (e.g., all players or all objects)
    public Dictionary<string, float> GetAllHealthData(string healthType)
    {
        if (healthDataByType.ContainsKey(healthType))
        {
            return new Dictionary<string, float>(healthDataByType[healthType]); // Return a copy of health data for the requested health type.
        }
        return new Dictionary<string, float>(); // Return an empty dictionary if the health type is not found.
    }

    // Get the total health for a specific health type (e.g., total health of all players)
    public float GetTotalHealthData(string healthType)
    {
        float totalHealth = 0f;
        if (healthDataByType.ContainsKey(healthType))
        {
            foreach (var health in healthDataByType[healthType].Values)
            {
                totalHealth += health; // Sum up health values.
            }
        }
        return totalHealth;
    }

    // Clear all health data for all types and entities.
    public void ClearAllHealthData()
    {
        healthDataByType.Clear(); // Clear all health data.
    }
}
