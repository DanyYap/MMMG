using UnityEngine;

public interface IRotatable
{
    void Rotate(Rigidbody rb, Vector2 inputDirection, Camera camera);
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

    public void Rotate(Rigidbody rb, Vector2 inputDirection, Camera camera)
    {
        if (inputDirection != Vector2.zero)
        {
            // Calculate the camera's forward direction
            Vector3 forward = camera.transform.forward;
            forward.y = 0; // Ignore vertical movement
            forward.Normalize(); // Normalize to get a unit vector

            // Calculate the desired direction, inverting the input direction
            Vector3 direction = forward * -inputDirection.y + camera.transform.right * -inputDirection.x + rotationOffset;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Smooth rotation
        }
    }
}
