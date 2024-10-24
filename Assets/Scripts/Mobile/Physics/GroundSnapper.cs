using UnityEngine;

public class GroundSnapper : MonoBehaviour
{
    public float snapDistance = 0.1f; // Distance to snap above the ground
    public LayerMask groundLayer; // Layer representing the ground

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Check if Rigidbody is attached
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject.");
        }
    }

    void Update()
    {
        SnapToGround();
    }

    private void SnapToGround()
    {
        // Ensure Rigidbody is not null
        if (rb != null && !rb.isKinematic)
        {
            RaycastHit hit;
            // Perform the raycast
            if (Physics.Raycast(transform.position, Vector3.down, out hit, snapDistance + 1f, groundLayer))
            {
                // Snap to the ground
                Vector3 newPosition = hit.point + Vector3.up * snapDistance;
                transform.position = newPosition;
            }
        }
    }

    private void OnDisable()
    {
        SnapToGround();
    }

    private void OnEnable()
    {
        SnapToGround();
    }
}