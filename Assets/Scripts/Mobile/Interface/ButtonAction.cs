using UnityEngine;

// Button action interface
public interface IButtonAction
{
    void Execute();
}

// Scenario-specific button actions
public class StartGameAction : IButtonAction
{
    private ISceneController sceneController;

    public StartGameAction(ISceneController sceneController)
    {
        this.sceneController = sceneController;
    }

    public void Execute()
    {
        Debug.Log("load game scene");
        sceneController.LoadScene(SceneNames.GameScene);
    }
}

// TODO: will implement start coop button action

public class BackMenuAction : IButtonAction
{
    private ISceneController sceneController;

    public BackMenuAction(ISceneController sceneController)
    {
        this.sceneController = sceneController;
    }

    public void Execute()
    {
        Debug.Log("load game scene");
        sceneController.LoadScene(SceneNames.MenuScene);
    }
}

public class SwitchPlayerAction : IButtonAction
{
    private IPlayerSwitcher playerSwitcher;

    public SwitchPlayerAction(IPlayerSwitcher playerSwitcher)
    {
        this.playerSwitcher = playerSwitcher;
    }

    public void Execute()
    {
        playerSwitcher.Switch();
    }
}

public class InteractObjectAction : IButtonAction
{
    private readonly GameObject interactableObject;

    public InteractObjectAction(GameObject interactableObject)
    {
        this.interactableObject = interactableObject;
    }

    public void Execute()
    {
        if (interactableObject == null) return;


    }
}