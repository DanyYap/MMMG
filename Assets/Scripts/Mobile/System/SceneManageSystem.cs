using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneNames
{
    public const string MenuScene = "MenuScene";
    public const string GameScene = "GameScene";
}

public interface ISceneController
{
    void OnSceneChanged(string sceneName);
    void LoadScene(string sceneName);
}

public class SceneManageSystem : MonoBehaviour, ISceneController
{
    public static SceneManageSystem Instance;

    private string currentSceneName;
    private bool isLoadingScene = false; // Flag to prevent multiple loads
    private InterfaceManageSystem interfaceManageSystem;
    private PlayerManageSystem playerManageSystem;

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

        currentSceneName = SceneManager.GetActiveScene().name;
        interfaceManageSystem = GetComponent<InterfaceManageSystem>();
        playerManageSystem = GetComponent<PlayerManageSystem>();
    }

    public void OnSceneChanged(string sceneName)
    {
        Debug.Log($"Scene changed to: {sceneName}");
        
        switch (sceneName)
        {
            case SceneNames.MenuScene:
                ActivateMenu();
                break;
            case SceneNames.GameScene:
                ActivateGame();
                break;
            default:
                Debug.LogWarning($"Unknown scene: {sceneName}.");
                break;
        }
    }

    private void ActivateMenu()
    {
        interfaceManageSystem.SwitchToPanel(PanelIdentifiers.MainMenu);
    }

    private void ActivateGame()
    {
        interfaceManageSystem.SwitchToPanel(PanelIdentifiers.InGame);
        playerManageSystem.InitializePlayers();
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Load Scene");

        if (!isLoadingScene && currentSceneName != sceneName)
        {
            isLoadingScene = true; // Set loading flag
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.Log($"Scene {sceneName} is already loaded or currently loading.");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to load scene {sceneName}.");
            yield break;
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isLoadingScene = false; 
        CheckSceneChange();
    }

    private void CheckSceneChange()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (!currentSceneName.Equals(activeSceneName))
        {
            currentSceneName = activeSceneName;
            OnSceneChanged(currentSceneName);
        }
    }
}
