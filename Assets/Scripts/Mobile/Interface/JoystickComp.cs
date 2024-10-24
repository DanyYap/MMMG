using UnityEngine;

public interface IJoystickComponent
{
    void ReinitializeJoystick(FixedJoystick joystick);
    Vector2 GetJoystickDirection(); // Add this method to the interface
}

public class GameJoystick : IJoystickComponent
{
    private FixedJoystick joystick;

    public GameJoystick(FixedJoystick joystick)
    {
        this.joystick = joystick;
    }

    public void ReinitializeJoystick(FixedJoystick joystick)
    {
        this.joystick = joystick;
    }

    public Vector2 GetJoystickDirection()
    {
        if (joystick == null) return Vector2.zero;

        return joystick.Direction;
    }
}

