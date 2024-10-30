using System;
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
    private InteractObjectAction interactAction = new();

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

        SetupButton(ButtonIdentifiers.RotateCameraButton,
            () => new RotateCameraAction(cameraController).Execute());
    }

    public void SetNewInteractAction(IInteractable interactableObject)
    {
        this.interactAction.Reinitialize(interactableObject);
    }

    public Vector2 GetJoystickDirection()
    {
        return gameJoystick.GetJoystickDirection();
    }

    private void SetupButton(string buttonId, UnityAction action)
    {
        Button button = GameObject.Find(buttonId)?.GetComponent<Button>();
        if (button != null)
        {
            new GameButton(button, action);
        }
    }
}
