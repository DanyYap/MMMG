using System;
using UnityEngine;

public class PlayerState
{
    public bool IsMoving    { get; set; }
    public bool IsGrabbing  { get; set; }
    public bool IsBurning    { get; set; }

    public PlayerState()
    {
        IsMoving = false;
        IsGrabbing = false;
        IsBurning = false;
    }

    public void SetState(Action<bool> stateSetter, bool flag)
    {
        stateSetter(flag);
    }
}

public class PlayerController : MonoBehaviour
{
    public PlayerState PlayerState;
    public IInteractable ObjectOnInteract;

    private InterfaceManageSystem interfaceManageSystem;
    private Rigidbody rb;
    private IMovable playerMover;
    private IRotatable playerRotator;
    private Vector2 moveDirection;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private Camera mainCamera; // Reference to the main camera

    private void Awake()
    {
        interfaceManageSystem = FindFirstObjectByType<InterfaceManageSystem>();
        mainCamera = FindFirstObjectByType<Camera>();
        rb = GetComponent<Rigidbody>();

        this.PlayerState = new PlayerState();
        playerRotator = new PlayerRotator(rotationSpeed, rotationOffset);
        playerMover = new PlayerMover(moveSpeed, playerRotator, mainCamera);
    }

    private void FixedUpdate()
    {
        if (interfaceManageSystem == null) return;
        moveDirection = interfaceManageSystem.GameJoystick.GetJoystickDirection();
        playerMover.Move(rb, moveDirection, PlayerState.IsMoving);
    }
}