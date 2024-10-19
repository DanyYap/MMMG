// IMovable interface for movement
using UnityEngine;

public interface IMovable
{
    void Move(Rigidbody rb, Vector2 inputDirection, bool isMoving);
}

public class PlayerMover : IMovable
{
    private readonly float speed;
    private readonly IRotatable rotator;

    public PlayerMover(float moveSpeed, IRotatable rotator)
    {
        speed = moveSpeed;
        this.rotator = rotator;
    }

    public void Move(Rigidbody rb, Vector2 inputDirection, bool isMoving)
    {
        if (rb == null) return;
        isMoving = inputDirection != Vector2.zero;

        // Use efficient calculations for target velocity
        Vector3 targetVelocity = new Vector3(inputDirection.x, rb.linearVelocity.y, inputDirection.y) * speed;
        rb.linearVelocity = targetVelocity; // Set velocity directly

        // Rotate player to face moving direction
        rotator.Rotate(rb, inputDirection);
    }
}

