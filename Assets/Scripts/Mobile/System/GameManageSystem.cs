using UnityEngine;

public class GameManageSystem : MonoBehaviour
{
    private IStatusManager statusManager;

    private void Awake()
    {
        // Dependency injection of the status manager
        statusManager = new StatusManager();

        // Find and register all ObjectStatus components
        var objectStatuses = FindObjectsByType<ObjectStatus>(FindObjectsSortMode.InstanceID);
        foreach (var status in objectStatuses)
        {
            statusManager.RegisterStatus(status);
        }

        // Log the total status value for debugging
        Debug.Log($"Total Status Value: {statusManager.CalculateTotalStatus()}");
    }
}
