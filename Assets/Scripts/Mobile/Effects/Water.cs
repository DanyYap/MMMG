using UnityEngine;

public class Water : MonoBehaviour
{
    
    // Method called when this particle system collides with another object
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Collided with object: {other.name}");

        // Extinguish the fire when it collides
        ExtinguishFire(other);
    }
    
    // Method to handle the extinguishing of fire
    private void ExtinguishFire(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
            other.GetComponent<Fire>()?.StartExtinguishing();
        }
    }
}
