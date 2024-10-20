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

    // Constructor to initialize with orbitalFollow and rotation settings
    public CameraRotate(CameraController camera)
    {
        this.camera = camera;
        orbitalFollow = camera.GetComponent<CinemachineOrbitalFollow>();
        rotateSettings = camera.rotateSettings;
    }

    public void RotateCamera(RotationAxis axis)
    {
        // Determine the rotation value based on the direction
        float rotationValue = rotateSettings.clockwise ? rotateSettings.rotationAmount : -rotateSettings.rotationAmount;

        // Check the chosen rotation axis
        if (axis == RotationAxis.Horizontal)
        {
            TargetHorizontalRotation = orbitalFollow.HorizontalAxis.Value + rotationValue;

            // Normalize the target rotation to keep it within 0-360 degrees
            if (TargetHorizontalRotation >= 360)
            {
                TargetHorizontalRotation -= 360;
            }
            else if (TargetHorizontalRotation < 0)
            {
                TargetHorizontalRotation += 360;
            }
        }
        else if (axis == RotationAxis.Vertical)
        {
            TargetVerticalRotation = orbitalFollow.VerticalAxis.Value + rotationValue;

            // Normalize the vertical axis value
            if (TargetVerticalRotation > 1f) // Assuming 1 is the max for vertical axis
            {
                TargetVerticalRotation = 1f;
            }
            else if (TargetVerticalRotation < -1f) // Assuming -1 is the min for vertical axis
            {
                TargetVerticalRotation = -1f;
            }
        }
    }

    // Method to update the axis value based on the event
    public void UpdateAxisValues()
    {
        orbitalFollow.HorizontalAxis.Value = Mathf.Lerp(orbitalFollow.HorizontalAxis.Value, TargetHorizontalRotation, Time.deltaTime * rotateSettings.rotationSpeed);
        orbitalFollow.VerticalAxis.Value = Mathf.Lerp(orbitalFollow.VerticalAxis.Value, TargetVerticalRotation, Time.deltaTime * rotateSettings.rotationSpeed);
    }
}