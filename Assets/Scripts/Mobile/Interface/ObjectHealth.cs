using UnityEngine;

public class ObjectHealth : HealthBase
{
    public override void ReceiveDamage(float damageAmount)
    {
        base.ReceiveDamage(damageAmount); // Use base behavior to handle damage.
        GameManageSystem.Instance.GetHealthSceneManager.UpdateHealthData("object", gameObject.name, CurrentHealth);
        Debug.Log($"Object-specific logic: Play a hit animation or sound.");
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        Debug.Log($"Object-specific logic: Play a heal animation or sound.");
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Debug.Log($"{gameObject.name} has been destroyed!");
        // Add object-specific death logic here (e.g., destroy object or trigger animation).
        //Destroy(gameObject); // Example: Destroy the object when health reaches 0.
    }
}
