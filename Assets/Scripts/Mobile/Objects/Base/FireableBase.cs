using System.Collections;
using UnityEngine;

// This interface defines methods for anything that can catch fire
public interface IFireable : IInteractable
{
    void Ignite(); // Method to start fire
    void Extinguish(); // Method to extinguish fire
}

public class FireFactory
{
    // Factory Method to create fire particles
    public GameObject CreateFireParticle(GameObject target, GameObject fireParticle)
    {
        GameObject fireInstance = Object.Instantiate(
            fireParticle,
            target.transform.position,
            Quaternion.Euler(-90, 0, 0),
            target.transform);

        fireInstance.transform.localPosition = Vector3.zero; // Make sure fire is on the object
        return fireInstance;
    }
}

// Base class for fireable objects
public abstract class FireableBase : MonoBehaviour, IFireable
{
    protected ParticleEffectsScriptableObject effectsLibrary;
    protected FireFactory fireFactory;
    protected GameObject fire;
    protected ParticleSystem fireParticleSystem;

    protected virtual void Awake()
    {
        effectsLibrary = ScriptableObjectManageSystem.Instance.EffectsLibrary;
        fireFactory = new FireFactory();
    }

    protected void CreateFireParticle()
    {
        fire = fireFactory.CreateFireParticle(gameObject, effectsLibrary.Fire); // Instantiate fire particles
        fireParticleSystem = fire.GetComponent<ParticleSystem>(); // Get the ParticleSystem component
    }

    protected IEnumerator FadeInFireEffect()
    {
        fire.SetActive(true); // Activate the fire particles GameObject
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Play(); // Start the particle system
    }

    protected IEnumerator FadeOutFireEffect()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Stop(); // Stop the particle system
        fire.SetActive(false); // Disable the fire particles GameObject
    }

    public abstract void Ignite();
    public abstract void Extinguish();
    public abstract void Interact();
}