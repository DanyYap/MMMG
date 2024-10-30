using System.Collections.Generic;
using UnityEngine;

public interface IPanelManager
{
    void InitializePanels();
    void SwitchToPanel(string panelId);
}

public class PanelManager : IPanelManager
{
    private readonly Dictionary<string, IPanel> panels = new();
    private IPanel currentPanel;

    public void InitializePanels()
    {
        var canvasLibrary = ScriptableObjectManageSystem.Instance.CanvasLibrary;
        var canvasPrefab = GameObject.Find(canvasLibrary.CanvasPrefab.name)
                                   ?? Object.Instantiate(canvasLibrary.CanvasPrefab);

        panels[PanelIdentifiers.MainMenu] = PanelFactory.CreatePanel(
            PanelIdentifiers.MainMenu, FindPanelByName(canvasPrefab.transform, PanelIdentifiers.MainMenu));
        panels[PanelIdentifiers.InGame] = PanelFactory.CreatePanel(
            PanelIdentifiers.InGame, FindPanelByName(canvasPrefab.transform, PanelIdentifiers.InGame));
    }

    public void SwitchToPanel(string panelId)
    {
        // Safely handle the current panel
        if (currentPanel != null)
        {
            if (IsPanelDestroyed(currentPanel) || !IsPanelInCurrentScene(currentPanel))
            {
                Debug.LogWarning($"Current panel '{panelId}' is no longer valid. Clearing reference.");
                currentPanel = null;
                InitializePanels();
            }
            else
            {
                currentPanel.Hide(); // Only hide if the panel is valid
            }
        }

        // Switch to the new panel
        if (panels.TryGetValue(panelId, out var newPanel) && !IsPanelDestroyed(newPanel))
        {
            currentPanel = newPanel;
            currentPanel.Show(); // Show the new panel safely
        }
        else
        {
            Debug.LogError($"Panel '{panelId}' not found or has been destroyed.");
        }
    }

    private bool IsPanelDestroyed(IPanel panel)
    {
        var panelObject = (panel as BasePanel)?.PanelObject;
        // Check if the object is null or destroyed
        return panelObject == null || !panelObject;
    }

    private bool IsPanelInCurrentScene(IPanel panel)
    {
        var panelObject = (panel as BasePanel)?.PanelObject;
        return panelObject != null && panelObject.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    private GameObject FindPanelByName(Transform parent, string panelName)
    {
        var panelTransform = parent.Find(panelName);
        if (panelTransform == null)
        {
            Debug.LogError($"Panel '{panelName}' not found.");
            return null;
        }
        return panelTransform.gameObject;
    }
}