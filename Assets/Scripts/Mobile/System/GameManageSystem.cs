using UnityEngine;

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
            InitializeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
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
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(TextType.TimerText, TextNames.TimerText, healths);
    }

    public void EnableExecution(bool enabled)
    {
        isExecuting = enabled;
    }
}
