using TMPro;
using UnityEngine;

// Interface for text components
public interface ITextAction
{
    void UpdateText(object value); // Accept any type
}

public class CountdownAction : ITextAction
{
    private TextMeshProUGUI textComponent;
    private TextVisualizer textVisualizer;

    public CountdownAction()
    {
        var countdownText = UiElementNames.Texts.CountdownText;
        textComponent = GameObject.Find(countdownText)?.GetComponent<TextMeshProUGUI>();
        if (textComponent == null) return;

        textVisualizer = new TextVisualizer(textComponent);
    }

    public void UpdateText(object value)
    {
        if (textComponent == null) return;

        if (value is float floatValue)
        {
            textComponent.text = $"Saving: {floatValue:F1}%"; // Format float to one decimal place

            textVisualizer.UpdateTextColor(floatValue);
        }
        else
        {
            return;
        }
    }

}