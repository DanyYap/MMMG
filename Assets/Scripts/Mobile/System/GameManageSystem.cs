using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    [SerializeField]
    private GameLevelSettings levelSettings;

    private HealthSceneManager healthSceneManager = new HealthSceneManager(); // Manages health in the scene
    private TimeManager timeManager; // Manages time in the scene

    private bool isGameStarted = false;

    public delegate void GameEvent();
    public event GameEvent OnGameStart;
    public event GameEvent OnGameEnd;

    // Additional state tracking
    private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            timeManager = new TimeManager(levelSettings);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnGameStart += InitializeLevel;
        OnGameEnd += HandleGameEnd;
    }

    private void Update()
    {
        if (!isGameStarted || isGamePaused) return;

        // Notify UI with values only if the game is not paused
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

    public void PauseGame()
    {
        isGamePaused = true;
        // Additional pause logic, e.g., stop time manager
        timeManager.StopGameTime();
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        // Additional unpause logic, e.g., resume time manager
        timeManager.StartGameTime();
    }

    private void InitializeLevel()
    {
        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene)
        {
            isGameStarted = false;
        }
        else
        {
            isGameStarted = true;
            timeManager.StartGameTime();
        }
    }

    private void HandleGameEnd()
    {
        timeManager.StopGameTime();

        // Show result panel
        InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.WinResult);
        InterfaceManageSystem.Instance.InitializeUiElements();

        /*
        // Optional: Show win/lose panel based on outcome
        if (healthSceneManager.GetTotalHealthData("object") == 0)
        {
            InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.Win);
        }
        else
        {
            InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.LoseResult);
        }
        */
    }

    private void UpdateFireHealthUI()
    {
        var fireHealths = healthSceneManager.GetTotalHealthData("object");
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.FireHealthLeftText, fireHealths);

        if (fireHealths == 0) HandleLevelSuccess();
    }

    private void UpdateCountdownUI()
    {
        var countdownTime = timeManager.GetCountdownTimer().GetRemainingTime();
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.CountdownText, countdownTime);

        if (countdownTime == 0) HandleLevelFailure();
    }

    private void HandleLevelSuccess()
    {
        isGameStarted = false;
        OnGameEnd += () => InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.WinResult);
        OnGameEnd += () => InterfaceManageSystem.Instance.InitializeUiElements();
        OnGameEnd += () => InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.ResultText, true);
        OnGameEnd?.Invoke();

        OnGameEnd -= () => InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.WinResult);
        OnGameEnd -= () => InterfaceManageSystem.Instance.InitializeUiElements();
        OnGameEnd -= () => InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.ResultText, true);
    }

    private void HandleLevelFailure()
    {
        isGameStarted = false;
        OnGameEnd += () => InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.LoseResult);
        OnGameEnd += () => InterfaceManageSystem.Instance.InitializeUiElements();
        OnGameEnd += () => InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.ResultText, false);
        OnGameEnd?.Invoke();

        OnGameEnd -= () => InterfaceManageSystem.Instance.GetPanelManager().ShowPanel(UiElementNames.Panels.LoseResult);
        OnGameEnd -= () => InterfaceManageSystem.Instance.InitializeUiElements();
        OnGameEnd -= () => InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.ResultText, false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneNames.MenuScene) return;

        // Initialize health and time manager for the new scene
        healthSceneManager.InitializeHealthDataForScene("player");
        healthSceneManager.InitializeHealthDataForScene("object");
        timeManager = new TimeManager(levelSettings);

        OnGameStart?.Invoke();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == SceneNames.MenuScene) return;

        healthSceneManager.OnSceneChanged("player");
        healthSceneManager.OnSceneChanged("object");
        timeManager.StopGameTime();
    }

    public HealthSceneManager GetHealthSceneManager => healthSceneManager;
    public TimeManager GetTimeManager => timeManager;

    // Handle game state persistence
    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    private void OnDestroy()
    {
        // Unregister from scene events and game state events
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Function to reset the game and return to the main menu
    public void ResetGame()
    {
        // Clear game state
        timeManager.StopGameTime();
        healthSceneManager.ClearAllHealthData();

        // Clear UI elements
        InterfaceManageSystem.Instance.GetTextManager().ClearAllTextActions();
        InterfaceManageSystem.Instance.GetButtonManager().ClearAllButtonActions();

        // Hide all panels
        InterfaceManageSystem.Instance.GetPanelManager().HideAllPanels();

        // Load the main menu
        SceneManager.LoadScene(SceneNames.MenuScene);
    }
}
