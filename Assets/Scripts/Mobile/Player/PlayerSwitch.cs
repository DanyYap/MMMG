using UnityEngine;
using System.Collections.Generic;

public interface IPlayerSwitcher
{
    void Switch();
}

public class PlayerSwitcher : IPlayerSwitcher
{
    public static PlayerController SelectedPlayer;
    
    private List<GameObject> players;
    private int currentPlayerIndex;

    public PlayerSwitcher(List<GameObject> players)
    {
        this.players = players;
        currentPlayerIndex = 0;
    }

    public void Switch()
    {
        if (players.Count > 1)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            SetActivePlayer(currentPlayerIndex);
        }
    }

    private void SetActivePlayer(int index)
    {
        for (int i = 0; i < players.Count; i++)
        {
            var playerController = players[i].GetComponent<PlayerController>();
            playerController.enabled = (i == index);

            if (i == index)
            {
                PlayerSwitcher.SelectedPlayer = playerController;
            }
        }
    }

}