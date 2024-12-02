// Class for object fire behavior
using UnityEngine;

public class ObjectFireable : FireableBase
{
    [SerializeField] private bool isFiredAtStart = false;

    private void Start()
    {
        if (isFiredAtStart)
        {
            Ignite();
        }
    }

    public override void Ignite()
    {
        if (fire == null)
        {
            CreateFireParticle();
        }
        else
        {
            StartCoroutine(FadeInFireEffect());
        }
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
        HandleFireInteraction(other); // Add additional behavior specific to ObjectFireable.
    }

    private void HandleFireInteraction(Collider other)
    {
        if (other.TryGetComponent<IFireable>(out var fireable))
        {
            Ignite();
        }
        else
        {
            var fireablesInChildren = other.GetComponentsInChildren<IFireable>();
            if (fireablesInChildren.Length > 0)
            {
                fireablesInChildren[0].Ignite();
            }
        }
    }

    public override void Interact()
    {
        
    }
}