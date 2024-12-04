// Class for object fire behavior
using UnityEngine;

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
        StartCoroutine(FadeInFireEffect());
    }

    public override void Extinguish()
    {
        if (fire != null)
        {
            StartCoroutine(FadeOutFireEffect());
            Debug.Log("extinguish");
        }
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