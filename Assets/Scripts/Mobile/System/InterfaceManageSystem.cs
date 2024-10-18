using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

// Manager for handling UI transitions and button actions
public class InterfaceManageSystem : MonoBehaviour
{
    public static InterfaceManageSystem Instance;

    public CanvasScriptableObject canvasFactory;
    private GameObject canvasPrefab;

    private Dictionary<string, IPanel> panels;
    private IPanel currentPanel;

    private GameJoystick gameJoystick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePanels();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SwitchToPanel(PanelIdentifiers.MainMenu);
    }

    private void InitializePanels()
    {
        string canvasName = canvasFactory.CanvasPrefab.name;
        canvasPrefab = GameObject.Find(canvasName);

        // Check if the canvas prefab still exists in the scene
        if (canvasPrefab == null)
        {
            canvasPrefab = Instantiate(canvasFactory.CanvasPrefab);
        }

        // Ensure that we create panels even if they were previously destroyed
        var menuPanel = FindPanelByName(canvasPrefab.transform, PanelIdentifiers.MainMenu);
        var gamePanel = FindPanelByName(canvasPrefab.transform, PanelIdentifiers.InGame);

        if (panels == null)
        {
            panels = new Dictionary<string, IPanel>();
        }

        panels[PanelIdentifiers.MainMenu] = PanelFactory.CreatePanel(PanelIdentifiers.MainMenu, menuPanel);
        panels[PanelIdentifiers.InGame] = PanelFactory.CreatePanel(PanelIdentifiers.InGame, gamePanel);
    }

    public void SwitchToPanel(string panelId)
    {
        // Check if the current panel still exists
        if (currentPanel != null && currentPanel.IsDestroyed())
        {
            currentPanel = null; // Reset currentPanel if it's destroyed
            Debug.LogWarning("Current panel has been destroyed. Reinitializing panels.");
            InitializePanels(); // Reinitialize panels
        }

        currentPanel?.Hide(); // Hide the current panel if it exists
        if (panels.TryGetValue(panelId, out currentPanel))
        {
            currentPanel.Show(); // Show the new panel
        }
        else
        {
            Debug.LogError($"Panel '{panelId}' not found.");
        }

        SetupJoystickAction();
        SetupButtonActions();
    }

    private void SetupJoystickAction()
    {
        FixedJoystick joystick = FindFirstObjectByType<FixedJoystick>();
        gameJoystick = new GameJoystick(joystick);
    }

    public GameJoystick GameJoystick => gameJoystick;

    private void SetupButtonActions()
    {
        ISceneController sceneController = GetComponent<ISceneController>();
        PlayerManageSystem.Instance.InitializePlayers();

        // Setup action for the button
        void SetupButton(string buttonIdentifier, UnityAction action)
        {
            Button button = GameObject.Find(buttonIdentifier)?.GetComponent<Button>();
            if (button != null)
            {
                new GameButton(button, action);
            }
        }

        // Menu buttons
        SetupButton(ButtonIdentifiers.SoloGameButton, () => new StartGameAction(sceneController).Execute());

        // Game buttons
        SetupButton(ButtonIdentifiers.PlayerSwitchButton, () => 
        new SwitchPlayerAction(PlayerManageSystem.Instance.GetPlayerSwitcher()).Execute());
        SetupButton(ButtonIdentifiers.BackToMenuButton, () => new BackMenuAction(sceneController).Execute());
    }

    // Finds a panel or a sub panel by its name within the provided parent transform
    private GameObject FindPanelByName(Transform parentTransform, string panelName, string subPanelName = null)
    {
        if (parentTransform == null)
        {
            Debug.LogError("Parent transform is null. Ensure it is properly set up.");
            return null;
        }

        Transform panelTransform = parentTransform.Find(panelName);
        if (panelTransform == null)
        {
            Debug.LogError($"Panel with name '{panelName}' not found.");
            return null;
        }

        Transform targetTransform = subPanelName != null ? panelTransform.Find(subPanelName) : panelTransform;
        if (targetTransform == null)
        {
            Debug.LogError($"Sub panel with name '{subPanelName}' not found under '{panelName}'.");
            return null;
        }

        return targetTransform.gameObject;
    }
}