using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum PlayerMode
{
    Solo,
    Multiplayer1,
    Multiplayer2
}

[Serializable]
public enum PlatformInput
{
    KeyboardOrConsole,
    Mobile
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    private PlayerInput playerInputComponent;
    private FixedJoystick fixedJoystick;
    private PlatformInput platformInput;
    [SerializeField] private PlayerMode playerMode;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInput();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        playerInputComponent.enabled = true;
    }

    private void OnDisable()
    {
        playerInputComponent.enabled = false;
    }

    private void InitializeInput()
    {
        playerInputComponent = GetComponent<PlayerInput>();
        platformInput = DeterminePlatformInput();
        fixedJoystick = FindAnyObjectByType<FixedJoystick>();
        SwitchActionMap();
    }

    private PlatformInput DeterminePlatformInput()
    {
        return Application.isMobilePlatform ? PlatformInput.Mobile : PlatformInput.KeyboardOrConsole;
    }

    private void SwitchActionMap()
    {
        playerInputComponent.SwitchCurrentActionMap(playerMode.ToString());
    }

    public bool IsMobile() => platformInput == PlatformInput.Mobile;

    public Vector2 GetJoystickDirection() => fixedJoystick.Direction;
}