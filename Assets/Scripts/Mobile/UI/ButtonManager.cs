using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class ButtonManager
{
    private readonly Dictionary<string, IButtonAction> buttonActions = new Dictionary<string, IButtonAction>();
    private readonly Dictionary<string, Button> buttonComponents = new Dictionary<string, Button>();

    // Initialize button actions for a given set of buttons and their actions
    public void InitializeButtonActions(Dictionary<string, IButtonAction> actions)
    {
        foreach (var actionPair in actions)
        {
            SetButtonAction(actionPair.Key, actionPair.Value);
        }
    }

    // Set or remove an action for a button using the button identifier
    public void SetButtonAction(string buttonIdentifier, IButtonAction action = null)
    {
        if (!buttonComponents.ContainsKey(buttonIdentifier))
        {
            // Find the button in the scene and add it to the dictionary if it doesn't exist
            Button gameButton = GameObject.Find(buttonIdentifier)?.GetComponent<Button>();

            if (gameButton != null)
            {
                buttonComponents[buttonIdentifier] = gameButton; // Add or update the button in the dictionary
            }
            else
            {
                Debug.LogWarning($"Button with identifier {buttonIdentifier} not found in the scene.");
                return;
            }
        }

        Button button = buttonComponents[buttonIdentifier];

        if (action != null)
        {
            // Assign the action to the button's onClick event
            button.onClick.RemoveAllListeners();  // Clear previous listeners
            button.onClick.AddListener(() => action.Execute());   // Add the new action
            buttonActions[buttonIdentifier] = action;
        }
        else
        {
            // If no action is provided, clear all listeners and remove from dictionary
            button.onClick.RemoveAllListeners();
            buttonActions.Remove(buttonIdentifier);
        }
    }

    // Clears all button actions and listeners
    public void ClearAllButtonActions()
    {
        foreach (var button in buttonComponents.Values)
        {
            button.onClick.RemoveAllListeners();
        }

        buttonComponents.Clear();
        buttonActions.Clear();
    }

    // Initialize buttons after a new scene has loaded
    public void OnSceneLoaded()
    {
        // Re-initialize button actions in the new scene
        foreach (var button in buttonComponents.Values)
        {
            button.gameObject.SetActive(true); // Ensure buttons are active in the new scene
        }

        Debug.Log("Scene loaded. Button actions initialized.");
    }

    // Clear all button actions after a scene is unloaded
    public void OnSceneUnloaded()
    {
        // Clear button actions when the scene is unloaded
        ClearAllButtonActions();
        Debug.Log("Scene unloaded. Button actions cleared.");
    }
}
