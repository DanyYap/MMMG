using UnityEngine;
using Unity.Cinemachine;

public class GroupFramer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float framingSize = 0.8f; // Adjust to control how much padding around the group
    [SerializeField] private Vector2 centerOffset = Vector2.zero; // Offset for the center point
    [SerializeField] private float damping = 2f; // Damping for camera movement
    [SerializeField] private float minFOV = 30f; // Minimum Field of View
    [SerializeField] private float maxFOV = 60f; // Maximum Field of View
    [SerializeField] private float zoomOutFactor = 1.5f; // Factor to adjust the zoom out size

    private void LateUpdate()
    {
        if (targetGroup == null || targetGroup.IsEmpty)
            return;

        FrameGroup();
    }

    private void FrameGroup()
    {
        Vector3 desiredPosition = CalculateDesiredPosition();
        AdjustCameraFOV(); // Adjust FOV based on distance

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.rotation = Quaternion.Slerp(transform.rotation, CalculateDesiredRotation(), Time.deltaTime * damping);
    }

    private Vector3 CalculateDesiredPosition()
    {
        Bounds groupBounds = targetGroup.BoundingBox;
        Vector3 center = groupBounds.center + (Vector3)centerOffset;

        // Calculate the distance to the furthest target
        float maxDistance = Mathf.Max(groupBounds.extents.x, groupBounds.extents.y, groupBounds.extents.z) * framingSize;

        // Adjust the position based on the calculated maxDistance
        Vector3 offset = new Vector3(0, 0, -maxDistance * zoomOutFactor); // Zoom out more if needed

        return center + offset;
    }

    private Quaternion CalculateDesiredRotation()
    {
        // Simple look at the center of the group
        Vector3 center = targetGroup.BoundingBox.center;
        return Quaternion.LookRotation(center - transform.position);
    }

    private void AdjustCameraFOV()
    {
        Bounds groupBounds = targetGroup.BoundingBox;
        float distance = Vector3.Distance(groupBounds.min, groupBounds.max);

        // Calculate desired FOV based on the distance
        float desiredFOV = Mathf.Clamp(distance / 10f, minFOV, maxFOV); // Adjust scaling factors as needed
        virtualCamera.Lens.FieldOfView = desiredFOV;
    }
}