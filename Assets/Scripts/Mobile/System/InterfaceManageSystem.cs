using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManageSystem : MonoBehaviour
{
    public static InterfaceManageSystem Instance { get; private set; }

    private PanelManager panelManager;
    private JoystickManager joystickManager;
    private ButtonManager buttonManager = new ButtonManager();
    private TextManager textManager = new TextManager();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Register for scene load and unload events
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        InitializeForScene(SceneManager.GetActiveScene());
    }

    private void InitializeForScene(Scene scene)
    {
        var canvas = FactoryManageSystem.Instance.CanvasFactory.CreateInstance(UiElementNames.Canvas.Mobile);
        panelManager = new PanelManager(canvas.GetComponent<Canvas>());

        if (scene.name == SceneNames.MenuScene)
        {
            panelManager.ShowPanel(UiElementNames.Panels.MainMenu);
        }
        else
        {
            panelManager.ShowPanel(UiElementNames.Panels.InGame);
        }

        InitializeUiElements();
    }

    public void InitializeUiElements()
    {
        InitializeJoystick();
        InitializeButtons();
        InitializeTexts();
    }

    private void InitializeJoystick()
    {
        FixedJoystick joystick = Object.FindAnyObjectByType<FixedJoystick>();
        if (joystick != null)
        {
            joystickManager = new JoystickManager(joystick);
        }
        else
        {
            Debug.LogWarning("FixedJoystick not found in the scene.");
        }
    }

    private void InitializeButtons()
    {
        var buttonActions = new Dictionary<string, IButtonAction>
        {
            { UiElementNames.Buttons.QuitGame, new QuitGameAction() },
            { UiElementNames.Buttons.Interact, new InteractObjectAction() },
            { UiElementNames.Buttons.UseTool, new InteractObjectAction() },
            { UiElementNames.Buttons.StartGame, new StartGameAction(SceneManageSystem.Instance) },
            { UiElementNames.Buttons.BackToMenu, new BackMenuAction(SceneManageSystem.Instance, GameManageSystem.Instance) },
            { UiElementNames.Buttons.SwitchPlayer, new SwitchPlayerAction(PlayerManageSystem.Instance) },
            { UiElementNames.Buttons.RotateCamera, new RotateCameraAction(FindAnyObjectByType<CameraController>())},
            { UiElementNames.Buttons.RetryLevel, new RetryLevelAction(SceneManageSystem.Instance)},
            { UiElementNames.Buttons.NextLevel, new NextLevelAction(SceneManageSystem.Instance)},
        };

        buttonManager.InitializeButtonActions(buttonActions);
    }

    private void InitializeTexts()
    {
        var textActions = new Dictionary<string, ITextUpdate>
        {
            { UiElementNames.Texts.FireHealthLeftText, new FireHealthLeft() },
            { UiElementNames.Texts.CountdownText, new Countdown(GameManageSystem.Instance.GetTimeManager.GetCountdownTimer()) },
            { UiElementNames.Texts.ResultText, new Result()}
        };

        textManager.InitializeTextActions(textActions);
    }

    private void ClearForScene(Scene scene)
    {
        buttonManager.ClearAllButtonActions();
        textManager.ClearAllTextActions();

        if (panelManager != null)
        {
            panelManager.HideAllPanels();
        }

        joystickManager = null; // Clear joystick manager
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeForScene(scene);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        ClearForScene(scene);
    }

    private void OnDestroy()
    {
        // Unregister from scene events
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public PanelManager GetPanelManager() => panelManager;

    public ButtonManager GetButtonManager() => buttonManager;

    public JoystickManager GetJoystickManager() => joystickManager;

    public TextManager GetTextManager() => textManager;
}
