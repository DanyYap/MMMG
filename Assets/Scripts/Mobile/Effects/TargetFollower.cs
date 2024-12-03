using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    // The object this will follow
    public Transform targetObject;

    // Whether this should follow the target
    public bool toFollow = true;

    // How fast this moves toward the target
    public float followSpeed = 10f;

    // How high this stays above the target
    public float offsetHeight = 1f;

    // Used to check the target's size and position
    private Collider targetCollider;

    void Start()
    {
        // Make sure a target is assigned
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned.");
            return;
        }

        // Try to get the target's Collider (used for height adjustment)
        targetCollider = targetObject.GetComponent<Collider>();

        // Warn if the target has no Collider
        if (targetCollider == null)
        {
            Debug.LogError("Target object does not have a Collider.");
        }
    }

    void Update()
    {
        // Follow the target if allowed and a target exists
        if (targetObject != null && toFollow)
        {
            // Start at the target's position
            Vector3 targetPosition = targetObject.position;

            // Adjust height if the target has a Collider
            if (targetCollider != null)
            {
                targetPosition.y = targetCollider.bounds.max.y + offsetHeight;
            }

            // Move smoothly toward the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
