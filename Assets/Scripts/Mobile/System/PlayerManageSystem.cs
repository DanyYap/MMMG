using UnityEngine;
using System.Collections.Generic;

public class PlayerManageSystem : MonoBehaviour
{
    public static PlayerManageSystem Instance;

    private List<GameObject> players;
    private IPlayerSwitcher playerSwitcher;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializePlayers();
    }

    public void InitializePlayers()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        playerSwitcher = new PlayerSwitcher(players);

        if (players.Count > 0)
        {
            playerSwitcher.Switch(); // Activate the first player
        }
    }

    public IPlayerSwitcher GetPlayerSwitcher() => playerSwitcher;
}