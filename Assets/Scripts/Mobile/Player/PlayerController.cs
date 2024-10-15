using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MobileInputSystem inputSystem;
    private Rigidbody rb;
    private IMovable playerMover;
    private IRotatable playerRotator;
    private Vector2 moveDirection;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    private void Awake()
    {
        inputSystem = MobileInputSystem.Instance;
        rb = GetComponent<Rigidbody>();

        playerRotator = new PlayerRotator(rotationSpeed, rotationOffset);
        playerMover = new PlayerMover(moveSpeed, playerRotator);
    }

    private void FixedUpdate()
    {
        moveDirection = inputSystem.GetDirection();
        playerMover.Move(rb, moveDirection);
    }
}