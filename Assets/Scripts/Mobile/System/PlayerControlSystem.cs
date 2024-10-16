using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerControlSystem : MonoBehaviour
{
    private List<GameObject> players; // List of players
    private int currentPlayerIndex; // Index of the current player
    private GrabToolsSystem _grabToolScript;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "FireHoseScene")
        {
            InitializePlayers(); // Only initialize players when GameScene is loaded
        }
    }

    public void InitializePlayers()
    {
        // Find all players in the game
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        if (players.Count > 0)
        {
            currentPlayerIndex = 0; // Start with the first player
            SetActivePlayer(currentPlayerIndex); // Activate the first player
        }
        else
        {
            Debug.LogError("No players found in the scene with the 'Player' tag!");
        }
    }

    // Switch player when button is clicked
    public void OnSwitchPlayerButtonClick()
    {
        if (players.Count > 1)
        {
            // Move to the next player
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            SetActivePlayer(currentPlayerIndex); // Activate the new current player
        }
    }

    // Activate player at given index
    private void SetActivePlayer(int index)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Enable the player controller only for the active player
            bool isActive = (i == index);
            players[i].GetComponent<PlayerController>().enabled = isActive;

            // If this is the active player, get its GrabToolsSystem component
            if (isActive)
            {
                _grabToolScript = players[i].GetComponent<GrabToolsSystem>();

                if (_grabToolScript == null)
                {
                    Debug.LogError($"No GrabToolsSystem found on player '{players[i].name}'!");
                }
                else
                {
                    Debug.Log($"GrabToolsSystem found on player '{players[i].name}'.");
                }
            }
        }
    }

    public void OnGrabObjectClick()
    {
        if (_grabToolScript != null)
        {
            _grabToolScript.GrabObject();
        }
        else
        {
            Debug.LogError("No GrabToolsSystem found on active player when trying to grab object!");
        }
    }

    public void OnReleaseObjectClick()
    {
        if (_grabToolScript != null)
        {
            _grabToolScript.ReleaseObject();
        }
        else
        {
            Debug.LogError("No GrabToolsSystem found on active player!");
        }
    }

    public void OnUseToolsClick()
    {
        if (_grabToolScript != null)
        {
            _grabToolScript.UseTools();
        }
        else
        {
            Debug.LogError("No GrabToolsSystem found on active player!");
        }
    }
}
