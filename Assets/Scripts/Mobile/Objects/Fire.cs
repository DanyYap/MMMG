using System.Collections;
using UnityEngine;

// This interface defines methods for anything that can catch fire
public interface IFireable
{
    void StartFire(); // Method to start fire
    void PutOutFire(); // Method to extinguish fire
}

// This class handles the fire behavior
public class Fire : MonoBehaviour, IFireable
{
    [SerializeField]
    private GameObject fireParticlePrefab; // The fire effect to show
    private FireParticleManager fireParticleManager; // Manages fire particles
    private GameObject fireParticles; // Tracks the fire particles
    private ParticleSystem fireParticleSystem; // To access particle system directly

    private void Awake()
    {
        fireParticleManager = gameObject.AddComponent<FireParticleManager>(); // Use AddComponent to initialize particle manager
    }

    public void StartFire()
    {
        if (fireParticles == null) // Create fire particles if none exist
        {
            CreateFireParticles();
        }
        else
        {
            StartCoroutine(FadeInFireParticles()); // Gradually activate existing fire particles
        }
    }

    private void CreateFireParticles()
    {
        fireParticles = fireParticleManager.CreateFireParticles(gameObject, fireParticlePrefab); // Instantiate fire particles
        fireParticleSystem = fireParticles.GetComponent<ParticleSystem>(); // Get the ParticleSystem component
    }

    private IEnumerator FadeInFireParticles()
    {
        fireParticles.SetActive(true); // Activate the fire particles GameObject
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        // Gradually increase the alpha value to 1
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Play(); // Start the particle system
    }

    public void PutOutFire()
    {
        if (fireParticles != null) // Check if an active fire exists
        {
            StartCoroutine(FadeOutFireParticles()); // Gradually deactivate fire particles
        }
    }

    private IEnumerator FadeOutFireParticles()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        // Gradually decrease the alpha value to 0
        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Stop(); // Stop the particle system
        fireParticles.SetActive(false); // Disable the fire particles GameObject
    }

    // Triggered when something enters the fire area
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IFireable>(out var fireable)) 
        {
            StartFire();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PutOutFire(); // Extinguish fire when exiting
    }
}
