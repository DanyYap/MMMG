using System.Collections.Generic;
using UnityEngine;

public class HealthManager
{
    private readonly List<PlayerHealth> playerHealths = new List<PlayerHealth>(); // Stores all player healths.
    private readonly Dictionary<string, float> playerHealthValues = new Dictionary<string, float>(); // Player health values.

    // Add a player health to the list.
    public void RegisterPlayerHealth(PlayerHealth playerHealth)
    {
        if (playerHealth != null && !playerHealths.Contains(playerHealth))
        {
            playerHealths.Add(playerHealth);
            playerHealthValues[playerHealth.gameObject.name] = playerHealth.CurrentHealth; // Initialize health value for new player.
        }
    }

    // Remove a player health from the list.
    public void UnregisterPlayerHealth(PlayerHealth playerHealth)
    {
        if (playerHealth != null && playerHealths.Contains(playerHealth))
        {
            playerHealths.Remove(playerHealth);
            playerHealthValues.Remove(playerHealth.gameObject.name); // Remove health value.
        }
    }

    // Get a specific player's current health by player name.
    public float GetPlayerHealth(string playerName)
    {
        if (playerHealthValues.ContainsKey(playerName))
        {
            return playerHealthValues[playerName];
        }
        return 0f; // Return 0 if the player is not found.
    }

    // Update the health value for a specific player.
    public void UpdatePlayerHealth(string playerName, float currentHealth)
    {
        if (playerHealthValues.ContainsKey(playerName))
        {
            playerHealthValues[playerName] = currentHealth; // Update health.
        }
    }

    // Get all player healths in a dictionary format.
    public Dictionary<string, float> GetAllPlayerHealths()
    {
        return new Dictionary<string, float>(playerHealthValues);
    }

    // Get the total summed health of all players.
    public float GetTotalHealth()
    {
        float totalHealth = 0f;
        foreach (var health in playerHealthValues.Values)
        {
            totalHealth += health; // Sum up each player's health.
        }
        return totalHealth;
    }

    // Clear all registered player healths.
    public void Clear()
    {
        playerHealths.Clear();
        playerHealthValues.Clear();
    }

    // Initialize the health manager with all current player healths in the scene.
    public void InitializeWithSceneHealthData()
    {
        playerHealths.Clear(); // Clear any existing player data.
        playerHealthValues.Clear();

        // Get all PlayerHealth components in the scene
        PlayerHealth[] allPlayerHealths = GameObject.FindObjectsByType<PlayerHealth>(FindObjectsSortMode.InstanceID);

        foreach (var playerHealth in allPlayerHealths)
        {
            RegisterPlayerHealth(playerHealth); // Add each player to the health manager.
        }
    }
}
