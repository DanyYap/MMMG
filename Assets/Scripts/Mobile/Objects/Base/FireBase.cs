using UnityEngine;

public abstract class FireBase : MonoBehaviour, IFireable
{
    protected GameObject fire; // Reference to the fire particle system GameObject
    protected ParticleSystem fireParticleSystem; // Reference to the fire's Particle System
    protected FireBehavior fireBehavior; // Handles fire behavior
    private IHealth health; // Reference to the health system (loosely coupled)

    protected virtual void Start()
    {
        // Try to find an IHealth implementation in the same GameObject
        health = GetComponent<IHealth>();

        if (health == null)
        {
            Debug.LogError($"No IHealth component found on {gameObject.name}. Fire behavior requires a health system.");
            return;
        }

        // Create the fire particle system instance
        fire = FactoryManageSystem.Instance.ParticleSystemFactory.CreateInstance("Fire", transform.position, transform.rotation);
        fireParticleSystem = fire.GetComponent<ParticleSystem>();
        fireBehavior = new FireBehavior(fireParticleSystem, health.MaxHealth); // Initialize with max health as max flammable value

        // Attach fire to this object
        fire.transform.SetParent(transform);

        UpdateFireBehavior(); // Sync fire visuals with the current health value

        // Subscribe to the health's death event
        if (health is HealthBase healthBase)
        {
            // Add a listener method to the OnDeathEvent
            healthBase.AddOnDeathListener(HandleDeath);
        }
    }

    public abstract void Ignite(); // Abstract method to ignite the object
    public abstract void Extinguished(); // Abstract method to extinguish the object
    public abstract void Interact(); // Abstract method for interaction

    // Update fire visuals based on current health
    protected void UpdateFireBehavior()
    {
        if (health != null)
        {
            fireBehavior.SetFlammableValue(health.CurrentHealth); // Set flammable value to match current health
        }
    }

    private void FixedUpdate()
    {
        if (fire != null && health != null)
        {
            // Dynamically sync flammable value with health in each frame
            UpdateFireBehavior();
        }
    }

    private void HandleDeath()
    {
        if (fire != null)
        {
            // Return the fire to the pool
            FactoryManageSystem.Instance.ParticleSystemFactory.ReturnToPool("Fire", fire);
            fire = null;
        }
    }

    protected virtual void OnDestroy()
    {
        if (health is HealthBase healthBase)
        {
            healthBase.RemoveOnDeathListener(HandleDeath); // Unsubscribe to avoid memory leaks
        }
    }
}
