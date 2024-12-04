using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSceneManager : HealthDataManager
{
    private readonly Dictionary<string, List<string>> lastSceneEntities = new Dictionary<string, List<string>>(); // Keeps track of entities' names per health type (e.g., "player", "object", etc.)

    // Initialize health data for a specific health type (e.g., "player", "enemy") in the current scene.
    public void InitializeHealthDataForScene(string healthType)
    {
        // Clear health data for the specified health type
        ClearHealthDataByType(healthType);

        // Get all health components of the specified type in the current scene
        var healthComponents = FindHealthComponentsByType(healthType);

        // Register health data for each found component
        foreach (var component in healthComponents)
        {
            RegisterHealthData(healthType, component.gameObject.name, component.CurrentHealth);
        }

        // Store entity names for the current scene for future scene switching
        lastSceneEntities[healthType] = new List<string>();
        foreach (var component in healthComponents)
        {
            lastSceneEntities[healthType].Add(component.gameObject.name);
        }
    }

    // Handle scene changes and manage health data for a specific health type
    public void OnSceneChanged(string healthType)
    {
        // Get all health components of the specified type in the new scene
        var newSceneHealthComponents = FindHealthComponentsByType(healthType);

        // Unregister entities from the previous scene (if they are no longer in the new scene)
        if (lastSceneEntities.ContainsKey(healthType))
        {
            foreach (var entityName in lastSceneEntities[healthType])
            {
                UnregisterHealthData(healthType, entityName); // Unregister entity from the previous scene
            }
        }

        // Register new entities in the current scene
        foreach (var component in newSceneHealthComponents)
        {
            RegisterHealthData(healthType, component.gameObject.name, component.CurrentHealth); // Register new entities' health data
        }

        // Update the list of entities for the health type in the last scene
        lastSceneEntities[healthType] = new List<string>();
        foreach (var component in newSceneHealthComponents)
        {
            lastSceneEntities[healthType].Add(component.gameObject.name); // Store new scene entities
        }
    }

    // Subscribe to scene load events to automatically update health data when a scene is loaded
    public void SubscribeToSceneEvents(string healthType)
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            OnSceneChanged(healthType); // Automatically handle scene changes for the specified health type
        };
    }

    // Find all health components of a given health type (e.g., "player", "enemy", "object")
    private IEnumerable<HealthBase> FindHealthComponentsByType(string healthType)
    {
        List<HealthBase> healthComponents = new List<HealthBase>();

        switch (healthType)
        {
            case "player":
                // Debug log to check how many player health components are found
                var playerHealths = GameObject.FindObjectsByType<PlayerHealth>(FindObjectsSortMode.InstanceID);
                healthComponents.AddRange(playerHealths);
                Debug.Log($"Found {playerHealths.Length} PlayerHealth components.");
                break;

            case "object":
                // Debug log to check how many object health components are found
                var objectHealths = GameObject.FindObjectsByType<ObjectHealth>(FindObjectsSortMode.InstanceID);
                healthComponents.AddRange(objectHealths);
                Debug.Log($"Found {objectHealths.Length} ObjectHealth components.");
                break;

            default:
                Debug.LogWarning($"Health type '{healthType}' not recognized. Returning an empty list.");
                break;
        }

        // If no components are found, log it as well
        if (healthComponents.Count == 0)
        {
            Debug.LogWarning($"No health components found for health type: {healthType}");
        }

        return healthComponents;
    }

    // Clear health data for a specific health type
    private void ClearHealthDataByType(string healthType)
    {
        if (healthDataByType.ContainsKey(healthType))
        {
            healthDataByType[healthType].Clear(); // Clear only health data for the specified health type
        }
    }
}
