using System.Collections;
using UnityEngine;

// Base class for fireable objects
public abstract class FireableBase : MonoBehaviour, IFireable
{
    protected GameObject fire;
    protected ParticleSystem fireParticleSystem;
    protected FireBehavior fireBehavior;

    //TESTING ONLY
    public GameObject FireGO;

    protected virtual void Start()
    {
        var effectFactory = FactoryManageSystem.Instance.ParticleSystemFactory;

        fire = effectFactory.CreateInstance("Fire", transform.position, transform.rotation);
        fireParticleSystem = fire.GetComponent<ParticleSystem>();
        fireBehavior = new FireBehavior(fireParticleSystem);

        fire.transform.SetParent(transform);
        FireGO = fire;
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
            fire.transform.rotation = Quaternion.Euler(-90, 0, 0);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                fireBehavior.Update(rb.linearVelocity.y);
            }
        }
    }
}