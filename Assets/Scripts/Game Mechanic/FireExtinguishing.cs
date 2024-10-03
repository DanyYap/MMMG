using UnityEngine;

public class FireExtinguishing : MonoBehaviour
{
    public ParticleSystem waterParticleSystem; 
    public ParticleSystem fireParticleSystem; 
    public Collider fireCollider; 

    void Start()
    {
        
        var waterCollision = waterParticleSystem.collision;
        waterCollision.enabled = true;
    }

    void Update()
    {
        if (IsWaterExtinguishingFire())
        {
            ExtinguishFire();
        }
    }

    bool IsWaterExtinguishingFire()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[waterParticleSystem.main.maxParticles];
        int numParticlesAlive = waterParticleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            if (fireCollider.bounds.Contains(particles[i].position))
            {
                return true;
            }
        }

        return false;
    }

    void ExtinguishFire()
    {
        if (fireParticleSystem.isPlaying)
        {
            fireParticleSystem.Stop();
            Debug.Log("Fire is extinguished!");
        }
    }
    
}
