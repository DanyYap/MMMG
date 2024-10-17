using UnityEngine;

public interface IMobileInput
{
    Vector2 GetDirection();
}

public class MobileInputSystem : MonoBehaviour, IMobileInput
{
    public static MobileInputSystem Instance { get; private set; }

    private FixedJoystick _fixedJoystick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindJoystick();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FindJoystick()
    {
        _fixedJoystick = FindFirstObjectByType<FixedJoystick>();
    }

    public Vector2 GetDirection()
    {
        return _fixedJoystick.Direction;
    }
}