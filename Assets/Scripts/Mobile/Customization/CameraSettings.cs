using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Game Settings/Camera Settings", order = 1)]
public class CameraSettings : ScriptableObject
{
    [Header("Zoom Settings")]
    [Tooltip("Width for near view")]
    public float nearViewWidth = 8f;

    [Tooltip("Width for middle view")]
    public float middleViewWidth = 13f;

    [Tooltip("Width for far view")]
    public float farViewWidth = 22f;

    [Header("Distance Threshold Settings")]
    [Tooltip("Distance threshold for far view")]
    public float farDistanceThreshold = 15f;

    [Tooltip("Distance threshold for middle view")]
    public float middleDistanceThreshold = 5f;

    // Add other camera-related settings as needed
}
