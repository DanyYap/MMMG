using UnityEngine;

public interface IInputHandler
{
    Vector2 GetMoveDirection(Vector2 moveDirection);
}

public class MobileInputHandler : IInputHandler
{
    private readonly FixedJoystick fixedJoystick;

    public MobileInputHandler(FixedJoystick joystick)
    {
        fixedJoystick = joystick;
    }

    // mobile moveDirection is unused
    public Vector2 GetMoveDirection(Vector2 moveDirection)
    {
        return fixedJoystick.Direction;
    }
}

public class DesktopInputHandler : IInputHandler
{
    public Vector2 GetMoveDirection(Vector2 moveDirection)
    {
        return moveDirection;
    }
}

