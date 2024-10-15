using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CanvasManageSystem;

public class SceneManageSystem : MonoBehaviour
{
    private string currentSceneName;
    private CanvasManageSystem canvasManageSystem;
    private MobileInputSystem mobileInputSystem;
    private PlayerControlSystem playerControlSystem;
    private bool isLoadingScene = false; // Flag to prevent multiple loads

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        InitializeComponents();
    }

    void Update()
    {
        CheckSceneChange();
    }

    // Set up the necessary components for the game.
    private void InitializeComponents()
    {
        canvasManageSystem = GetComponent<CanvasManageSystem>();
        mobileInputSystem = GetComponent<MobileInputSystem>();
        playerControlSystem = GetComponent<PlayerControlSystem>();
        ValidateComponents(); // Check if all components are found.
    }

    // Check if all required components are present.
    private void ValidateComponents()
    {
        if (canvasManageSystem == null || mobileInputSystem == null || playerControlSystem == null)
        {
            Debug.LogError("One or more required components are missing.");
        }
    }

    // Check if the current scene has changed.
    private void CheckSceneChange()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != activeSceneName)
        {
            currentSceneName = activeSceneName;
            StartCoroutine(WaitForSceneLoad()); // Wait for the new scene to load.
        }
    }

    // Wait for the new scene to load before proceeding.
    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        OnSceneChanged(); // Scene has loaded, now do the necessary setup.
    }

    // Scene has changed and is now loaded.
    private void OnSceneChanged()
    {
        Debug.Log("Scene changed to: " + currentSceneName);

        if (currentSceneName == SceneNames.MenuScene)
        {
            canvasManageSystem.SwitchToPanel(PanelIdentifiers.MainMenu);
        }
        else
        {
            canvasManageSystem.SwitchToPanel(PanelIdentifiers.InGame);
            mobileInputSystem.FindJoystick();
            playerControlSystem.InitializePlayers();
        }
    }

    // Load a scene when a button is clicked.
    public void OnLoadSceneButtonClick(string sceneName)
    {
        if (!isLoadingScene && currentSceneName != sceneName) // Check if not already loading
        {
            isLoadingScene = true; // Set loading flag
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.Log("Scene " + sceneName + " is already loaded or currently loading.");
        }
    }

    // Load a scene asynchronously.
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        isLoadingScene = false; // Reset loading flag after loading
    }
}

public static class SceneNames
{
    public const string MenuScene = "MenuScene";
    // Add other scene names as needed
}
