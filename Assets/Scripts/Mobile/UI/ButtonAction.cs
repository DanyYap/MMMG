using UnityEngine;
using UnityEngine.SceneManagement;

// Button action interface
public interface IButtonAction
{
    void Execute();
}

// Scenario-specific button actions
public class StartGameAction : IButtonAction
{
    private SceneManageSystem sceneManageSystem;

    public StartGameAction(SceneManageSystem sceneManageSystem)
    {
        this.sceneManageSystem = sceneManageSystem;
    }

    public void Execute()
    {
        sceneManageSystem.GetSceneManager().LoadSceneName(SceneNames.Level1Scene);
    }
}

public class RetryLevelAction : IButtonAction
{
    private SceneManageSystem sceneManageSystem;

    public RetryLevelAction(SceneManageSystem sceneManageSystem)
    {
        this.sceneManageSystem = sceneManageSystem;
    }

    public void Execute()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        sceneManageSystem.GetSceneManager().LoadSceneName(currentSceneName);
    }
}

public class NextLevelAction : IButtonAction
{
    private SceneManageSystem sceneManageSystem;

    public NextLevelAction(SceneManageSystem sceneManageSystem)
    {
        this.sceneManageSystem = sceneManageSystem;
    }

    public void Execute()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        sceneManageSystem.GetSceneManager().LoadNextScene();
    }
}

// TODO: will implement start coop button action

public class BackMenuAction : IButtonAction
{
    private SceneManageSystem sceneManageSystem;
    private GameManageSystem gameManageSystem;

    public BackMenuAction(SceneManageSystem sceneManageSystem, GameManageSystem gameManageSystem)
    {
        this.sceneManageSystem = sceneManageSystem;
        this.gameManageSystem = gameManageSystem;
    }

    public void Execute()
    {
        gameManageSystem.ResetGame();
        sceneManageSystem.GetSceneManager().LoadSceneName(SceneNames.MenuScene);
    }
}

public class QuitGameAction : IButtonAction
{
    public void Execute()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}

public class SwitchPlayerAction : IButtonAction
{
    private PlayerManageSystem playerManageSystem;

    public SwitchPlayerAction(PlayerManageSystem playerManageSystem)
    {
        this.playerManageSystem = playerManageSystem;
    }

    public void Execute()
    {
        playerManageSystem.GetPlayerManager().SwitchPlayer();
    }
}

public class InteractObjectAction : IButtonAction
{
    private IInteractable interactableObject;

    public InteractObjectAction(IInteractable interactableObject = null)
    {
        this.interactableObject = interactableObject;
    }

    public void Reinitialize(IInteractable interactableObject = null)
    {
        this.interactableObject = interactableObject;
    }

    public void Execute()
    {
        if (interactableObject == null) return;

        interactableObject.Interact();
    }
}

public class RotateCameraAction : IButtonAction
{
    private CameraController camera;
    private CameraRotate rotate;

    public RotateCameraAction(CameraController camera)
    {
        if (camera == null) return;
        this.camera = camera;
        rotate = camera.CameraRotate;
    }

    public void Execute()
    {
        if (camera == null) return;

        rotate.RotateCamera(RotationAxis.Horizontal);
    }
}