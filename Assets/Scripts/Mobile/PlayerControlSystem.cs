using UnityEngine;
using System.Collections.Generic;

public class PlayerControlSystem : MonoBehaviour
{
    private List<GameObject> players; // List of players
    private int currentPlayerIndex; // Index of the current player

    private void Start()
    {
        InitializePlayers();
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
    }

    // This method is called when the switch player button is clicked
    public void OnSwitchPlayerButtonClick()
    {
        if (players.Count > 1)
        {
            // Move to the next player
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            SetActivePlayer(currentPlayerIndex); // Activate the new current player
        }
    }

    // This method activates the player at the given index
    private void SetActivePlayer(int index)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Enable the player controller only for the active player
            players[i].GetComponent<PlayerController>().enabled = (i == index);
        }
    }
}
