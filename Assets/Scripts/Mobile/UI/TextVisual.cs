using TMPro;
using UnityEngine;

public class TextVisualizer
{
    private TextMeshProUGUI textComponent;

    // Thresholds and corresponding colors
    private readonly Color[] colorStages = { Color.white, Color.yellow, new Color(1f, 0.64f, 0f), Color.red }; // White, Yellow, Orange, Red
    private readonly float[] thresholds = { 80f, 50f, 20f, 0f }; // Percentages for color transitions

    public TextVisualizer(TextMeshProUGUI textComponent)
    {
        this.textComponent = textComponent;
    }

    /// <summary>
    /// Updates the text color based on the given percentage.
    /// </summary>
    /// <param name="percentage">The percentage value (0 to 100).</param>
    public void UpdateTextColor(float percentage)
    {
        if (percentage > 100f) percentage = 100f; // Clamp to maximum
        if (percentage < 0f) percentage = 0f;     // Clamp to minimum

        // Determine the appropriate color based on thresholds
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (percentage >= thresholds[i])
            {
                textComponent.color = colorStages[i];
                break;
            }
        }
    }
}
