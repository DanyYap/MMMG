using TMPro;

// Enum for text types
public enum TextType
{
    TimerText,
    // Additional text types here
}

// Static class for text names
public static class TextNames
{
    public const string TimerText = "Timer Text";
}

// Interface for text components
public interface ITextComponent
{
    void InitializeText(TextMeshProUGUI text);
    void UpdateText(object value); // Accept any type
}

// Abstract class for game text components
public abstract class GameText : ITextComponent
{
    protected TextMeshProUGUI textComponent;

    public void InitializeText(TextMeshProUGUI text)
    {
        textComponent = text;
    }

    public abstract void UpdateText(object value);
}

// Timer text implementation
public class TimerText : GameText
{
    public TimerText(TextMeshProUGUI text)
    {
        InitializeText(text);
    }

    public override void UpdateText(object value)
    {
        if (value is int intValue)
        {
            textComponent.text = $"Saving: {intValue}%";
        }
        else if (value is float floatValue)
        {
            textComponent.text = $"Saving: {floatValue:F1}%"; // Format float to one decimal place
        }
        else if (value is string stringValue)
        {
            textComponent.text = stringValue;
        }
        else
        {
            textComponent.text = "Invalid value type"; // Handle unexpected types
        }
    }
}

// Factory interface for creating text components
public interface ITextComponentFactory
{
    ITextComponent CreateTextComponent(TextMeshProUGUI textComponent, TextType textType);
}

// Factory implementation for creating text components
public class TextComponentFactory : ITextComponentFactory
{
    public ITextComponent CreateTextComponent(TextMeshProUGUI textComponent, TextType textType)
    {
        return textType switch
        {
            TextType.TimerText => new TimerText(textComponent),
            _ => throw new System.NotImplementedException($"TextType {textType} not implemented")
        };
    }
}