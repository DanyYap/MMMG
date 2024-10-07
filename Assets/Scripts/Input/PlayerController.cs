using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private Rigidbody rb;
    private IMovable playerMover;
    private IRotatable playerRotator;
    private System.Func<Vector2> getMoveDirection;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private int playerNumber = 1; // 1 for player 1, 2 for player 2

    private void Awake()
    {
        inputManager = PlayerInputManager.Instance;
        rb = GetComponent<Rigidbody>();

        playerRotator = new PlayerRotator(rotationSpeed, rotationOffset);
        playerMover = new PlayerMover(moveSpeed, playerRotator);

        if (playerNumber == 1)
        {
            getMoveDirection = inputManager.GetPlayer1MoveDirection;
        }
        else
        {
            getMoveDirection = inputManager.GetPlayer2MoveDirection;
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = getMoveDirection();
        playerMover.Move(rb, direction);
    }
}