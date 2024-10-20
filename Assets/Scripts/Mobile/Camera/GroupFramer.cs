using UnityEngine;
using Unity.Cinemachine;

public class CameraTargetTracker : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    private CinemachineFollowZoom followZoom;

    private const float nearViewWidth = 8f;    // Near view width
    private const float middleViewWidth = 16f; // Middle view width
    private const float farViewWidth = 25f;    // Far view width

    [SerializeField]
    private float transitionSpeed = 2f; // Speed of the transition smoothing

    private float currentWidth; // Current zoom width

    private void Awake()
    {
        followZoom = GetComponent<CinemachineFollowZoom>();
        currentWidth = nearViewWidth; // Start at near view
        followZoom.Width = currentWidth;
    }

    private void LateUpdate()
    {
        AdjustCameraWidth();
    }

    private void AdjustCameraWidth()
    {
        if (targetGroup.Targets.Count == 0) return;

        float distance = CalculateTargetsDistance();
        float targetWidth;

        // Determine the target width based on the calculated distance
        if (distance > 15f) // Threshold for switching to far view
        {
            targetWidth = farViewWidth; // Switch to far view
        }
        else if (distance > 5f) // Threshold for switching to middle view
        {
            targetWidth = middleViewWidth; // Switch to middle view
        }
        else
        {
            targetWidth = nearViewWidth; // Stay at near view
        }

        // Smoothly transition the current width towards the target width
        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * transitionSpeed);

        // Update the followZoom width
        followZoom.Width = currentWidth;
    }

    private float CalculateTargetsDistance()
    {
        Vector3 centerPoint = Vector3.zero;
        foreach (var target in targetGroup.Targets)
        {
            centerPoint += target.Object.transform.position;
        }
        centerPoint /= targetGroup.Targets.Count;

        float maxDistance = 0f;
        foreach (var target in targetGroup.Targets)
        {
            float distance = Vector3.Distance(centerPoint, target.Object.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
        return maxDistance;
    }
}