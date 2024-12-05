using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthTextManager
{
    private Text fireHealthText; // Reference to the UI Text component
    private HealthDataManager healthDataManager; // Reference to the HealthDataManager
    private string healthType; // The health type to track (e.g., "object")

    public HealthTextManager(HealthDataManager healthDataManager, string healthType)
    {
        this.healthDataManager = healthDataManager;
        this.healthType = healthType;

        // Subscribe to scene events
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        Debug.Log($"HealthTextManager initialized for health type: {healthType}");
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}. Searching for 'Fire Healths Text' GameObject...");
        GameObject fireHealthTextObject = GameObject.Find("Fire Healths Text");
        Debug.Log("yoooooo " + fireHealthTextObject);
        if (fireHealthTextObject != null)
        {
            fireHealthText = fireHealthTextObject.GetComponent<Text>();
            if (fireHealthText == null)
            {
                Debug.LogWarning("Fire Healths Text GameObject does not have a Text component!");
            }
            else
            {
                Debug.Log("Successfully found and assigned Fire Healths Text component.");
            }
        }
        else
        {
            Debug.LogWarning("GameObject named 'Fire Healths Text' not found in the scene!");
        }
    }

    // Called when the active scene is unloaded
    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"Scene unloaded: {scene.name}. Clearing reference to Fire Healths Text.");
        fireHealthText = null;
    }

    // Update the health text with the current total health from HealthDataManager
    public void UpdateText()
    {
        if (fireHealthText == null)
        {
            //Debug.LogWarning("Cannot update text because Fire Healths Text reference is null.");
            return;
        }

        if (healthDataManager == null)
        {
            Debug.LogError("HealthDataManager is null! Cannot retrieve health data.");
            return;
        }

        float totalHealth = healthDataManager.GetTotalHealthData(healthType);
        fireHealthText.text = $"Current Fire Healths Left: {totalHealth}";
        Debug.Log($"Updated Fire Healths Text: Current Fire Healths Left: {totalHealth}");
    }

    // Cleanup resources
    public void Dispose()
    {
        Debug.Log("Disposing HealthTextManager and unsubscribing from scene events.");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
