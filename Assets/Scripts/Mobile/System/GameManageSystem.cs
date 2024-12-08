using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    [SerializeField]
    private GameLevelSettings levelSettings;

    private GameStateManager gameStateManager = new GameStateManager();
    private HealthSceneManager healthSceneManager = new HealthSceneManager();
    private TimeManager timeManager;

    private bool isUpdating = false;
    private bool hasWon = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeManagers();
            InitializeStateHandlers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isUpdating) return;

        timeManager.UpdateGameTime();
        UpdateFireHealthUI();
        UpdateCountdownUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void InitializeManagers()
    {
        timeManager = new TimeManager(levelSettings);
        gameStateManager.ChangeState(GameState.MainMenu);
    }

    private void InitializeStateHandlers()
    {
        // Enter State Handlers
        gameStateManager.AddEnterAction(GameState.MainMenu, () =>
        {
            Debug.Log("Enter Action: MainMenu");
            isUpdating = false;
            SceneManager.LoadScene(SceneNames.MenuScene);
        });

        gameStateManager.AddEnterAction(GameState.GameStart, () =>
        {
            Debug.Log("Enter Action: GameStart");
            isUpdating = false;
            timeManager = new TimeManager(levelSettings);
            timeManager.StopGameTime();
            
        });

        gameStateManager.AddEnterAction(GameState.Playing, () =>
        {
            Debug.Log("Enter Action: Playing");
            isUpdating = true;
            timeManager.StartGameTime();
        });

        gameStateManager.AddEnterAction(GameState.Paused, () =>
        {
            Debug.Log("Enter Action: Paused");
            isUpdating = false;
            timeManager.StopGameTime();
            InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.PauseMenu);
            InterfaceManageSystem.Instance.InitializeUiElements();
        });

        gameStateManager.AddEnterAction(GameState.GameEnd, () =>
        {
            Debug.Log("Enter Action: GameEnd");
            isUpdating = false;
            InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(
                hasWon ? UiElementNames.Panels.WinResult : UiElementNames.Panels.LoseResult
            );
            InterfaceManageSystem.Instance.InitializeUiElements();
            InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.ResultText, hasWon);
        });

        // Exit State Handlers
        gameStateManager.AddExitAction(GameState.Paused, () =>
        {
            Debug.Log("Exit Action: Paused");
            timeManager.StartGameTime();
            InterfaceManageSystem.Instance.GetPanelManager().HidePanel(UiElementNames.Panels.PauseMenu);
        });

        gameStateManager.AddExitAction(GameState.GameEnd, () =>
        {
            Debug.Log("Exit Action: GameEnd");
            healthSceneManager.ClearAllHealthData();
            InterfaceManageSystem.Instance.GetTextManager().ClearAllTextActions();
            InterfaceManageSystem.Instance.GetButtonManager().ClearAllButtonActions();
            InterfaceManageSystem.Instance.GetPanelManager().HideAllPanels();
        });
    }

    private void UpdateFireHealthUI()
    {
        var fireHealths = healthSceneManager.GetTotalHealthData("object");
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.FireHealthLeftText, fireHealths);

        if (fireHealths == 0)
        {
            hasWon = true;
            gameStateManager.ChangeState(GameState.GameEnd);
        }
    }

    private void UpdateCountdownUI()
    {
        var countdownTime = timeManager.GetCountdownTimer().GetRemainingTime();
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.CountdownText, countdownTime);

        if (countdownTime == 0)
        {
            hasWon = false;
            gameStateManager.ChangeState(GameState.GameEnd);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeGameScene();
        InitializeStateHandlers();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        healthSceneManager.OnSceneChanged("player");
        healthSceneManager.OnSceneChanged("object");
        timeManager.StopGameTime();
    }

    private void InitializeGameScene()
    {
        if (gameStateManager.CurrentState == GameState.GameStart)
        {
            healthSceneManager.InitializeHealthDataForScene("player");
            healthSceneManager.InitializeHealthDataForScene("object");
            timeManager.StartGameTime();
            gameStateManager.ChangeState(GameState.Playing);
        }
    }

    public GameStateManager GetGameStateManager => gameStateManager;
    public HealthSceneManager GetHealthSceneManager => healthSceneManager;
    public TimeManager GetTimeManager => timeManager;

    private void OnApplicationPause(bool paused)
    {
        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene) return;

        gameStateManager.ChangeState(paused ? GameState.Paused : GameState.Playing);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
