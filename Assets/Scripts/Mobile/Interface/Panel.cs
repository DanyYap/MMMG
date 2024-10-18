using UnityEngine;

public static class PanelIdentifiers
{
    // Main panels
    public static readonly string MainMenu      = "Main Menu";
    public static readonly string PauseMenu     = "Pause Menu";
    public static readonly string InGame        = "In Game";

    // Sub panels for InGame
    public static readonly string InGameBoth    = "Both";
    public static readonly string InGameSolo    = "Solo";
    public static readonly string InGameCoop    = "Coop";
}

// Interface for panels
public interface IPanel
{
    void Show();
    void Hide();
    bool IsDestroyed(); // To check if the panel is destroyed
}

// Abstract class for panel behavior
public abstract class BasePanel : IPanel
{
    protected GameObject panelObject;

    public BasePanel(GameObject panelObject)
    {
        this.panelObject = panelObject;
    }

    public virtual void Show() => panelObject.SetActive(true);
    public virtual void Hide() => panelObject.SetActive(false);
    public virtual bool IsDestroyed() => panelObject == null;
}

// Concrete panel classes
public class MainMenuPanel : BasePanel
{
    public MainMenuPanel(GameObject panelObject) : base(panelObject) { }
}

public class InGamePanel : BasePanel
{
    public InGamePanel(GameObject panelObject) : base(panelObject) { }
}

// Factory for creating panels
public class PanelFactory
{
    public static IPanel CreatePanel(string panelName, GameObject panelObject)
    {
        if (panelName == PanelIdentifiers.MainMenu)
        {
            return new MainMenuPanel(panelObject);
        }
        else if (panelName == PanelIdentifiers.InGame)
        {
            return new InGamePanel(panelObject);
        }
        return null;
    }
}