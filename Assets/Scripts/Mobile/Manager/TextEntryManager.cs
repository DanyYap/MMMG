using System.Collections.Generic;
using TMPro;

// Class to hold a text entry
public class TextEntry
{
    public ITextComponent TextComponent { get; }
    public TextType Type { get; }
    public string TextName { get; }

    public TextEntry(ITextComponent textComponent, TextType type, string textName)
    {
        TextComponent = textComponent;
        Type = type;
        TextName = textName; 
    }

    public void Reinitialize(TextMeshProUGUI newTextComponent)
    {
        TextComponent.InitializeText(newTextComponent);
    }
}

// Class to hold and manage text entries
public class TextEntryManager
{
    private readonly List<TextEntry> textEntries = new List<TextEntry>();
    private readonly ITextComponentFactory textComponentFactory;

    // Constructor for dependency injection
    public TextEntryManager(ITextComponentFactory factory)
    {
        textComponentFactory = factory;
    }

    // Add a new text entry
    public void AddTextEntry(TextMeshProUGUI textComponent, TextType textType, string textName)
    {
        ITextComponent newTextComponent = textComponentFactory.CreateTextComponent(textComponent, textType);
        textEntries.Add(new TextEntry(newTextComponent, textType, textName));
    }

    // Reinitialize an existing text entry
    public void ReinitializeTextEntry(TextEntry textEntry, TextMeshProUGUI newTextComponent)
    {
        textEntry.Reinitialize(newTextComponent);
    }

    // Try to find an existing text entry by type and name
    public bool TryGetTextEntry(TextType textType, string textName, out TextEntry textEntry)
    {
        textEntry = textEntries.Find(entry => entry.Type == textType && entry.TextName == textName);
        return textEntry != null;
    }

    // Get all text entries
    public List<TextEntry> GetAllTextEntries()
    {
        return new List<TextEntry>(textEntries);
    }
}