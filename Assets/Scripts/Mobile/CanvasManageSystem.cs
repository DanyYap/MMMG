using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CanvasManageSystem : MonoBehaviour
{
    public CanvasScriptableObject canvasPrefab;

    private GameObject canvasInstance;
    private List<GameObject> mainPanels;
    private Dictionary<string, List<GameObject>> subPanels;
    private bool isGameModeSolo;
    private Dictionary<string, Button> buttonMap;
    private List<TMP_Text> textMeshProTexts;

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

    private static class ButtonIdentifiers
    {
        // Main menu buttons
        public const string SoloGameButton = "Solo Game Button";
        public const string MultiplayerButton = "Multiplayer Button";

        // Lobby buttons
        public const string CreateLobbyButton = "Create Lobby Button";
        public const string JoinLobbyButton = "Join Lobby Button";
        public const string ExitLobbyButton = "Exit Lobby Button";
        public const string BackToMenuButton = "Back To Menu Button";

        // In-game buttons
        public const string ExitGameButton = "Exit Game Button";
        public const string PlayerSwitchButton = "Player Switch Button";
    }

    private static class TextIdentifiers
    {
        // menu texts
        public const string MenuText = "Menu Text";

        // lobby texts
        public const string LobbyText = "Lobby Text";
    }

    void Start()
    {
        SwitchToPanel(PanelIdentifiers.MainMenu);
    }

    // Switches to a specific panel and disables all other panels
    public void SwitchToPanel(string panelName)
    {
        InitializeAndManageCanvas();

        GameObject targetPanel = mainPanels.Find(p => p != null && p.name == panelName);
        if (targetPanel == null)
        {
            Debug.LogError($"Panel with name '{panelName}' not found.");
        }

        foreach (GameObject panel in mainPanels)
        {
            SetPanelActive(panel, panel == targetPanel);
        }

        if (panelName == PanelIdentifiers.InGame)
        {
            SetPanelActive(subPanels[PanelIdentifiers.InGame].Find(c => c.name == PanelIdentifiers.InGameBoth), true);

            string activeSubPanelName = isGameModeSolo ? PanelIdentifiers.InGameSolo : PanelIdentifiers.InGameMultiplayer;
            string inactiveSubPanelName = isGameModeSolo ? PanelIdentifiers.InGameMultiplayer : PanelIdentifiers.InGameSolo;

            SetPanelActive(subPanels[PanelIdentifiers.InGame].Find(c => c.name == activeSubPanelName), true);
            SetPanelActive(subPanels[PanelIdentifiers.InGame].Find(c => c.name == inactiveSubPanelName), false);
        }
    }

    // Initializes and manages the canvas instance, main panels, and sub panels
    private void InitializeAndManageCanvas()
    {
        InitializeCanvasInstance();
        InitializeMainPanelsAndSubPanels();
        InitializeTextMeshProTexts();
        AssignButtonActions();
    }

    private void InitializeCanvasInstance()
    {
        if (canvasInstance == null)
        {
            canvasInstance = GameObject.FindWithTag("UI");
            if (canvasInstance == null)
            {
                canvasInstance = Instantiate(canvasPrefab.CanvasPrefab);
            }
            canvasInstance.SetActive(true);
        }
    }

    private void InitializeMainPanelsAndSubPanels()
    {
        mainPanels = new List<GameObject>
        {
            FindPanelByName(PanelIdentifiers.MainMenu),
            FindPanelByName(PanelIdentifiers.Lobby),
            FindPanelByName(PanelIdentifiers.InGame)
        };
        mainPanels.RemoveAll(p => p == null);

        subPanels = new Dictionary<string, List<GameObject>>();

        List<GameObject> inGameSubPanels = new()
        {
            FindPanelByName(PanelIdentifiers.InGame, PanelIdentifiers.InGameBoth),
            FindPanelByName(PanelIdentifiers.InGame, PanelIdentifiers.InGameSolo),
            FindPanelByName(PanelIdentifiers.InGame, PanelIdentifiers.InGameMultiplayer),
        };
        inGameSubPanels.RemoveAll(p => p == null); // Remove null game objects from the list
        subPanels.Add(PanelIdentifiers.InGame, inGameSubPanels);
    }

    // Function to get specific TextMesh Pro Text components from canvasInstance based on TextIdentifiers
    private void InitializeTextMeshProTexts()
    {
        textMeshProTexts = new List<TMP_Text>();
        TMP_Text[] allTextMeshProTexts = canvasInstance.GetComponentsInChildren<TMP_Text>(true);

        foreach (TMP_Text text in allTextMeshProTexts)
        {
            if (text == null)
            {
                Debug.LogWarning("TextMesh Pro Text is null. Skipping.");
                continue;
            }

            // Check if the text component matches any of the identifiers
            if (text.name == TextIdentifiers.MenuText || text.name == TextIdentifiers.LobbyText)
            {
                textMeshProTexts.Add(text);
            }
        }
    }

    // Sets up the actions for each button
    private void AssignButtonActions()
    {
        buttonMap = new Dictionary<string, Button>();
        Button[] allButtons = canvasInstance.GetComponentsInChildren<Button>(true);

        foreach (Button button in allButtons)
        {
            if (button == null)
            {
                Debug.LogWarning("Button is null. Skipping.");
                continue;
            }

            buttonMap.Add(button.name, button);
        }

        // Main menu buttons
        buttonMap[ButtonIdentifiers.SoloGameButton].onClick.AddListener(() =>
        {
            OnGameModeChange(true);
            GetComponent<SceneManageSystem>().OnLoadSceneButtonClick("GameScene");
        });

        buttonMap[ButtonIdentifiers.MultiplayerButton].onClick.AddListener(() =>
        {

            GetComponent<MultiplayerManageSystem>().OnConnectToPhotonServerButtonClick();
            OnGameModeChange(false);
            SwitchToPanel(PanelIdentifiers.Lobby);
            SetPanelActive(buttonMap[ButtonIdentifiers.ExitLobbyButton].gameObject, false);

        });

        // Lobby buttons

        buttonMap[ButtonIdentifiers.CreateLobbyButton].onClick.AddListener(() =>
        {
            SetPanelActive(buttonMap[ButtonIdentifiers.ExitLobbyButton].gameObject, true);
            SetPanelActive(buttonMap[ButtonIdentifiers.CreateLobbyButton].gameObject, false);
            SetPanelActive(buttonMap[ButtonIdentifiers.JoinLobbyButton].gameObject, false);
            SetPanelActive(buttonMap[ButtonIdentifiers.BackToMenuButton].gameObject, false);

            InitializeTextMeshProTexts();
            TMP_Text lobbyText = textMeshProTexts.Find(text => text.name == TextIdentifiers.LobbyText);
            GetComponent<MultiplayerManageSystem>().OnCreateLobbyButtonClick(lobbyText);
        });

        buttonMap[ButtonIdentifiers.ExitLobbyButton].onClick.AddListener(() =>
        {
            SetPanelActive(buttonMap[ButtonIdentifiers.ExitLobbyButton].gameObject, false);
            SetPanelActive(buttonMap[ButtonIdentifiers.CreateLobbyButton].gameObject, true);
            SetPanelActive(buttonMap[ButtonIdentifiers.JoinLobbyButton].gameObject, true);
            SetPanelActive(buttonMap[ButtonIdentifiers.BackToMenuButton].gameObject, true);

            InitializeTextMeshProTexts();
            TMP_Text lobbyText = textMeshProTexts.Find(text => text.name == TextIdentifiers.LobbyText);
            GetComponent<MultiplayerManageSystem>().OnLeaveLobbyButtonClick(lobbyText);
        });

        buttonMap[ButtonIdentifiers.BackToMenuButton].onClick.AddListener(() =>
        {
            SwitchToPanel(PanelIdentifiers.MainMenu);
            GetComponent<MultiplayerManageSystem>().OnDisconnectButtonClick();
        });

        // In-game buttons
        buttonMap[ButtonIdentifiers.PlayerSwitchButton].onClick.AddListener(() => GetComponent<PlayerControlSystem>().OnSwitchPlayerButtonClick());
        buttonMap[ButtonIdentifiers.ExitGameButton].onClick.AddListener(() => GetComponent<SceneManageSystem>().OnLoadSceneButtonClick("MenuScene"));
    }

    // Finds a panel or a sub panel by its name
    private GameObject FindPanelByName(string panelName, string subPanelName = null)
    {
        if (canvasInstance == null)
        {
            Debug.LogError("Canvas instance is null. Ensure it is properly set up.");
            return null;
        }

        Transform panelTransform = canvasInstance.transform.Find(panelName);
        if (panelTransform == null)
        {
            Debug.LogError($"Panel with name '{panelName}' not found.");
            return null;
        }

        if (subPanelName != null)
        {
            Transform subPanelTransform = panelTransform.Find(subPanelName);
            if (subPanelTransform == null)
            {
                Debug.LogError($"Sub panel with name '{subPanelName}' not found under '{panelName}'.");
                return null;
            }
            return subPanelTransform.gameObject;
        }
        return panelTransform.gameObject;
    }

    // Sets a panel and its sub panels to be active or inactive
    private void SetPanelActive(GameObject panel, bool isActive)
    {
        panel.SetActive(isActive);
        subPanels.TryGetValue(panel.name, out List<GameObject> panelSubPanels);
        panelSubPanels?.ForEach(subPanel => subPanel.SetActive(isActive));
    }

    // Sets the game mode to solo or multiplayer
    private void OnGameModeChange(bool isSolo)
    {
        this.isGameModeSolo = isSolo;
    }
}
