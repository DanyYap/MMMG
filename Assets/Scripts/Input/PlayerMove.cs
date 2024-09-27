using UnityEngine;

public class PlayerMover
{
    private float speed;

    public PlayerMover(float moveSpeed)
    {
        speed = moveSpeed;
    }

    public void MoveWithRigidbody(Rigidbody rb, Vector2 inputDirection)
    {
        if (rb == null) return; // Ensure the Rigidbody is valid

        // Calculate the desired velocity
        Vector3 targetVelocity = new Vector3(inputDirection.x, rb.linearVelocity.y, inputDirection.y) * speed;

        // Update the Rigidbody's velocity
        rb.linearVelocity = targetVelocity;
    }
}