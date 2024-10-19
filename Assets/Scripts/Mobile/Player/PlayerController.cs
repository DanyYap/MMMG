using System;
using UnityEngine;

// TODO: need to modify so can have multiple player states at the same time
public class PlayerState
{
    public bool IsMoving    { get; set; }
    public bool IsGrabbing  { get; set; }

    public PlayerState()
    {
        IsMoving = false;
        IsGrabbing = false;
    }

    public void SetState(Action<bool> stateSetter, bool flag)
    {
        stateSetter(flag);
    }
}

public class PlayerController : MonoBehaviour
{
    public PlayerState PlayerState; 

    private InterfaceManageSystem interfaceManageSystem;
    private Rigidbody rb;
    private IMovable playerMover;
    private IRotatable playerRotator;
    private Vector2 moveDirection;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    private void Awake()
    {
        interfaceManageSystem = FindFirstObjectByType<InterfaceManageSystem>();
        rb = GetComponent<Rigidbody>();

        this.PlayerState = new PlayerState();
        playerRotator = new PlayerRotator(rotationSpeed, rotationOffset);
        playerMover = new PlayerMover(moveSpeed, playerRotator);
    }

    private void FixedUpdate()
    {
        if (interfaceManageSystem == null) return;
        moveDirection = interfaceManageSystem.GameJoystick.GetJoystickDirection();
        playerMover.Move(rb, moveDirection, PlayerState.IsMoving);
    }
}