using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField] private PlatformCanvasSpawner canvasSpawner;
    private PlayerInput playerInput;
    private PlatformInput platformInput;
    private IInputHandler inputHandler;
    private Vector2 moveDirection;
    private CustomPlayerMode customPlayerMode;

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
        playerInput = GetComponent<PlayerInput>();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void InitializeInput()
    {
        AssignPlatformInput();
        playerInput = GetComponent<PlayerInput>();

        canvasSpawner.SpawnCanvas(platformInput);

        inputHandler = CreateInputHandler();

        // (Further changes) later will create an event to trigger this switch player mode
        customPlayerMode = new CustomPlayerMode(playerInput, PlayerMode.Solo);
    }

    private void AssignPlatformInput() => platformInput = Application.isMobilePlatform ? PlatformInput.Mobile : PlatformInput.KeyboardOrConsole;
    //private void AssignPlatformInput() => platformInput = PlatformInput.Mobile;

    private IInputHandler CreateInputHandler()
    {
        if (platformInput == PlatformInput.Mobile)
        {
            // Cache the joystick reference and check for null
            var joystick = FindAnyObjectByType<FixedJoystick>();
            if (joystick == null)
            {
                Debug.LogError("FixedJoystick not found. Please ensure it is in the scene.");
                return null; // Handle the error gracefully
            }
            return new MobileInputHandler(joystick);
        }
        else
        {
            return new DesktopInputHandler();
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change is InputDeviceChange.Added or InputDeviceChange.Reconnected)
        {
            InitializeInput();
        }
    }

    public void RecordMoveDirection(InputAction.CallbackContext context) => moveDirection = context.ReadValue<Vector2>();

    public Vector2 GetMoveDirection() => inputHandler.GetMoveDirection(moveDirection);
}