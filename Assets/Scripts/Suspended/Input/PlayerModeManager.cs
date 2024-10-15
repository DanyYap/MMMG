using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Different player modes 
[Serializable]
public enum PlayerMode
{
    Solo,
    Multiplayer,
}

// Manages the player's mode
public class PlayerModeManager
{
    private PlayerMode playerMode;
    private readonly PlatformInput cachedPlatformInput;
    private readonly PlayerInput playerInput;

    // Constructor
    public PlayerModeManager(PlatformInputManager platformInputManager)
    {
        cachedPlatformInput = platformInputManager.CurrentPlatformInput;
        playerInput = UnityEngine.Object.FindFirstObjectByType<PlayerInput>();
    }

    // Change the player mode
    public void ChangePlayerMode(PlayerMode newMode)
    {
        // Do nothing if on mobile or if the mode is the same
        if (cachedPlatformInput == PlatformInput.Mobile)
        {
            Debug.Log("Player mode change is not supported on Mobile platform.");
            return;
        }

        if (playerMode == newMode)
        {
            return;
        }

        Debug.Log($"Player mode changed to: {newMode}");
        playerMode = newMode;
        playerInput.SwitchCurrentActionMap(newMode.ToString());
    }

    // Get the current player mode
    public PlayerMode GetCurrentMode() => playerMode;
}
