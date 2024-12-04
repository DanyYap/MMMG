using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderTracker : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider; // The box that detects other objects.
    [SerializeField] private LayerMask detectionLayer; // Which objects should be detected.
    [SerializeField] private float updateInterval = 0.2f; // How often to check for new objects.

    private Collider[] colliderBuffer = new Collider[10]; // Store the detected objects temporarily.
    private HashSet<Collider> trackedColliders = new HashSet<Collider>(); // List of objects currently inside the box.
    private Collider nearestCollider; // The closest object inside the box.
    private float nearestDistanceSquared = float.MaxValue; // The distance to the closest object.
    private bool trackerEnabled = true; // Is the tracker turned on?
    private float timeSinceLastUpdate; // Timer to control how often updates happen.

    private void Awake()
    {
        // Make sure the box collider is set up correctly.
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        if (boxCollider != null)
        {
            boxCollider.isTrigger = true; // Make the box a trigger, so it doesn't collide but detects objects.
        }
        else
        {
            Debug.LogError("BoxCollider is missing or not assigned to this object.");
        }
    }

    private void Update()
    {
        if (!trackerEnabled) return; // Don't do anything if the tracker is off.

        // Check for updates only after a certain time interval.
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            timeSinceLastUpdate = 0; // Reset the timer.
            UpdateTrackedColliders(); // Check for new objects inside the box.
        }
    }

    private void UpdateTrackedColliders()
    {
        // Get the center and size of the box.
        Vector3 center = boxCollider.bounds.center;
        Vector3 halfExtents = boxCollider.bounds.extents;

        // Find all objects inside the box.
        int count = Physics.OverlapBoxNonAlloc(center, halfExtents, colliderBuffer, transform.rotation, detectionLayer);

        // Add all detected objects to the tracker.
        trackedColliders.Clear();
        for (int i = 0; i < count; i++)
        {
            if (colliderBuffer[i] != boxCollider) // Don't track the box itself.
            {
                trackedColliders.Add(colliderBuffer[i]);
            }
        }

        UpdateNearestCollider(); // Find the closest object.
    }

    private void UpdateNearestCollider()
    {
        Collider previousNearest = nearestCollider; // Save the previous nearest object.
        nearestCollider = null; // Start with no nearest object.
        nearestDistanceSquared = float.MaxValue; // Set the distance to the farthest possible value.

        // Loop through all tracked objects to find the closest one.
        foreach (var collider in trackedColliders)
        {
            if (collider == null) continue;

            // Calculate the distance to this object.
            float distanceSquared = (collider.transform.position - transform.position).sqrMagnitude;
            if (distanceSquared < nearestDistanceSquared)
            {
                nearestDistanceSquared = distanceSquared; // Update the nearest distance.
                nearestCollider = collider; // Update the nearest object.
            }
        }

        /*
        // Only log if the nearest object has changed.
        if (nearestCollider != previousNearest)
        {
            if (nearestCollider != null)
            {
                Debug.Log($"Nearest collider updated: {nearestCollider.name}, Distance: {Mathf.Sqrt(nearestDistanceSquared)} units");
            }
            else
            {
                Debug.Log("No colliders in range.");
            }
        }
        */
    }

    public void SetTrackerEnabled(bool enabled)
    {
        trackerEnabled = enabled;

        if (!enabled)
        {
            ResetTracker(); // Clear all data when tracker is off.
            Debug.Log("Tracker disabled and data reset.");
        }
        else
        {
            Debug.Log("Tracker enabled.");
        }
    }

    private void ResetTracker()
    {
        trackedColliders.Clear(); // Clear the list of tracked objects.
        nearestCollider = null; // Clear the nearest object.
        nearestDistanceSquared = float.MaxValue; // Reset the distance.
    }

    private void OnDrawGizmos()
    {
        // Draw the box in the scene for debugging.
        if (boxCollider != null)
        {
            Gizmos.color = trackerEnabled ? Color.green : Color.red; // Green if enabled, red if disabled.
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size); // Draw the box shape.
        }
    }

    public Collider NearestCollider => nearestCollider; // Get the nearest object.
}
