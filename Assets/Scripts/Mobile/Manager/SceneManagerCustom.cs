using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneManager
{
    void LoadScene(string sceneName);
    void OnSceneChanged(string sceneName);
}

public class SceneManagerCustom : ISceneManager
{
    private string currentSceneName;
    private bool isLoadingScene = false;

    public SceneManagerCustom()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName)
    {
        if (!isLoadingScene && currentSceneName != sceneName)
        {
            isLoadingScene = true;
            PlayerManageSystem.Instance.GetPlayerManager().InitializePlayers();
            SceneManageSystem.Instance.StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.Log($"Scene {sceneName} is already loaded or currently loading.");
        }
    }

    public void OnSceneChanged(string sceneName)
    {
        Debug.Log($"Scene changed to: {sceneName}");

        PlayerManageSystem.Instance.InitializeSystem();
        InterfaceManageSystem.Instance.InitializeSystem();
        GameManageSystem.Instance.InitializeSystem();

        InterfaceManageSystem.Instance.SwitchToPanel(sceneName == SceneNames.MenuScene
            ? PanelIdentifiers.MainMenu
            : PanelIdentifiers.InGame);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
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
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        OnSceneChanged(currentSceneName);
    }
}
