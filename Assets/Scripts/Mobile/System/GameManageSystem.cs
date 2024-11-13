using UnityEngine;

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    private IHealthStatusManager statusManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSystem();
            InitializeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateSystem()
    {
        // Dependency injection of the status manager
        statusManager = new HealthStatusManager();
    }

    public void InitializeSystem()
    {
        statusManager.ClearAllHealthStatuses();

        // Find and register all Health components
        var objectStatuses = FindObjectsByType<Health>(FindObjectsSortMode.InstanceID);
        foreach (var status in objectStatuses)
        {
            statusManager.RegisterHealthStatus(status);
        }

        // Log the total status value for debugging
        Debug.Log($"Total Status Value: {statusManager.CalculateTotalHealth()}");
    }
}
