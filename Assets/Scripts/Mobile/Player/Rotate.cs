using UnityEngine;

public interface IRotatable
{
    void Rotate(Rigidbody rb, Vector2 inputDirection);
}

public class PlayerRotator : IRotatable
{
    private readonly float rotationSpeed;
    private readonly Vector3 rotationOffset;

    public PlayerRotator(float rotationSpeed = 10f, Vector3 rotationOffset = default)
    {
        this.rotationSpeed = rotationSpeed;
        this.rotationOffset = rotationOffset == default ? Vector3.zero : rotationOffset;
    }

    public void Rotate(Rigidbody rb, Vector2 inputDirection)
    {
        if (inputDirection != Vector2.zero)
        {
            Vector3 direction = new Vector3(-inputDirection.x, 0, -inputDirection.y) + rotationOffset;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Smooth rotation
        }
    }
}
