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
    private Vector2 moveDirection;
    private IMovable playerMover;
    private IRotatable playerRotator;
    private IAnimatable playerAnimator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private Camera mainCamera; // Reference to the main camera

    public Transform playerGrabPoint;

    private void Awake()
    {
        interfaceManageSystem = FindFirstObjectByType<InterfaceManageSystem>();
        mainCamera = FindFirstObjectByType<Camera>();
        rb = GetComponent<Rigidbody>();

        // movement & rotation
        this.PlayerState = new PlayerState();
        playerRotator = new PlayerRotator(rotationSpeed, rotationOffset);
        playerMover = new PlayerMover(moveSpeed, playerRotator, mainCamera);

        // animation
        var animator = GetComponent<Animator>();
        var library = FactoryManageSystem.Instance.AnimationLibrary;
        playerAnimator = AnimatableFactory.CreateAnimator(animator, library, EntityType.Character);
        if (playerAnimator is CharacterAnimator characterAnimator)
        {
            characterAnimator.InitializePlayerState(PlayerState);
        }

        // grabbing
        if (playerGrabPoint == null)
        {
            playerGrabPoint = gameObject.transform;
        }
    }

    private void Update()
    {
        playerAnimator.PlayIdleOrRun();
    }

    private void FixedUpdate()
    {
        if (interfaceManageSystem == null) return;
        moveDirection = interfaceManageSystem.GetInputManager().GetJoystickDirection();

        PlayerState.IsMoving = moveDirection != Vector2.zero;
        playerMover.Move(rb, moveDirection);
    }
}