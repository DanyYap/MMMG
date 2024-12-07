using TMPro;
using UnityEngine;

public class TextVisualizer
{
    private TextMeshProUGUI textComponent;

    // Colors for percentage-based visualization
    private readonly Color[] percentageColors = { Color.white, Color.yellow, new Color(1f, 0.64f, 0f), Color.red }; // White, Yellow, Orange, Red
    private readonly float[] percentageThresholds = { 80f, 50f, 20f, 0f }; // Thresholds for percentage transitions

    public TextVisualizer(TextMeshProUGUI textComponent)
    {
        this.textComponent = textComponent;
    }

    /// <summary>
    /// Updates the text color based on a given percentage value.
    /// </summary>
    /// <param name="percentage">The percentage value (0 to 100).</param>
    public void SetColorByPercentage(float percentage)
    {
        // Clamp the percentage value to be within 0 to 100
        percentage = Mathf.Clamp(percentage, 0f, 100f);

        // Determine the appropriate color based on percentage thresholds
        for (int i = 0; i < percentageThresholds.Length; i++)
        {
            if (percentage >= percentageThresholds[i])
            {
                textComponent.color = percentageColors[i];
                break;
            }
        }
    }

    /// <summary>
    /// Updates the text color to either green or red based on a condition.
    /// </summary>
    /// <param name="isConditionMet">If true, sets the color to green; otherwise, sets the color to red.</param>
    public void SetColorByCondition(bool isConditionMet)
    {
        textComponent.color = isConditionMet ? Color.green : Color.red;
    }
}
