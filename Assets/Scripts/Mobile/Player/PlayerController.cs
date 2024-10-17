using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

        playerRotator = new PlayerRotator(rotationSpeed, rotationOffset);
        playerMover = new PlayerMover(moveSpeed, playerRotator);
    }

    private void FixedUpdate()
    {
        if (interfaceManageSystem == null) return;
        moveDirection = interfaceManageSystem.GameJoystick.GetJoystickDirection();
        playerMover.Move(rb, moveDirection);
    }
}