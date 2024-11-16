using System.Collections;
using UnityEngine;

public enum FireState
{
    Idle,
    Rising,
    Falling,
    Extinguished // State indicating the fire is extinguished
}

public class FireBehavior
{
    private ParticleSystem fireParticleSystem; // Reference to the Particle System for fire
    private FireState currentState; // Current state of the fire
    private float flammableValue = 0f; // Dynamic flammable value

    public float FlammableValue => flammableValue;

    // Constructor that initializes the fire behavior with a Particle System
    public FireBehavior(ParticleSystem particleSystem)
    {
        fireParticleSystem = particleSystem;
        currentState = FireState.Idle; // Start in Idle state
        fireParticleSystem.Stop(); // Ensure fire is not emitting at the start
    }

    // Update method to check and update the fire state based on vertical velocity
    public void Update(float verticalVelocity)
    {
        FireState newState = GetFireState(verticalVelocity);

        if (newState != currentState) // If the state has changed
        {
            currentState = newState; // Update the current state
            UpdateFireEffect(); // Update the fire's visual effects
        }

        AdjustFlammableValue(); // Adjust the flammable value based on current state
    }

    // Determine the new fire state based on vertical velocity
    private FireState GetFireState(float verticalVelocity)
    {
        if (verticalVelocity < 0) return FireState.Falling; // Falling state
        else if (verticalVelocity > 0) return FireState.Rising; // Rising state
        return currentState == FireState.Extinguished ? FireState.Extinguished : FireState.Idle; // Idle or Extinguished
    }

    // Update the fire's visual effects based on the current state
    private void UpdateFireEffect()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main; // Access the main module of the Particle System

        switch (currentState)
        {
            case FireState.Falling:
                HandleFallingState(mainModule); // Handle effects for falling state
                break;

            case FireState.Rising:
                HandleRisingState(mainModule); // Handle effects for rising state
                break;

            case FireState.Idle:
                HandleIdleState(mainModule); // Handle effects for idle state
                break;

            case FireState.Extinguished:
                HandleExtinguishedState(mainModule); // Handle effects for extinguished state
                break;
        }

        ApplyInstabilityEffect(mainModule); // Apply instability effects based on size
    }

    // Adjust the flammable value dynamically based on the fire's state and time
    private void AdjustFlammableValue()
    {
        switch (currentState)
        {
            case FireState.Rising:
                flammableValue = Mathf.Min(flammableValue + Time.deltaTime * 5f, 100f); // Increase intensity
                break;
            case FireState.Falling:
                flammableValue = Mathf.Max(flammableValue - Time.deltaTime * 3f, 1f); // Decrease intensity
                break;
            case FireState.Idle:
                flammableValue = Mathf.Max(flammableValue - Time.deltaTime * 1f, 1f); // Gradual decay when idle
                break;
            case FireState.Extinguished:
                flammableValue = 0f; // Set to zero when extinguished
                break;
        }
    }

    // Handle visual effects when the fire is falling
    private void HandleFallingState(ParticleSystem.MainModule mainModule)
    {
        mainModule.startColor = new Color(1f, 0.5f, 0.5f, 1f); // Set cooler shade for falling fire
        mainModule.startSize = new ParticleSystem.MinMaxCurve(Mathf.Max(0.5f, mainModule.startSize.constant - 0.1f)); // Reduce size
    }

    // Handle visual effects when the fire is rising
    private void HandleRisingState(ParticleSystem.MainModule mainModule)
    {
        mainModule.startColor = new Color(1f, 1f, 0f, 1f); // Set hotter color for rising fire
        mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant + 0.1f); // Increase size
    }

    // Handle visual effects when the fire is idle
    private void HandleIdleState(ParticleSystem.MainModule mainModule)
    {
        mainModule.startColor = new Color(1f, 0.8f, 0f, 1f); // Set normal fire color for idle state
        mainModule.startSize = new ParticleSystem.MinMaxCurve(Mathf.Max(0.75f, mainModule.startSize.constant)); // Maintain slightly reduced size
    }

    // Handle visual effects when the fire is extinguished
    private void HandleExtinguishedState(ParticleSystem.MainModule mainModule)
    {
        mainModule.startColor = new Color(1f, 0.3f, 0f, 1f); // Set dimmed fire color for extinguished state
        mainModule.startSize = new ParticleSystem.MinMaxCurve(0.1f); // Set very small size
        fireParticleSystem.Stop(); // Ensure the fire is not emitting particles
    }

    // Apply instability effects based on the size of the fire
    private void ApplyInstabilityEffect(ParticleSystem.MainModule mainModule)
    {
        float currentSize = mainModule.startSize.constant; // Get the current size of the fire

        if (currentSize > 1.5f) // Threshold for instability
        {
            // Apply random flickering color for instability
            mainModule.startColor = new Color(Random.Range(0.8f, 1f), Random.Range(0f, 0.5f), 0f, 1f);
            // Randomly adjust size for instability
            mainModule.startSize = new ParticleSystem.MinMaxCurve(currentSize * Random.Range(0.8f, 1.2f));
        }

        // Play or stop the particle system based on the current state
        if (currentState != FireState.Extinguished)
        {
            fireParticleSystem.Play();
        }
        else
        {
            fireParticleSystem.Stop();
        }
    }

    // Coroutine to fade in the fire effect
    public IEnumerator FadeInFireEffect()
    {
        fireParticleSystem.gameObject.SetActive(true); // Activate the fire particles GameObject
        ParticleSystem.MainModule mainModule = fireParticleSystem.main; // Access the main module
        Color startColor = mainModule.startColor.color; // Get the current color

        // Gradually increase the alpha value to create a fade-in effect
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Play(); // Start the particle system
    }

    // Coroutine to fade out the fire effect
    public IEnumerator FadeOutFireEffect()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main; // Access the main module
        Color startColor = mainModule.startColor.color; // Get the current color

        // Gradually decrease the alpha value to create a fade-out effect
        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Stop(); // Stop the particle system
        fireParticleSystem.gameObject.SetActive(false); // Disable the fire particles GameObject
    }
}