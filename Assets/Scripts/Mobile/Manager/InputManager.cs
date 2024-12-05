using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public interface IInputManager
{
    void SetJoystick();
    void SetButtonAction(string buttonIdentifier, UnityAction action);
}

public class MobileInputManager : IInputManager
{
    private GameJoystick gameJoystick;
    private readonly SceneManageSystem sceneManageSystem;
    private readonly PlayerManageSystem playerManageSystem;
    private readonly CameraController cameraController;

    // Use a Dictionary to map ButtonIdentifiers to Button components
    private readonly Dictionary<string, UnityEngine.UI.Button> buttonComponents = new Dictionary<string, UnityEngine.UI.Button>();

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

        InitializeButtonActions();
    }

    public void SetJoystick()
    {
        FixedJoystick joystick = Object.FindAnyObjectByType<FixedJoystick>();
        gameJoystick = new GameJoystick(joystick);
    }

    // Set or remove an action for a button and toggle its visibility based on action presence
    public void SetButtonAction(string buttonIdentifier, UnityAction action = null)
    {
        // Find the button in the scene
        UnityEngine.UI.Button gameButton = GameObject.Find(buttonIdentifier)?.GetComponent<UnityEngine.UI.Button>();

        // If button exists, assign or clear the action
        if (gameButton != null)
        {
            if (action != null)
            {
                // Add button component to dictionary if not already present
                if (!buttonComponents.ContainsKey(buttonIdentifier))
                {
                    buttonComponents.Add(buttonIdentifier, gameButton);
                }

                // Assign the action to the button
                gameButton.onClick.RemoveAllListeners();  // Clear previous listeners
                gameButton.onClick.AddListener(action);  // Assign new action
            }
            else
            {
                // If no action is provided, clear all listeners and disable the button
                gameButton.onClick.RemoveAllListeners();  // Clear previous listeners

                // Remove button component from dictionary if no action is set
                if (buttonComponents.ContainsKey(buttonIdentifier))
                {
                    buttonComponents.Remove(buttonIdentifier);
                }
            }
        }
    }


    public Vector2 GetJoystickDirection() => gameJoystick.GetJoystickDirection();

    // Method to initialize button actions with their respective identifiers
    public void InitializeButtonActions()
    {
        // Adding actions for each button identifier
        SetButtonAction(ButtonIdentifiers.SoloGameButton,       () => new StartGameAction(sceneManageSystem).Execute());
        SetButtonAction(ButtonIdentifiers.PlayerSwitchButton,   () => new SwitchPlayerAction(playerManageSystem).Execute());
        SetButtonAction(ButtonIdentifiers.QuitGameButton,       () => new QuitGameAction().Execute());
        SetButtonAction(ButtonIdentifiers.BackToMenuButton,     () => new BackMenuAction(sceneManageSystem).Execute());
        SetButtonAction(ButtonIdentifiers.InteractButton,       () => new InteractObjectAction());
        SetButtonAction(ButtonIdentifiers.UseToolButton,        () => new InteractObjectAction());
        SetButtonAction(ButtonIdentifiers.RotateCameraButton,   () => new RotateCameraAction(cameraController).Execute());
    }
}
