using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private ParticleSystem ParticleSystem;

    private void Start()
    {  
        ParticleSystem = GetComponent<ParticleSystem>();

        // Set up the ParticleSystem to detect collisions
        var collisionModule = ParticleSystem.collision; // Get the collision module from the ParticleSystem instance
        collisionModule.enabled = true;
        collisionModule.type = ParticleSystemCollisionType.World;
        collisionModule.mode = ParticleSystemCollisionMode.Collision3D;
    }

    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log(other.tag);
        
        if (other.CompareTag("Player")) 
        {
            Debug.Log(other);
            other.GetComponent<PlayerHealth>()?.ReceiveDamage(1f);
        }
        else if (other.CompareTag("Fire") || other.CompareTag("Untagged"))
        {
            Debug.Log(other);
            other.GetComponent<ObjectHealth>()?.ReceiveDamage(1f);
        }
    }
}