using Unity.Cinemachine;
using UnityEngine;

// Enum to define rotation axis
public enum RotationAxis
{
    Horizontal,
    Vertical
}

// Class to hold customizable rotation settings
[System.Serializable]
public class RotateSettings
{
    public float rotationSpeed = 5f; // Speed of rotation smoothing
    public float rotationAmount = 90f; // Default rotation amount (degrees)
    public bool clockwise = true; // True for clockwise, false for counterclockwise
}

// Interface for camera rotation strategy
public interface ICameraRotateStrategy
{
    void RotateCamera(RotationAxis axis);
}

// Class to handle camera rotation
public class CameraRotate : ICameraRotateStrategy
{
    private CameraController camera;
    private CinemachineOrbitalFollow orbitalFollow;
    private RotateSettings rotateSettings; // Customizable rotation settings

    // Target rotation values
    public float TargetHorizontalRotation { get; private set; }
    public float TargetVerticalRotation { get; private set; }

    public CameraRotate(CameraController camera)
    {
        this.camera = camera;
        orbitalFollow = camera.GetComponent<CinemachineOrbitalFollow>();
        rotateSettings = camera.rotateSettings;
    }

    public void RotateCamera(RotationAxis axis)
    {
        float rotationValue = rotateSettings.clockwise ? rotateSettings.rotationAmount : -rotateSettings.rotationAmount;

        if (axis == RotationAxis.Horizontal)
        {
            TargetHorizontalRotation += rotationValue;

            // Normalize the target rotation to keep it within 0-360 degrees
            TargetHorizontalRotation = NormalizeAngle(TargetHorizontalRotation);
        }
        else if (axis == RotationAxis.Vertical)
        {
            TargetVerticalRotation += rotationValue;

            // Normalize the vertical axis value
            TargetVerticalRotation = Mathf.Clamp(TargetVerticalRotation, -1f, 1f); // Assuming -1 to 1 is the range
        }
    }

    // Normalize angle to keep it within 0-360 degrees
    private float NormalizeAngle(float angle)
    {
        while (angle >= 360) angle -= 360;
        while (angle < 0) angle += 360;
        return angle;
    }

    public void UpdateAxisValues()
    {
        // Smoothly rotate towards the target horizontal rotation
        orbitalFollow.HorizontalAxis.Value = Mathf.LerpAngle(orbitalFollow.HorizontalAxis.Value, TargetHorizontalRotation, Time.deltaTime * rotateSettings.rotationSpeed);
        orbitalFollow.VerticalAxis.Value = Mathf.Lerp(orbitalFollow.VerticalAxis.Value, TargetVerticalRotation, Time.deltaTime * rotateSettings.rotationSpeed);
    }
}