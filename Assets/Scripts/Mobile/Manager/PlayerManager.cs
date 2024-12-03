using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public interface IPlayerManager
{
    void InitializePlayers();
    void SwitchPlayer();
}

public class PlayerManager : IPlayerManager
{
    private List<GameObject> players;
    private IPlayerSwitcher playerSwitcher;

    public PlayerManager()
    {
        // Register to scene loaded and unloaded events
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Initialize players when a scene is loaded
        InitializePlayers();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Ensure players is not null before clearing it
        if (players != null)
        {
            players.Clear(); // Clear the list of players
            Debug.Log("Players cleared on scene unload.");
        }
        else
        {
            Debug.LogWarning("Players list was null on scene unload.");
        }

        // Ensure playerSwitcher is not null before using it
        if (playerSwitcher != null)
        {
            // Optionally perform any cleanup related to playerSwitcher
        }
        else
        {
            Debug.LogWarning("PlayerSwitcher was null on scene unload.");
        }
    }

    public void InitializePlayers()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        playerSwitcher = new PlayerSwitcher(players);

        if (players.Count > 0)
        {
            playerSwitcher.Switch(); // Activate the first player
        }
        else
        {
            Debug.Log("No player found");
        }
    }

    public void SwitchPlayer()
    {
        playerSwitcher?.Switch(); // Safely switch to the next player
    }

    private void OnDestroy()
    {
        // Unregister from the scene events when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
