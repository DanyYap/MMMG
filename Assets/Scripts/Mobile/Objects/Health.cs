using UnityEngine;

// Interface for status management
public interface IHealthStatus
{
    float CurrentHealth { get; set; }
    void UpdateHealth(float affectedValue); 
}

public class Health : MonoBehaviour, IHealthStatus
{
    private float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            UpdateHealth(value);
        }
    }

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minHealth = 0f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(float affectedValue) 
    {
        if (currentHealth == minHealth) return;

        currentHealth += affectedValue;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth); // Clamp to a range
    }
}