using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionDestroyer : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public ParticleSystem spreadParticlePrefab; // Prefab for spreading water particles

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        // Set up the ParticleSystem to detect collisions
        var collisionModule = particleSystem.collision;
        collisionModule.enabled = true;
        collisionModule.type = ParticleSystemCollisionType.World;
        collisionModule.mode = ParticleSystemCollisionMode.Collision3D;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        // Only trigger when the water particles collide with a surface (e.g., ground, wall, etc.)
        if (other.CompareTag("Fire")) // Ensure the collider is tagged appropriately
        {
            Debug.Log(other);
        }
    }

    private void SpreadWater(Vector3 collisionPoint)
    {
        // Instantiate a new particle system to create spreading water at the collision point
        ParticleSystem spreadParticles = Instantiate(spreadParticlePrefab, collisionPoint, Quaternion.identity);
        spreadParticles.Play();

        // Optionally, you can destroy the particle system after some time to avoid memory buildup
        Destroy(spreadParticles.gameObject, spreadParticles.main.duration);
    }
}