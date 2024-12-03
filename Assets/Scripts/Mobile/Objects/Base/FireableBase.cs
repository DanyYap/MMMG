using System.Collections;
using Unity.VisualScripting;
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
            Quaternion.Euler(-90, 0, 0), // TargetFollower facing upwards
            target.transform); // Make it a child of the target

        fireInstance.transform.localPosition = Vector3.zero; // Make sure fire is on the object
        return fireInstance;
    }
}

// Base class for fireable objects
public abstract class FireableBase : MonoBehaviour, IFireable
{
    protected ParticleEffectsLibrary effectsLibrary;
    protected FireFactory fireFactory;
    protected GameObject fire;
    protected ParticleSystem fireParticleSystem;
    protected FireBehavior fireBehavior;

    protected virtual void Awake()
    {
        effectsLibrary = ScriptableObjectManageSystem.Instance.EffectsLibrary;
        fireFactory = new FireFactory();
    }

    protected void CreateFireParticle()
    {
        fire = fireFactory.CreateFireParticle(gameObject, effectsLibrary.Fire); // Instantiate fire particles
        fireParticleSystem = fire.GetComponent<ParticleSystem>(); // Get the ParticleSystem component
        fireBehavior = new FireBehavior(fireParticleSystem);
    }

    public abstract void Ignite();
    public abstract void Extinguish();
    public abstract void Interact();

    public float GetFlammableValue()
    {
        if (fireBehavior == null) return 0f;

        return fireBehavior.FlammableValue;
    }

    protected IEnumerator FadeInFireEffect()
    {
        return fireBehavior.FadeInFireEffect(); // Delegate to FireBehavior
    }

    protected IEnumerator FadeOutFireEffect()
    {
        return fireBehavior.FadeOutFireEffect(); // Delegate to FireBehavior
    }

    private void FixedUpdate()
    {
        if (fire != null)
        {
            // Keep the fire facing upwards
            fire.transform.rotation = Quaternion.Euler(-90, 0, 0);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                fireBehavior.Update(rb.linearVelocity.y);
            }
        }
    }
}