using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    private HealthSceneManager playerHealthSceneManager = new HealthSceneManager(); // Use HealthSceneManager to manage health in the scene

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

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Initialize health data for player and other entities in the new scene
        playerHealthSceneManager.InitializeHealthDataForScene("player"); // Initialize health data for players
        playerHealthSceneManager.InitializeHealthDataForScene("object"); // Example: Initialize health data for enemies

        Debug.Log($"Scene {scene.name} loaded. PlayerHealthSceneManager initialized.");
    }

    // Called when a scene is unloaded
    private void OnSceneUnloaded(Scene scene)
    {
        // Clear health data for player and other entities when the scene is unloaded
        playerHealthSceneManager.OnSceneChanged("player"); // Unregister player health data for the previous scene
        playerHealthSceneManager.OnSceneChanged("object"); // Example: Unregister enemy health data

        Debug.Log($"Scene {scene.name} unloaded. PlayerHealthSceneManager cleared.");

        // Optionally update the interface with total health of players in the scene
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(TextType.TimerText, TextNames.TimerText, playerHealthSceneManager.GetTotalHealthData("object"));
        Debug.Log(playerHealthSceneManager.GetTotalHealthData("object"));
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
}
