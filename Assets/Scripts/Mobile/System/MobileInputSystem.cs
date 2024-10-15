using UnityEngine;

public class MobileInputSystem : MonoBehaviour
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

    public void FindJoystick()
    {
        _fixedJoystick = FindFirstObjectByType<FixedJoystick>();
    }

    public Vector2 GetDirection()
    {
        return _fixedJoystick.Direction;
    }
}
