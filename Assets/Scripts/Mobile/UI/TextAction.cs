using TMPro;
using UnityEngine;

// Interface for text components
public interface ITextUpdate
{
    void UpdateText(object value); // Accept a float value
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

    public void UpdateText(object value)
    {
        if (textComponent == null) return;

        if (value is float floatValue)
        {
            // Set the max value if it hasn't been set yet
            if (!isMaxValueSet)
            {
                maxValue = floatValue;
                isMaxValueSet = true;
            }

            // Calculate the percentage remaining
            float percentage = (floatValue / maxValue) * 100f;

            // Update the text to display the percentage remaining
            textComponent.text = $"Fire Left:\n {percentage:F1}%";
        }
        else return;
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

    public void UpdateText(object value)
    {
        if (textComponent == null) return;

        if (value is float floatValue)
        {
            textComponent.text = $"Time Left:\n {floatValue:F1}";
            textVisualizer.SetColorByPercentage(floatValue);
        }
        else return;
    }
}

public class Result : ITextUpdate
{
    private TextMeshProUGUI textComponent;
    private TextVisualizer textVisualizer;

    public Result()
    {
        var resultText = UiElementNames.Texts.ResultText;
        textComponent = GameObject.Find(resultText)?.GetComponent<TextMeshProUGUI>();
        if (textComponent == null) return;

        textVisualizer = new TextVisualizer(textComponent);
    }

    public void UpdateText(object value)
    {
        if (textComponent == null) return;

        if (value is bool boolValue)
        {
            if (boolValue)
            {
                textComponent.text = $"MISSION \nSUCCESS";
            }
            else
            {
                textComponent.text = $"MISSION \nFAILED";
            }

            textVisualizer.SetColorByCondition(boolValue);
        }
        else return;
    }
}