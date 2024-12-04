using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    private HealthManager healthManager = new HealthManager();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Register to scene loaded event
            SceneManager.sceneUnloaded += OnSceneUnloaded; // Register to scene unloaded event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        healthManager.InitializeWithSceneHealthData(); // Initialize HealthManager with all current player health data.
        Debug.Log($"Scene {scene.name} loaded. HealthManager initialized.");
    }

    private void OnSceneUnloaded(Scene scene)
    {
        healthManager.Clear(); // Clear HealthManager when a scene is unloaded.
        Debug.Log($"Scene {scene.name} unloaded. HealthManager cleared.");

        InterfaceManageSystem.Instance.GetTextManager().UpdateText(TextType.TimerText, TextNames.TimerText, healthManager.GetTotalHealth());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event.
        SceneManager.sceneUnloaded += OnSceneUnloaded; // Subscribe to scene unloaded event.
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from scene loaded event.
        SceneManager.sceneUnloaded -= OnSceneUnloaded; // Unsubscribe from scene unloaded event.
    }
}
