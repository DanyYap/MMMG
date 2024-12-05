using TMPro;
using UnityEngine;

// Interface for text management
public interface ITextManager
{
    void SetupTexts();
    void UpdateText(TextType textType, string textName, object value);
}

// Class to manage text entries
public class TextManager : ITextManager
{
    private readonly TextEntryManager textEntryManager;

    // Constructor for dependency injection
    public TextManager(ITextComponentFactory factory)
    {
        textEntryManager = new TextEntryManager(factory);
    }

    // Initialize texts
    public void SetupTexts()
    {
        CreateOrUpdateText(TextNames.FireHealths, TextType.TimerText);
    }

    // UpdateFire text value
    public void UpdateText(TextType textType, string textName, object value)
    {
        if (textEntryManager.TryGetTextEntry(textType, textName, out var entry))
        {
            entry.TextComponent.UpdateText(value);
        }
    }

    // Create or update text entry
    private void CreateOrUpdateText(string textName, TextType textType)
    {
        var existingComponent = FindTextComponent(textName);
        if (existingComponent == null) return;

        if (textEntryManager.TryGetTextEntry(textType, textName, out var existingEntry))
        {
            ReinitializeIfInvalid(existingEntry);
        }
        else
        {
            textEntryManager.AddTextEntry(existingComponent, textType, textName);
        }
    }

    // Reinitialize if invalid
    private void ReinitializeIfInvalid(TextEntry textEntry)
    {
        var textComponent = FindTextComponent(textEntry.TextName);

        if (textComponent != null)
        {
            textEntryManager.ReinitializeTextEntry(textEntry, textComponent);
        }
    }

    // Find component by name
    private TextMeshProUGUI FindTextComponent(string textName)
    {
        return GameObject.Find(textName)?.GetComponent<TextMeshProUGUI>();
    }
}