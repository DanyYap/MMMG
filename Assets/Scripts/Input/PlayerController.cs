using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // input
    private PlayerInputManager inputManager;
    [SerializeField] private PlayerMode customPlayerMode;
    
    // player movement
    private PlayerMover playerMover;
    private Rigidbody rb;
    private Vector2 moveVector;
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        inputManager = PlayerInputManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerMover = new PlayerMover(moveSpeed); 
    }

    private void FixedUpdate()
    {
        Move(moveVector);
    }

    private void Move(Vector2 moveVector)
    {
        if (inputManager.IsMobile())
        {            
            playerMover.MoveWithRigidbody(rb, inputManager.GetJoystickDirection());
        }
        else
        {
            playerMover.MoveWithRigidbody(rb, moveVector);
        }
    }

    public void GetMoveVector(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }
}