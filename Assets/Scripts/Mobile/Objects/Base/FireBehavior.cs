using UnityEngine;

public class FireBehavior
{
    private ParticleSystem fireParticleSystem; // Reference to the Particle System for fire
    private float flammableValue; // Current flammable value
    private float maxFlammableValue; // Maximum flammable value for this fire

    public float FlammableValue => flammableValue;

    // Constructor that initializes the fire behavior with a Particle System and a maximum flammable value
    public FireBehavior(ParticleSystem particleSystem, float maxFlammableValue = 100f)
    {
        fireParticleSystem = particleSystem;
        this.maxFlammableValue = maxFlammableValue;
        flammableValue = maxFlammableValue; // Start at maximum intensity
        fireParticleSystem.Stop(); // Ensure fire is not emitting at the start
        UpdateFireEffect(); // Initialize the fire's appearance
    }

    // Set flammable value and update fire appearance
    public void SetFlammableValue(float value)
    {
        flammableValue = Mathf.Clamp(value, 0f, maxFlammableValue); // Clamp value between 0 and maxFlammableValue
        UpdateFireEffect(); // Update the fire's appearance based on the new percentage
    }

    // Update the fire's appearance based on the current flammable value percentage
    private void UpdateFireEffect()
    {
        float percentage = flammableValue / maxFlammableValue; // Calculate percentage of flammable value
        ParticleSystem.MainModule mainModule = fireParticleSystem.main; // Access the main module of the Particle System

        if (percentage > 0.66f) // Intense phase (above 66%)
        {
            mainModule.startColor = new Color(1f, 1f, 0f, 1f); // Bright yellow fire
            mainModule.startSize = new ParticleSystem.MinMaxCurve(2f); // Large size
            if (!fireParticleSystem.isPlaying) fireParticleSystem.Play(); // Ensure fire is active
        }
        else if (percentage > 0f) // Chill phase (between 1% and 66%)
        {
            mainModule.startColor = new Color(1f, 0.5f, 0.2f, 1f); // Dim orange fire
            mainModule.startSize = new ParticleSystem.MinMaxCurve(1f); // Smaller size
            if (!fireParticleSystem.isPlaying) fireParticleSystem.Play(); // Ensure fire is active
        }
        else // None phase (0%)
        {
            mainModule.startColor = new Color(0f, 0f, 0f, 0f); // Invisible
            mainModule.startSize = new ParticleSystem.MinMaxCurve(0.1f); // Minimal size
            fireParticleSystem.Stop(); // Stop the particle system
        }
    }
}
