
// Class to hold camera zoom settings for easy customization
[System.Serializable]
public class CameraZoomSettings
{
    public float nearViewWidth = 8f;           // Width for near view
    public float middleViewWidth = 16f;        // Width for middle view
    public float farViewWidth = 25f;           // Width for far view

    public float farDistanceThreshold = 15f;    // Distance threshold for far view
    public float middleDistanceThreshold = 5f;  // Distance threshold for middle view
}

public interface ICameraZoomStrategy
{
    float GetTargetWidth(float distance);
}

// Strategy for determining camera zoom based on distance
public class ConfigurableCameraZoomStrategy : ICameraZoomStrategy
{
    private CameraZoomSettings settings;

    // Constructor to initialize with zoom settings
    public ConfigurableCameraZoomStrategy(CameraZoomSettings zoomSettings)
    {
        settings = zoomSettings;
    }

    // Get target width based on the distance from targets
    public float GetTargetWidth(float distance)
    {
        if (distance > settings.farDistanceThreshold)
            return settings.farViewWidth;          // Return far view width
        else if (distance > settings.middleDistanceThreshold)
            return settings.middleViewWidth;       // Return middle view width
        else
            return settings.nearViewWidth;         // Return near view width
    }
}