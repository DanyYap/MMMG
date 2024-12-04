using System.Collections;
using UnityEngine;

// Base class for fireable objects
public abstract class FireBase : MonoBehaviour, IFireable
{
    protected GameObject fire;
    protected ParticleSystem fireParticleSystem;
    protected FireBehavior fireBehavior;

    protected virtual void Start()
    {
        fire = FactoryManageSystem.Instance.ParticleSystemFactory.CreateInstance("Fire", transform.position, transform.rotation);
        fireParticleSystem = fire.GetComponent<ParticleSystem>();
        fireBehavior = new FireBehavior(fireParticleSystem);

        fire.transform.SetParent(transform);
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
        return fireBehavior.FadeInFireEffect(); 
    }

    protected IEnumerator FadeOutFireEffect()
    {
        return fireBehavior.FadeOutFireEffect(); 
    }

    private void FixedUpdate()
    {
        if (fire != null)
        {
            // Keep the fire facing upwards
            Quaternion targetRotation = Quaternion.Euler(-90, 0, 0);

            // If the fire's current rotation is not equal to the target, interpolate smoothly
            if (fire.transform.rotation != targetRotation)
            {
                fire.transform.rotation = Quaternion.Slerp(fire.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                fireBehavior.UpdateFire(rb.linearVelocity.y);
            }
        }
    }
}