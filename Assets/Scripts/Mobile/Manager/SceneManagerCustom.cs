using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneManager
{
    void LoadScene(string sceneName);
    void LoadNextScene();
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

    // Load a specific scene by name
    public void LoadScene(string sceneName)
    {
        if (!isLoadingScene && currentSceneName != sceneName)
        {
            isLoadingScene = true;
            SceneManageSystem.Instance.StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.Log($"Scene {sceneName} is already loaded or currently loading.");
        }
    }

    // Load the next scene in the build index
    public void LoadNextScene()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        int nextSceneIndex;

        if (currentBuildIndex + 1 < totalScenes) // Check if there's a next scene
        {
            nextSceneIndex = currentBuildIndex + 1; // Load the next scene
        }
        else
        {
            nextSceneIndex = 0; // Loop back to the first scene
            Debug.Log("Reached the last scene. Looping back to the first scene.");
        }

        string nextSceneName = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
        Debug.Log($"Loading scene: {nextSceneName}");
        LoadScene(nextSceneName);
    }


    public void OnSceneChanged(string sceneName)
    {
        Debug.Log($"Scene changed to: {sceneName}");

        InterfaceManageSystem.Instance.InitializeSystem();

        InterfaceManageSystem.Instance.SwitchToPanel(sceneName == SceneNames.MenuScene
            ? PanelIdentifiers.MainMenu
            : PanelIdentifiers.InGame);
        //GameManageSystem.Instance.EnableExecution(sceneName == SceneNames.GameScene);
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
        currentSceneName = SceneManager.GetActiveScene().name;
        OnSceneChanged(currentSceneName);
    }
}
