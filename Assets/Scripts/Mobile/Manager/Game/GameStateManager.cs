using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,           // Default state
    MainMenu,       // Main menu
    GameStart,      // Game starting
    Playing,        // Game in progress
    Paused,         // Game paused
    GameEnd,        // Game ended (win/lose)
}

public class GameStateManager
{
    private GameState currentState = GameState.None;
    public GameState CurrentState => currentState;

    private readonly Dictionary<GameState, Action> stateEnterActions = new();
    private readonly Dictionary<GameState, Action> stateExitActions = new();

    private event Action<GameState> OnStateEnter;
    private event Action<GameState> OnStateExit;

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"Changing state from {currentState} to {newState}");

        // Trigger exit actions
        if (stateExitActions.TryGetValue(currentState, out var exitAction))
        {
            Debug.Log($"Invoking Exit Actions for state: {currentState}");
            exitAction?.Invoke();
        }
        else
        {
            //Debug.Log($"No Exit Actions found for state: {currentState}");
        }

        OnStateExit?.Invoke(currentState);

        // Update the state
        currentState = newState;

        // Trigger enter actions
        if (stateEnterActions.TryGetValue(newState, out var enterAction))
        {
            Debug.Log($"Invoking Enter Actions for state: {newState}");
            enterAction?.Invoke();
        }
        else
        {
            //Debug.Log($"No Enter Actions found for state: {newState}");
        }

        OnStateEnter?.Invoke(newState);

        Debug.Log($"State changed successfully. Current state is now: {currentState}");
    }

    public void AddEnterAction(GameState state, Action action)
    {
        if (!stateEnterActions.ContainsKey(state))
        {
            stateEnterActions[state] = null;
        }
        stateEnterActions[state] += action;
        //Debug.Log($"Added Enter Action for state: {state}");
    }

    public void AddExitAction(GameState state, Action action)
    {
        if (!stateExitActions.ContainsKey(state))
        {
            stateExitActions[state] = null;
        }
        stateExitActions[state] += action;
        //Debug.Log($"Added Exit Action for state: {state}");
    }
}

