using UnityEngine;

public class Fire : MonoBehaviour
{
    // Reference to the object that this fire will follow
    public Transform mainObject;  // The object to follow

    public bool toFollow = true;  // Whether the fire should follow the main object

    // Speed of the following movement
    public float followSpeed = 10f; // Adjust this value to control how fast fire follows the object

    // Height offset above the main object's collider
    public float offsetHeight = 1f; // Distance above the collider where the fire should be positioned

    // Cached reference to the main object's Collider
    private Collider mainCollider;

    // Start is called once before the first execution of Update
    void Start()
    {
        // If mainObject is not set, log an error
        if (mainObject == null)
        {
            Debug.LogError("Main object is not assigned.");
            return;
        }

        // Cache the Collider component of the mainObject
        mainCollider = mainObject.GetComponent<Collider>();

        // If the mainObject does not have a Collider, log an error
        if (mainCollider == null)
        {
            Debug.LogError("Main object does not have a Collider.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if mainObject is assigned and the fire is supposed to follow
        if (mainObject != null && toFollow)
        {
            // Determine the target position based on the main object's position
            Vector3 targetPosition = mainObject.position;

            // If mainCollider is available, adjust the fire's position above the collider
            if (mainCollider != null)
            {
                // Adjust the fire's position to be above the collider, offset by 'offsetHeight'
                targetPosition.y = mainCollider.bounds.max.y + offsetHeight;
            }

            // Smoothly interpolate between current fire position and target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
