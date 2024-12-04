using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderTracker : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;  // The BoxCollider used as a trigger
    private List<Collider> trackedColliders = new List<Collider>(); // List to keep track of colliders inside the trigger

    private void Awake()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        if (boxCollider != null)
        {
            boxCollider.isTrigger = true; // Ensure the box collider is set as a trigger
        }
        else
        {
            Debug.LogError("BoxCollider is not assigned or missing on this object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add the collider to the tracked list if it's not already in it
        if (!trackedColliders.Contains(other))
        {
            trackedColliders.Add(other);
            Debug.Log($"Collider entered: {other.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the collider from the tracked list
        if (trackedColliders.Contains(other))
        {
            trackedColliders.Remove(other);
            Debug.Log($"Collider exited: {other.name}");
        }
    }

    private void Update()
    {
        if (trackedColliders.Count > 0)
        {
            Collider nearestCollider = null;
            float minDistance = float.MaxValue;

            // Loop through the tracked colliders to find the nearest one
            foreach (var collider in trackedColliders)
            {
                if (collider != null) // Ensure the collider still exists
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestCollider = collider;
                    }
                }
            }

            // Log the nearest collider
            if (nearestCollider != null)
            {
                Debug.Log($"Nearest collider: {nearestCollider.name}, Distance: {minDistance}");
            }
        }
    }
}
