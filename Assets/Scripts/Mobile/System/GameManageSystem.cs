using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    private HealthSceneManager healthSceneManager = new HealthSceneManager(); // Use HealthSceneManager to manage health in the scene
    private HealthTextManager healthTextManager;

    private bool hasSceneTransitioned = false; // Flag to ensure LoadNextScene is called only once

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (healthTextManager == null)
        {
            return;
        }

        // Skip processing if in MenuScene
        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene)
        {
            return;
        }

        // Check if total fire healths have dropped to 0
        var fireHealths = healthSceneManager.GetTotalHealthData("object");
        //Debug.Log(fireHealths);
        if (fireHealths == 0)
        {
            Debug.Log("load next scene");
        }
        if (fireHealths == 0 && !hasSceneTransitioned)
        {
            hasSceneTransitioned = true; // Set the flag to prevent repeated transitions
            SceneManageSystem.Instance.GetSceneManager().LoadNextScene();
        }
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the flag to allow scene transitions in the new scene
        hasSceneTransitioned = false;

        // Initialize health data for player and other entities in the new scene
        healthSceneManager.InitializeHealthDataForScene("player"); // Initialize health data for players
        healthSceneManager.InitializeHealthDataForScene("object"); // Example: Initialize health data for enemies

        Debug.Log($"Scene {scene.name} loaded. PlayerHealthSceneManager initialized.");

        // TEMP - HealthTextManager
        healthTextManager = new HealthTextManager(healthSceneManager, "object");
    }

    // Called when a scene is unloaded
    private void OnSceneUnloaded(Scene scene)
    {
        // Clear health data for player and other entities when the scene is unloaded
        healthSceneManager.OnSceneChanged("player"); // Unregister player health data for the previous scene
        healthSceneManager.OnSceneChanged("object"); // Example: Unregister enemy health data

        Debug.Log($"Scene {scene.name} unloaded. PlayerHealthSceneManager cleared.");

        // TEMP - HealthTextManager
        healthTextManager = null;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
        SceneManager.sceneUnloaded += OnSceneUnloaded; // Subscribe to scene unloaded event
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from scene loaded event
        SceneManager.sceneUnloaded -= OnSceneUnloaded; // Unsubscribe from scene unloaded event
    }

    public HealthSceneManager GetHealthSceneManager => healthSceneManager;
}
