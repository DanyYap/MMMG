using System.Collections.Generic;
using UnityEngine;

public interface IPlayerManager
{
    void InitializePlayers();
    void SwitchPlayer();
}

public class PlayerManager : IPlayerManager
{
    private List<GameObject> players;
    private IPlayerSwitcher playerSwitcher;

    public void InitializePlayers()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        playerSwitcher = new PlayerSwitcher(players);
        Debug.Log("yoooooooo " + players.Count);

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
}
