using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager
{
    private readonly Canvas canvas;  // Reference to the canvas that holds the panels
    private readonly GameObject[] panels;  // Array to hold all the panel objects

    public PanelManager(Canvas canvas)
    {
        this.canvas = canvas;

        // Collect all the panels based on the names defined in UiElementNames.Panels
        panels = new GameObject[]
        {
            canvas.transform.Find(UiElementNames.Panels.MainMenu)?.gameObject,
            canvas.transform.Find(UiElementNames.Panels.InGame)?.gameObject,
            canvas.transform.Find(UiElementNames.Panels.WinResult)?.gameObject,
            canvas.transform.Find(UiElementNames.Panels.LoseResult)?.gameObject,
            // Add more panels here as needed
        };

        // Register for scene load/unload events
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Initialize panels when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure all panels are initialized in the new scene
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(true); // Activate panels if needed (if they are active in this scene)
            }
        }

        Debug.Log($"Scene {scene.name} loaded. Panels initialized.");
    }

    // Clear panels when a scene is unloaded
    private void OnSceneUnloaded(Scene scene)
    {
        // Deactivate and clear all panel references when the scene is unloaded
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(false); // Deactivate the panel
            }
        }

        Debug.Log($"Scene {scene.name} unloaded. Panels cleared.");
    }

    // Show the specified panel by its name
    public void ShowPanel(string panelName)
    {
        foreach (var panel in panels)
        {
            if (panel != null && panel.name == panelName)
            {
                panel.SetActive(true);  // Activate the panel
            }
            else if (panel != null)
            {
                panel.SetActive(false);  // Deactivate the other panels
            }
        }
    }

    // Hide the specified panel by its name
    public void HidePanel(string panelName)
    {
        foreach (var panel in panels)
        {
            if (panel != null && panel.name == panelName)
            {
                panel.SetActive(false);  // Deactivate the panel
            }
        }
    }

    // Toggle the visibility of a panel
    public void TogglePanel(string panelName)
    {
        foreach (var panel in panels)
        {
            if (panel != null && panel.name == panelName)
            {
                panel.SetActive(!panel.activeSelf);  // Toggle the active state
            }
        }
    }

    // Hide all panels (useful for clearing the screen or when transitioning between scenes)
    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(false);  // Deactivate all panels
            }
        }
    }

    // Unregister from scene loaded and unloaded events
    public void Cleanup()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
