using System;
using UnityEngine;

// Different types of input
[Serializable]
public enum PlatformInput
{
    KeyboardOrConsole,
    Mobile
}

public class PlatformInputManager
{
    // Current input type
    public PlatformInput CurrentPlatformInput { get; private set; }

    // Set the input type based on the platform
    public void InitializePlatformInput()
    {
        CurrentPlatformInput = Application.isMobilePlatform ? PlatformInput.Mobile : PlatformInput.KeyboardOrConsole;
    }

    // Create the right input handler
    public IInputHandler CreateInputHandler()
    {
        if (CurrentPlatformInput == PlatformInput.Mobile)
        {
            // Find the joystick for mobile
            var joystick = UnityEngine.Object.FindFirstObjectByType<FixedJoystick>();
            if (joystick == null)
            {
                Debug.LogError("FixedJoystick not found. Please ensure it is present in the scene.");
                return null;
            }
            return new MobileInputHandler(joystick);
        }
        // Use desktop input handler
        return new DesktopInputHandler();
    }
}
