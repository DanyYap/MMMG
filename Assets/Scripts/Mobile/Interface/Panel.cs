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
    bool IsDestroyed(); // New method to check if the panel is destroyed
}

// Implement the IsDestroyed method in the panel classes
public class MainMenuPanel : IPanel
{
    private GameObject panel;

    public MainMenuPanel(GameObject panel)
    {
        this.panel = panel;
    }

    public void Show() { panel.SetActive(true); }
    public void Hide() { panel.SetActive(false); }
    public bool IsDestroyed() { return panel == null; }
}

public class InGamePanel : IPanel
{
    private GameObject panel;

    public InGamePanel(GameObject panel)
    {
        this.panel = panel;
    }

    public void Show() { panel.SetActive(true); }
    public void Hide() { panel.SetActive(false); }
    public bool IsDestroyed() { return panel == null; }
}