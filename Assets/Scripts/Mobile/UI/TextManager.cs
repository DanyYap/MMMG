using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager
{
    private readonly Dictionary<string, ITextUpdate> textActions = new Dictionary<string, ITextUpdate>();
    private readonly Dictionary<string, TextMeshProUGUI> textComponents = new Dictionary<string, TextMeshProUGUI>();

    // Initialize text actions for a given set of text elements and their actions
    public void InitializeTextActions(Dictionary<string, ITextUpdate> actions)
    {
        foreach (var actionPair in actions)
        {
            SetTextAction(actionPair.Key, actionPair.Value);
        }
    }

    // Set or remove an action for a text element using the text identifier
    public void SetTextAction(string textIdentifier, ITextUpdate action = null)
    {
        if (!textComponents.ContainsKey(textIdentifier))
        {
            // Find the text element in the scene and add it to the dictionary if it doesn't exist
            TextMeshProUGUI textElement = GameObject.Find(textIdentifier)?.GetComponent<TextMeshProUGUI>();

            if (textElement != null)
            {
                textComponents[textIdentifier] = textElement; // Add or update the text element in the dictionary
            }
            else
            {
                Debug.LogWarning($"Text element with identifier {textIdentifier} not found in the scene.");
                return;
            }
        }

        TextMeshProUGUI text = textComponents[textIdentifier];

        if (action != null)
        {
            // Assign the action to the text element
            textActions[textIdentifier] = action;
        }
        else
        {
            // If no action is provided, remove the action from the dictionary
            textActions.Remove(textIdentifier);
        }
    }

    // Update the text value for a specific text element
    public void UpdateText(string textIdentifier, object value)
    {
        if (textActions.TryGetValue(textIdentifier, out var action))
        {
            action.UpdateText(value);
        }
        else
        {
            Debug.LogWarning($"No action assigned for text element with identifier {textIdentifier}.");
        }
    }

    // Clears all text actions
    public void ClearAllTextActions()
    {
        textActions.Clear();
        textComponents.Clear();
    }
}
