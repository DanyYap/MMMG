using System.Collections.Generic;

// Abstraction for managing a collection of status components
public interface IStatusManager
{
    float CalculateTotalStatus();
    void RegisterStatus(IStatus status);
    void UnregisterStatus(IStatus status);
}

// Manages status components, following SRP and DIP
public class StatusManager : IStatusManager
{
    private readonly List<IStatus> statusComponents = new();

    public float CalculateTotalStatus()
    {
        float total = 0f;
        foreach (var status in statusComponents)
        {
            total += status.CurrentStatusValue;
        }
        return total;
    }

    public void RegisterStatus(IStatus status)
    {
        if (!statusComponents.Contains(status))
        {
            statusComponents.Add(status);
        }
    }

    public void UnregisterStatus(IStatus status)
    {
        statusComponents.Remove(status);
    }
}
