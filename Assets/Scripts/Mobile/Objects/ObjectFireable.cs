// Class for object fire behavior
using UnityEngine;

[RequireComponent(typeof(ObjectHealth))]
public class ObjectFireable : FireBase
{
    [SerializeField] private bool isFiredAtStart = false;

    private new void Start()
    {
        base.Start();

        if (isFiredAtStart)
        {
            Ignite();
        }
    }

    public override void Ignite()
    {
        fireParticleSystem.Play();
    }

    public override void Extinguished()
    {
        fireParticleSystem.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try to get the IFireable component on the object directly first.
        if (other.TryGetComponent<IFireable>(out var fireable))
        {
            fireable.Ignite();
        }
        else
        {
            // If no IFireable component is found, check its children.
            var fireableInChildren = other.GetComponentInChildren<IFireable>();
            if (fireableInChildren != null)
            {
                fireableInChildren.Ignite();
            }
        }
    }

    public override void Interact()
    {
        
    }
}