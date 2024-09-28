using System;
using UnityEngine.InputSystem;

[Serializable]
public enum PlayerMode
{
    Solo,
    Multiplayer1,
    Multiplayer2
}

public class CustomPlayerMode
{
    private PlayerMode playerMode;
    private readonly PlayerInput playerInput;

    public CustomPlayerMode(PlayerInput input, PlayerMode mode)
    {
        playerInput = input;
        playerMode = mode;
        SwitchActionMap();
    }

    private void SwitchActionMap()
    {
        playerInput.SwitchCurrentActionMap(playerMode.ToString());
    }

    public void SetPlayerMode(PlayerMode mode)
    {
        playerMode = mode;
        SwitchActionMap();
    }
}