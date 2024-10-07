// This script finds buttons in the UI and gives them actions to do when clicked.
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UiComponentAttacher : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    // Initialize with PlayerInputManager
    public void Initialize(PlayerInputManager playerInputManager)
    {
        this.playerInputManager = playerInputManager;
        TrackButtonComponent();
    }

    private Button[] childButtons;
    // List of button names to track
    private readonly List<string> buttonNames = new()
    {
        "Spawn 2P Button",
        "Interact Button",
        "Jump Button"
    };

    // Find and track buttons
    public void TrackButtonComponent()
    {
        childButtons = GetComponentsInChildren<Button>();
        foreach (var button in childButtons)
        {
            if (buttonNames.Contains(button.name))
            {
                Debug.Log($"Button {button.name} exists in the child buttons.");
                AssignButtonAction(button);
            }
        }
    }

    // Assign actions to buttons
    private void AssignButtonAction(Button button)
    {
        switch (button.name)
        {
            case "Spawn 2P Button":
                button.onClick.AddListener(playerInputManager.ChangePlayerMode);
                break;
            default:
                Debug.LogWarning($"No action assigned for button {button.name}");
                break;
        }
    }
}
