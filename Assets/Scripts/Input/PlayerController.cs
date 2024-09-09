using System;
using System.Collections.Generic;
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
    [SerializeField] private CustomPlayerInput customPlayerInput;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap(customPlayerInput.ToString());
    }

    public void ShowDebug(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.control.name);
        }
    }
}
