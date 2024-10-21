using System.Collections;
using UnityEngine;

public enum FireState
{
    Idle,
    Rising,
    Falling,
    Extinguished // New extinguished state
}

public class FireBehavior
{
    private ParticleSystem fireParticleSystem;
    private FireState currentState;

    public FireBehavior(ParticleSystem particleSystem)
    {
        fireParticleSystem = particleSystem;
        currentState = FireState.Idle;
        fireParticleSystem.Stop();
    }

    public void Update(float verticalVelocity)
    {
        FireState newState = GetFireState(verticalVelocity);

        if (newState != currentState)
        {
            currentState = newState;
            UpdateFireEffect();
        }
    }

    private FireState GetFireState(float verticalVelocity)
    {
        if (verticalVelocity < 0) return FireState.Falling;
        else if (verticalVelocity > 0) return FireState.Rising;
        return currentState == FireState.Extinguished ? FireState.Extinguished : FireState.Idle;
    }

    private void UpdateFireEffect()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;

        switch (currentState)
        {
            case FireState.Falling:
                mainModule.startColor = new Color(1f, 0.5f, 0.5f, 1f); // Cooler shade
                mainModule.startSize = new ParticleSystem.MinMaxCurve(Mathf.Max(0.5f, mainModule.startSize.constant - 0.1f)); // Reduce size
                break;

            case FireState.Rising:
                mainModule.startColor = new Color(1f, 1f, 0f, 1f); // Hotter color
                mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant + 0.1f); // Increase size
                break;

            case FireState.Idle:
                // Normal fire state but less intense
                mainModule.startColor = new Color(1f, 0.8f, 0f, 1f); // Normal fire color
                mainModule.startSize = new ParticleSystem.MinMaxCurve(0.75f); // Slightly reduced size
                break;

            case FireState.Extinguished:
                // Fire is minimal or extinguished
                mainModule.startColor = new Color(1f, 0.3f, 0f, 1f); // Dimmed fire color
                mainModule.startSize = new ParticleSystem.MinMaxCurve(0.1f); // Very small size
                fireParticleSystem.Stop(); // Ensure it's not actively emitting particles
                break;
        }

        if (currentState != FireState.Extinguished)
        {
            fireParticleSystem.Play();
        }
        else
        {
            fireParticleSystem.Stop();
        }
    }

    public IEnumerator FadeInFireEffect()
    {
        fireParticleSystem.gameObject.SetActive(true); // Activate the fire particles GameObject
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Play(); // Start the particle system
    }

    public IEnumerator FadeOutFireEffect()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

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