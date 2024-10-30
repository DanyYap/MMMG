using UnityEngine;

public class PlayerManageSystem : MonoBehaviour
{
    public static PlayerManageSystem Instance { get; private set; }

    private IPlayerManager playerManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSystem();
            InitializeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateSystem()
    {
        playerManager = new PlayerManager();
    }

    public void InitializeSystem()
    {
        playerManager.InitializePlayers();
    }

    public IPlayerManager GetPlayerManager()
    {
        return playerManager;
    }
}
