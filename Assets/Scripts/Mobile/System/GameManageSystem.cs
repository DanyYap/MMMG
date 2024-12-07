using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManageSystem : MonoBehaviour
{
    public static GameManageSystem Instance { get; private set; }

    [SerializeField]
    private GameLevelSettings levelSettings;

    private HealthSceneManager healthSceneManager = new HealthSceneManager(); // Manage health in the scene
    private TimeManager timeManager;    // Manage time in the scene

    private bool hasSceneTransitioned = false; // Flag to ensure LoadNextScene is called only once
    
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

    private void Update()
    {
        // Skip processing if in MenuScene
        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene)
        {
            return;
        }

        timeManager.UpdateGameTime();

        // Check if total fire healths have dropped to 0
        var fireHealths = healthSceneManager.GetTotalHealthData("object");
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.FireHealthLeftText, fireHealths);

        var countdownTime = timeManager.GetCountdownTimer().GetRemainingTime();
        InterfaceManageSystem.Instance.GetTextManager().UpdateText(UiElementNames.Texts.CountdownText, countdownTime);

        //Debug.Log(fireHealths);
        if (fireHealths == 0)
        {
            Debug.Log("load next scene");
        }
        if (fireHealths == 0 && !hasSceneTransitioned)
        {
            hasSceneTransitioned = true; // Set the flag to prevent repeated transitions
            SceneManageSystem.Instance.GetSceneManager().LoadNextScene();
        }
        if (countdownTime == 0)
        {
            hasSceneTransitioned = true; // Set the flag to prevent repeated transitions
            SceneManageSystem.Instance.GetSceneManager().LoadSceneName(SceneNames.MenuScene);
        }
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the flag to allow scene transitions in the new scene
        hasSceneTransitioned = false;

        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene)
        {
            return;
        }

        // Initialize health data for player and other entities in the new scene
        healthSceneManager.InitializeHealthDataForScene("player"); // Initialize health data for players
        healthSceneManager.InitializeHealthDataForScene("object");

        timeManager = new TimeManager(levelSettings);
        timeManager.StartGameTime();
    }

    // Called when a scene is unloaded
    private void OnSceneUnloaded(Scene scene)
    {
        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene)
        {
            return;
        }

        // Clear health data for player and other entities when the scene is unloaded
        healthSceneManager.OnSceneChanged("player"); // Unregister player health data for the previous scene
        healthSceneManager.OnSceneChanged("object");

        timeManager.StopGameTime();
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

    public HealthSceneManager GetHealthSceneManager => healthSceneManager;
    public TimeManager GetTimeManager => timeManager;
}
