using UnityEngine;

public class Water : MonoBehaviour
{
    // Method called when this particle system collides with another object
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Collided with new object: {other.name}");

        ExtinguishFire(other);
    }

    private void ExtinguishFire(GameObject other)
    {
        if (other.CompareTag("Fire")) // Check if the collided object is tagged as "Fire"
        {
            // Try to get the ParticleSystem component from the fire object
            ParticleSystem fireParticleSystem = other.GetComponent<ParticleSystem>();
            if (fireParticleSystem == null) return;

            // Stop the fire particle system if it's still active
            if (fireParticleSystem.isPlaying)
            {
                fireParticleSystem.Stop();
            }
        }
    }
}
