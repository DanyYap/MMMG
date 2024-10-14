using UnityEngine;
using UnityEngine.UI;

public class CanvasManageSystem : MonoBehaviour
{
    public CanvasScriptableObject canvasScriptableObject;
    private GameObject canvasInstance;
    private GameObject menuPanel;
    private GameObject inGamePanel;
    private GameObject bothPanel;
    private GameObject soloPanel;
    private GameObject multiplayerPanel;
    private bool isSolo;

    private static class PanelNames
    {
        public const string MenuPanel = "Menu Panel";
        public const string InGamePanel = "In Game Panel";
        public const string Both = "Both";
        public const string Solo = "Solo";
        public const string Multiplayer = "Multiplayer";
    }

    private static class ButtonNames
    {
        public const string SoloButton = "Solo Button";
        public const string MultiplayerButton = "Multiplayer Button";
        public const string SwitchButton = "Switch Button";
        public const string LeaveButton = "Leave Button";
    }

    void Start()
    {
        InitializeCanvas(); // Set up the canvas
        SwitchToMenuPanel(); // Show the menu panel
        AssignButtonListeners(); // Set up button actions
    }

    // Sets up the canvas and finds the panels
    private void InitializeCanvas()
    {
        if (canvasInstance == null)
        {
            canvasInstance = GameObject.FindWithTag("UI");
            if (canvasInstance == null)
            {
                canvasInstance = Instantiate(canvasScriptableObject.CanvasPrefab);
            }
            canvasInstance.SetActive(true);

            menuPanel = FindPanelByName(PanelNames.MenuPanel);
            inGamePanel = FindPanelByName(PanelNames.InGamePanel);

            bothPanel = FindChildPanelByName(inGamePanel, PanelNames.Both);
            soloPanel = FindChildPanelByName(inGamePanel, PanelNames.Solo);
            multiplayerPanel = FindChildPanelByName(inGamePanel, PanelNames.Multiplayer);
        }
    }

    // Finds a panel by its name
    private GameObject FindPanelByName(string panelName)
    {
        Transform panelTransform = canvasInstance.transform.Find(panelName);
        if (panelTransform != null)
        {
            return panelTransform.gameObject;
        }
        Debug.LogWarning($"Panel '{panelName}' not found.");
        return null;
    }

    // Finds a child panel by its name
    private GameObject FindChildPanelByName(GameObject parentPanel, string childPanelName)
    {
        if (parentPanel == null)
        {
            Debug.LogWarning("Parent panel is null.");
            return null;
        }

        Transform childTransform = parentPanel.transform.Find(childPanelName);
        if (childTransform != null)
        {
            return childTransform.gameObject;
        }
        Debug.LogWarning($"Child panel '{childPanelName}' not found in '{parentPanel.name}'.");
        return null;
    }

    // Sets a panel and its children to be active or inactive
    private void SetPanelActive(GameObject panel, bool isActive)
    {
        panel.SetActive(isActive);
        foreach (Transform child in panel.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }

    // Switches between two panels
    private void SwitchPanel(GameObject enablePanel, GameObject disablePanel)
    {
        SetPanelActive(enablePanel, true);
        SetPanelActive(disablePanel, false);
    }

    // Switches to the in-game panel
    public void SwitchToInGamePanel()
    {
        InitializeCanvas();
        SwitchPanel(inGamePanel, menuPanel);
        SetPanelActive(bothPanel, true);

        if (isSolo)
        {
            SetPanelActive(soloPanel, true);
            SetPanelActive(multiplayerPanel, false);
        }
        else
        {
            SetPanelActive(multiplayerPanel, true);
            SetPanelActive(soloPanel, false);
        }
    }

    // Switches to the menu panel
    public void SwitchToMenuPanel()
    {
        InitializeCanvas();
        SwitchPanel(menuPanel, inGamePanel);
    }

    // Sets the game mode to solo or multiplayer
    public void OnSetSoloButtonClick(bool isSolo)
    {
        this.isSolo = isSolo;
    }

    // Sets up the actions for each button
    public void AssignButtonListeners()
    {
        InitializeCanvas();
        Button[] buttons = canvasInstance.GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            if (button == null)
            {
                Debug.LogWarning("Button is null. Skipping.");
                continue;
            }

            button.onClick.RemoveAllListeners();

            switch (button.name)
            {
                case ButtonNames.SoloButton:
                    button.onClick.AddListener(() => OnSetSoloButtonClick(true));
                    button.onClick.AddListener(() => GetComponent<SceneManageSystem>().OnLoadSceneButtonClick("GameScene"));
                    break;
                case ButtonNames.MultiplayerButton:
                    button.onClick.AddListener(() => OnSetSoloButtonClick(false));
                    button.onClick.AddListener(() => GetComponent<SceneManageSystem>().OnLoadSceneButtonClick("GameScene"));
                    break;
                case ButtonNames.SwitchButton:
                    button.onClick.AddListener(() => GetComponent<PlayerControlSystem>().OnSwitchPlayerButtonClick());
                    break;
                case ButtonNames.LeaveButton:
                    button.onClick.AddListener(() => GetComponent<SceneManageSystem>().OnLoadSceneButtonClick("MenuScene"));
                    break;
                    // Add more cases for other buttons as needed
            }
        }
    }
}
