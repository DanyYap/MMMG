using UnityEngine;

// This class represents an object that can catch fire
public class FlammableObject : MonoBehaviour, IFireable
{
    public FireParticleManager fireParticleManager; // This manages fire particles
    public GameObject fireParticlePrefab; // The fire effect to use

    private void Start()
    {
        StartFire();
    }

    public void StartFire()
    {
        fireParticleManager.CreateFireParticles(gameObject, fireParticlePrefab); // Show fire on this object
    }

    public void PutOutFire()
    {
        
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
