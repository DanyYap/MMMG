using UnityEngine;

public abstract class FireBase : MonoBehaviour, IFireable
{
    protected GameObject fire; // Reference to the fire particle system GameObject
    protected ParticleSystem fireParticleSystem; // Reference to the fire's Particle System
    protected FireBehavior fireBehavior; // Handles fire behavior
    private IHealth health; // Reference to the health system (loosely coupled)

    [SerializeField] private Vector3 firePositionOffset = Vector3.zero; // Offset from the GameObject position
    [SerializeField] private float fireSize = 1f; // Default size

    protected virtual void Start()
    {
        // Try to find an IHealth implementation in the same GameObject
        health = GetComponent<IHealth>();

        if (health == null)
        {
            Debug.LogError($"No IHealth component found on {gameObject.name}. Fire behavior requires a health system.");
            return;
        }

        // Create the fire particle system instance with the position based on offset
        fire = FactoryManageSystem.Instance.ParticleSystemFactory.CreateInstance("Fire", transform.position + firePositionOffset, transform.rotation);
        fireParticleSystem = fire.GetComponent<ParticleSystem>();
        fireBehavior = new FireBehavior(fireParticleSystem, health.MaxHealth, fireSize);

        // Attach fire to this object
        fire.transform.SetParent(transform);

        UpdateFireBehavior(); // Sync fire visuals with the current health value

        // Subscribe to the health's death event
        if (health is HealthBase healthBase)
        {
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

            UpdateFireDirection();
        }
    }

    private void UpdateFireDirection()
    {
        // Keep the fire facing upwards
        Quaternion targetRotation = Quaternion.Euler(-90, 0, 0);

        // If the fire's current rotation is not equal to the target, interpolate smoothly
        if (fire.transform.rotation != targetRotation)
        {
            fire.transform.rotation = Quaternion.Slerp(fire.transform.rotation, targetRotation, Time.deltaTime * 5f);
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

    // Public method to adjust fire's position offset based on the GameObject's position
    public void SetFirePositionOffset(Vector3 offset)
    {
        firePositionOffset = offset;
        if (fire != null)
        {
            fire.transform.position = transform.position + firePositionOffset; // Adjust position based on the new offset
        }
    }

    // Public method to set the base fire size externally
    public void SetFireSize(float size)
    {
        fireSize = size;
        if (fireBehavior != null)
        {
            fireBehavior.SetBaseFireSize(size); // Adjust the fire behavior with the new size
        }
    }

    // TESTING PURPOSES ONLY
    private void Update()
    {
        SetFirePositionOffset(firePositionOffset);

        SetFireSize(fireSize);
    }
}
