using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager
{
    private readonly Dictionary<string, IButtonAction> buttonActions = new Dictionary<string, IButtonAction>();
    private readonly Dictionary<string, List<Button>> buttonComponents = new Dictionary<string, List<Button>>();

    // Initialize button actions for a given set of buttons and their actions
    public void InitializeButtonActions(Dictionary<string, IButtonAction> actions)
    {
        foreach (var actionPair in actions)
        {
            SetButtonAction(actionPair.Key, actionPair.Value);
        }
    }

    // Set or remove an action for buttons using the button identifier
    public void SetButtonAction(string buttonIdentifier, IButtonAction action = null)
    {
        // Find all buttons in the scene with the specified name, regardless of the cache
        Button[] gameButtons = GameObject.FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);
        List<Button> matchingButtons = new List<Button>();

        foreach (var button in gameButtons)
        {
            if (button.name == buttonIdentifier)
            {
                matchingButtons.Add(button);
            }
        }

        if (matchingButtons.Count > 0)
        {
            // Replace the existing cache with the new list of matching buttons
            buttonComponents[buttonIdentifier] = matchingButtons;
        }
        else
        {
            Debug.LogWarning($"No buttons with identifier '{buttonIdentifier}' found in the scene.");
            return;
        }

        // Assign the action to all matching buttons
        foreach (var button in matchingButtons)
        {
            if (action != null)
            {
                // Assign the action to each button's onClick event
                button.onClick.RemoveAllListeners();  // Clear previous listeners
                button.onClick.AddListener(() => action.Execute());   // Add the new action
            }
            else
            {
                // If no action is provided, clear all listeners
                button.onClick.RemoveAllListeners();
            }
        }

        // Update or remove the action in the dictionary
        if (action != null)
        {
            buttonActions[buttonIdentifier] = action;
        }
        else
        {
            buttonActions.Remove(buttonIdentifier);
        }
    }


    // Clears all button actions and listeners
    public void ClearAllButtonActions()
    {
        foreach (var buttonList in buttonComponents.Values)
        {
            foreach (var button in buttonList)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        buttonComponents.Clear();
        buttonActions.Clear();
    }
}
