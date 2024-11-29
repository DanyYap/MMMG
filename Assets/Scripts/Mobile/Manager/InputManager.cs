using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IInputManager
{
    void SetupJoystick();
    void SetupButtonActions();
}

public class MobileInputManager : IInputManager
{
    private GameJoystick gameJoystick;
    private InteractObjectAction[] interactActions;
    private InteractObjectAction interactAction = new();
    private InteractObjectAction interactAction_2 = new();
    
    private readonly SceneManageSystem sceneManageSystem;
    private readonly PlayerManageSystem playerManageSystem;
    private readonly CameraController cameraController;

    public MobileInputManager(
        SceneManageSystem sceneManageSystem,
        PlayerManageSystem playerManageSystem,
        CameraController cameraController)
    {
        ComponentValidator.ValidateNotNull(sceneManageSystem, nameof(sceneManageSystem));
        ComponentValidator.ValidateNotNull(playerManageSystem, nameof(playerManageSystem));

        this.sceneManageSystem = sceneManageSystem;
        this.playerManageSystem = playerManageSystem;
        this.cameraController = cameraController;

        interactActions = new InteractObjectAction[] { interactAction, interactAction_2 };
    }

    public void SetupJoystick()
    {
        FixedJoystick joystick = UnityEngine.Object.FindAnyObjectByType<FixedJoystick>();
        gameJoystick = new GameJoystick(joystick);
    }

    public void SetupButtonActions()
    {
        SetupButton(ButtonIdentifiers.SoloGameButton,
            () => new StartGameAction(sceneManageSystem).Execute());

        SetupButton(ButtonIdentifiers.PlayerSwitchButton,
            () => new SwitchPlayerAction(playerManageSystem).Execute());

        SetupButton(ButtonIdentifiers.BackToMenuButton,
            () => new BackMenuAction(sceneManageSystem).Execute());

        SetupButton(ButtonIdentifiers.InteractButton,
            () => interactAction.Execute());

        SetupButton(ButtonIdentifiers.UseToolButton,
            () => interactAction_2.Execute());

        SetupButton(ButtonIdentifiers.RotateCameraButton,
            () => new RotateCameraAction(cameraController).Execute());
    }

    public void SetNewInteractAction(IInteractable interactableObject, int index)
    {
        if (index >= 0 && index <= interactActions.Length)
        {
            interactActions[index].Reinitialize(interactableObject);
        }
    }

    public Vector2 GetJoystickDirection()
    {
        return gameJoystick.GetJoystickDirection();
    }

    private void SetupButton(string buttonId, UnityAction action)
    {
        UnityEngine.UI.Button button = GameObject.Find(buttonId)?.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            new Button(button, action);
        }
    }
}
