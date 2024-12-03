using UnityEngine;

public class Fire : MonoBehaviour
{
    public ParticleSystem fireParticles;  // The particle system representing the fire
    public bool BurnAtStart = true;
    public float Damage = 5.0f;

    private void Start()
    {
        if (BurnAtStart)
        {
            StartBurning();
        }
    }

    // Call this to start extinguishing the fire
    public void StartExtinguishing()
    {
        fireParticles.Stop();
    }

    // Call this to start burning the fire
    public void StartBurning()
    {
        fireParticles.Play();
    }

    public float HitDamage() => this.Damage;
}
