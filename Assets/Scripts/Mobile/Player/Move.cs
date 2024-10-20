// IMovable interface for movement
using UnityEngine;
using UnityEngine.UIElements;

public interface IMovable
{
    void Move(Rigidbody rb, Vector2 inputDirection, bool isMoving);
}

public class PlayerMover : IMovable
{
    private readonly float speed;
    private readonly IRotatable rotator;
    private readonly Camera camera; // Reference to the camera

    public PlayerMover(float moveSpeed, IRotatable rotator, Camera camera)
    {
        speed = moveSpeed;
        this.rotator = rotator;
        this.camera = camera; // Store the camera reference
    }

    public void Move(Rigidbody rb, Vector2 inputDirection, bool isMoving)
    {
        if (rb == null) return;
        isMoving = inputDirection != Vector2.zero;

        // Calculate camera's forward direction
        Vector3 forward = camera.transform.forward;
        forward.y = 0; // Flatten the vector to ignore vertical movement
        forward.Normalize(); // Normalize to get a unit vector

        // Calculate right direction based on camera's orientation
        Vector3 right = camera.transform.right;

        // Determine the desired movement direction
        Vector3 targetDirection = (forward * inputDirection.y + right * inputDirection.x).normalized;

        // Use efficient calculations for target velocity
        Vector3 targetVelocity = targetDirection * speed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z); // Maintain vertical velocity

        rotator.Rotate(rb, inputDirection, camera); // Pass the camera reference for rotation
    }
}