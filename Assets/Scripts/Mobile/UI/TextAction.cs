using TMPro;
using UnityEngine;

// Interface for text components
public interface ITextUpdate
{
    void UpdateText(float value); // Accept a float value
}


public class FireHealthLeft : ITextUpdate
{
    private TextMeshProUGUI textComponent;
    private TextVisualizer textVisualizer;
    private float maxValue; // Stores the maximum value
    private bool isMaxValueSet = false; // Tracks if the max value has been set

    public FireHealthLeft()
    {
        var fireHealthLeftText = UiElementNames.Texts.FireHealthLeftText;
        textComponent = GameObject.Find(fireHealthLeftText)?.GetComponent<TextMeshProUGUI>();
        if (textComponent == null) return;
    }

    public void UpdateText(float value)
    {
        if (textComponent == null) return;

        // Set the max value if it hasn't been set yet
        if (!isMaxValueSet)
        {
            maxValue = value;
            isMaxValueSet = true;
        }

        // Calculate the percentage remaining
        float percentage = (value / maxValue) * 100f;

        // Update the text to display the percentage remaining
        textComponent.text = $"Fire Left:\n {percentage:F1}%";
    }
}

public class Countdown : ITextUpdate
{
    private TextMeshProUGUI textComponent;
    private TextVisualizer textVisualizer;
    private CountdownTimer countdownTimer;

    public Countdown(CountdownTimer timer)
    {
        var countdownText = UiElementNames.Texts.CountdownText;
        textComponent = GameObject.Find(countdownText)?.GetComponent<TextMeshProUGUI>();
        if (textComponent == null) return;

        textVisualizer = new TextVisualizer(textComponent);
        countdownTimer = timer;

        // Subscribe to countdown events
    }

    public void UpdateText(float value)
    {
        if (textComponent == null) return;

        textComponent.text = $"Time Left:\n {value:F1}";
        textVisualizer.UpdateTextColor(value);
    }
}