// IMovable interface for movement
using UnityEngine;

public interface IMovable
{
    void Move(Rigidbody rb, Vector2 inputDirection);
}

public class PlayerMover : IMovable
{
    private readonly float speed;

    public PlayerMover(float moveSpeed)
    {
        speed = moveSpeed;
    }

    public void Move(Rigidbody rb, Vector2 inputDirection)
    {
        if (rb == null) return;

        // Use efficient calculations for target velocity
        Vector3 targetVelocity = new Vector3(inputDirection.x, rb.linearVelocity.y, inputDirection.y) * speed;
        rb.linearVelocity = targetVelocity; // Set velocity directly
    }
}