using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private Rigidbody rb;
    private IMovable playerMover;

    [SerializeField] private PlayerMode playerMode; // Keeping PlayerMode here
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        inputManager = PlayerInputManager.Instance;
        rb = GetComponent<Rigidbody>();
        playerMover = new PlayerMover(moveSpeed);
    }

    private void FixedUpdate()
    {
        Vector2 direction = inputManager.GetMoveDirection();
        playerMover.Move(rb, direction);
    }
}