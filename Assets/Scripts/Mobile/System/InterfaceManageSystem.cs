using UnityEngine;

public class InterfaceManageSystem : MonoBehaviour
{
    public static InterfaceManageSystem Instance { get; private set; }

    private IPanelManager panelManager;
    private MobileInputManager inputManager;
    private TextManager textManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSystem();
            InitializeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SwitchToPanel(PanelIdentifiers.MainMenu);
    }

    private void CreateSystem()
    {
        panelManager = new PanelManager();

        ITextComponentFactory textFactory = new TextComponentFactory(); // Create textFactory instance
        textManager = new TextManager(textFactory);
    }

    public void InitializeSystem()
    {
        // Dependency injection
        var sceneManageSystem = SceneManageSystem.Instance;
        var playerManageSystem = PlayerManageSystem.Instance;
        var cameraController = FindAnyObjectByType<CameraController>();

        inputManager = new MobileInputManager(sceneManageSystem, playerManageSystem, cameraController);
        panelManager.InitializePanels();
        textManager.SetupTexts();
    }

    public MobileInputManager GetInputManager()
    {
        return inputManager;
    }

    public TextManager GetTextManager()
    {
        return textManager;
    }

    public void SwitchToPanel(string panel)
    {
        panelManager.SwitchToPanel(panel);
        inputManager.SetJoystick();
        inputManager.InitializeButtonActions();
        textManager.SetupTexts();
    }
}