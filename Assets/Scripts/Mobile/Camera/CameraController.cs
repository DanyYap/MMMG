using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    public CameraZoomSettings zoomSettings;
    public RotateSettings rotateSettings;

    public CameraRotate CameraRotate { get; private set; }

    private CinemachineFollowZoom followZoom;
    private CinemachineOrbitalFollow orbitalFollow;
    private ICameraZoomStrategy zoomStrategy;
    private float currentWidth;

    [SerializeField] private float transitionSpeed = 2f; // Speed of the transition smoothing

    private void Awake()
    {
        // zoom
        followZoom = GetComponent<CinemachineFollowZoom>();
        currentWidth = zoomSettings.nearViewWidth; // Start at near view width
        followZoom.Width = currentWidth; // Set initial width
        zoomStrategy = new ConfigurableCameraZoomStrategy(zoomSettings); // Initialize zoom strategy

        // rotate
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        CameraRotate = new CameraRotate(this);
    }

    private void LateUpdate()
    {
        AdjustCameraWidth(); // Adjust camera zoom based on target distances
        CameraRotate.UpdateAxisValues(); // Interpolate camera rotation
    }

    private void AdjustCameraWidth()
    {
        if (targetGroup.Targets.Count == 0) return; // Exit if no targets

        float distance = CalculateTargetsDistance(); // Calculate distance to targets
        float targetWidth = zoomStrategy.GetTargetWidth(distance); // Get target width based on distance

        // Smoothly transition the current width towards the target width
        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * transitionSpeed);

        // Update the followZoom width
        followZoom.Width = currentWidth;
    }

    private float CalculateTargetsDistance()
    {
        Vector3 centerPoint = Vector3.zero; // Initialize center point
        foreach (var target in targetGroup.Targets)
        {
            centerPoint += target.Object.transform.position; // Sum positions of targets
        }
        centerPoint /= targetGroup.Targets.Count; // Calculate average position

        float maxDistance = 0f; // Initialize max distance
        foreach (var target in targetGroup.Targets)
        {
            float distance = Vector3.Distance(centerPoint, target.Object.transform.position); // Calculate distance
            if (distance > maxDistance)
            {
                maxDistance = distance; // Update max distance
            }
        }
        return maxDistance; // Return the maximum distance
    }
}