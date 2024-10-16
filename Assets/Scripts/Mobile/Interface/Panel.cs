using UnityEngine;

public static class PanelIdentifiers
{
    // Main panels
    public static readonly string MainMenu = "Main Menu";
    public static readonly string Lobby = "Lobby";
    public static readonly string InGame = "In Game";

    // Sub panels for InGame
    public static readonly string InGameBoth = "Both";
    public static readonly string InGameSolo = "Solo";
    public static readonly string InGameMultiplayer = "Multiplayer";
}

// Interface for panels
public interface IPanel
{
    void Show();
    void Hide();
}

// Concrete panel implementations
public class MainMenuPanel : IPanel
{
    private GameObject panel;

    public MainMenuPanel(GameObject panel)
    {
        this.panel = panel;
    }

    public void Show() => panel.SetActive(true);
    public void Hide() => panel.SetActive(false);
}

public class LobbyPanel : IPanel
{
    private GameObject panel;

    public LobbyPanel(GameObject panel)
    {
        this.panel = panel;
    }

    public void Show() => panel.SetActive(true);
    public void Hide() => panel.SetActive(false);
}

public class InGamePanel : IPanel
{
    private GameObject panel;

    public InGamePanel(GameObject panel)
    {
        this.panel = panel;
    }

    public void Show() => panel.SetActive(true);
    public void Hide() => panel.SetActive(false);
}