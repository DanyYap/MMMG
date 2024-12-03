using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    private IHealthManager healthManager;
    private bool isExecuting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSystem();
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
        // Initialize the system when a new scene is loaded
        InitializeSystem();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Perform cleanup when the scene is unloaded
        CleanupSystem();
    }

    private void Update()
    {
        ExecuteSystem();
    }

    private void CreateSystem()
    {
        // Dependency injection of the status manager
        healthManager = new HealthManager();
    }

    public void InitializeSystem()
    {
        healthManager.ClearAllHealths();

        // Find and register all Health components
        var objectStatuses = FindObjectsByType<Health>(FindObjectsSortMode.InstanceID);
        foreach (var status in objectStatuses)
        {
            healthManager.RegisterNewHealth(status);
        }

        // Log the total status value for debugging
        Debug.Log($"Total Status Value: {healthManager.GetInitialTotalHealths()}");
    }

    private void ExecuteSystem()
    {
        if (!isExecuting) return;

        var healths = healthManager.GetCurrentTotalHealths();
        if (healths == 0) isExecuting = false;

        InterfaceManageSystem.Instance.GetTextManager().UpdateText(TextType.TimerText, TextNames.TimerText, healths);
    }

    public void EnableExecution(bool enabled)
    {
        isExecuting = enabled;
    }

    private void CleanupSystem()
    {
        // Perform any necessary cleanup when the scene is unloaded
        healthManager.ClearAllHealths();
        Debug.Log("System cleaned up on scene unload.");
    }

    private void OnDestroy()
    {
        // Unregister from the scene events when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
