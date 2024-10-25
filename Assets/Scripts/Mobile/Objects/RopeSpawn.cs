using UnityEngine;
using System.Collections.Generic;

public class RopeSpawn : MonoBehaviour
{
    public Transform player; // The player's transform
    public float maxLength = 5f; // Maximum length of the hose
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Initially set to kinematic for controlled movement
    }

    void Update()
    {
        // Calculate distance from player to hose's end
        float distance = Vector3.Distance(player.position, transform.position);

        // Check if the distance exceeds max length
        if (distance > maxLength)
        {
            DetachHose();
        }
    }

    private void DetachHose()
    {
        // Set hose Rigidbody to non-kinematic to allow physics interaction
        rb.isKinematic = false;

        // Optionally, you can also apply a force or change its position
        // For example, you can make it fall or shoot in a specific direction
        // rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // Example force
    }
}

