using UnityEngine;

public class JoystickManager
{
    private FixedJoystick gameJoystick;

    public JoystickManager(FixedJoystick gameJoystick)
    {
        this.gameJoystick = gameJoystick;
    }

    public void SetJoystick()
    {
        FixedJoystick joystick = Object.FindAnyObjectByType<FixedJoystick>();
        gameJoystick = joystick;
    }

    public Vector2 GetJoystickDirection()
    {
        if (gameJoystick == null) return Vector2.zero;
        return gameJoystick.Direction;
    }
}
