using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    [SerializeField] private PlatformCanvasSpawner canvasSpawner;
    private PlatformInputManager platformInputManager;
    private PlayerModeManager playerModeManager;
    private IInputHandler inputHandler;
    private Vector2 player1MoveDirection;
    private Vector2 player2MoveDirection;

    private void Awake()
    {
        // Ensure only one instance exists
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
        // Listen for device changes
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        // Stop listening for device changes
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void InitializeInput()
    {
        // Initialize platform input and spawn the appropriate canvas
        platformInputManager = new PlatformInputManager();
        platformInputManager.InitializePlatformInput();
        canvasSpawner.SpawnCanvas(this, platformInputManager.CurrentPlatformInput);

        // Create input handler and set player mode
        inputHandler = platformInputManager.CreateInputHandler();
        playerModeManager = new PlayerModeManager(platformInputManager);
        playerModeManager.ChangePlayerMode(PlayerMode.Multiplayer);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Reinitialize input on device added or reconnected
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
        {
            InitializeInput();
        }
    }

    // Record move direction from input for player 1
    public void RecordPlayer1MoveDirection(InputAction.CallbackContext context) => player1MoveDirection = context.ReadValue<Vector2>();

    // Record move direction from input for player 2
    public void RecordPlayer2MoveDirection(InputAction.CallbackContext context) => player2MoveDirection = context.ReadValue<Vector2>();

    // Get the current move direction for player 1
    public Vector2 GetPlayer1MoveDirection() => inputHandler.GetPlayer1MoveDirection(player1MoveDirection);

    // Get the current move direction for player 2
    public Vector2 GetPlayer2MoveDirection() => inputHandler.GetPlayer2MoveDirection(player2MoveDirection);

    // Change player mode to Solo
    public void ChangePlayerMode() => playerModeManager.ChangePlayerMode(PlayerMode.Solo);
}