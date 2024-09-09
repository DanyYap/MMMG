using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum CustomPlayerInput
{
    SoloPlayer,
    MultiPlayer1,
    MultiPlayer2
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody rb;
    private Vector2 moveVector;
    [SerializeField] private CustomPlayerInput customPlayerInput;
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        input.SwitchCurrentActionMap(customPlayerInput.ToString());
    }

    private void FixedUpdate()
    {
        Move(moveVector);
    }

    public void ShowDebug(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.control.name);
        }
    }

    private void Move(Vector2 direction)
    {
        // Calculate the desired velocity
        Vector3 moveVelocity = new Vector3(direction.x, rb.linearVelocity.y, direction.y) * moveSpeed;

        // Update the Rigidbody's velocity
        rb.linearVelocity = moveVelocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }
}