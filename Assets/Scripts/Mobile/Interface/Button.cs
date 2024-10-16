using UnityEngine;

public static class ButtonIdentifiers
{
    // Main menu buttons
    public const string SoloGameButton = "Solo Game Button";
    public const string MultiplayerButton = "Multiplayer Button";

    // Lobby buttons
    public const string CreateLobbyButton = "Create Lobby Button";
    public const string JoinLobbyButton = "Join Lobby Button";
    public const string ExitLobbyButton = "Exit Lobby Button";
    public const string BackToMenuButton = "Back To Menu Button";

    // In-game buttons
    public const string ExitGameButton = "Exit Game Button";
    public const string PlayerSwitchButton = "Player Switch Button";
}

// Button action interface
public interface IButtonAction
{
    void Execute();
}

// Scenario-specific button actions
public class StartSoloGameAction : IButtonAction
{
    private CanvasManageSystem canvasManageSystem;
    private SceneManageSystem sceneManageSystem;

    public StartSoloGameAction(
        CanvasManageSystem canvasManageSystem,
        SceneManageSystem sceneManageSystem)
    {
        this.canvasManageSystem = canvasManageSystem;
        this.sceneManageSystem = sceneManageSystem;
    }

    public void Execute()
    {
        canvasManageSystem.OnGameModeChange(true);
        sceneManageSystem.OnLoadSceneButtonClick("GameScene");
    }
}

public class InteractObjectAction : IButtonAction
{
    private GameObject interactableObject;

    public InteractObjectAction(GameObject interactableObject)
    {
        this.interactableObject = interactableObject;
    }

    public void Execute()
    {
        if (interactableObject == null) return;


    }
}
