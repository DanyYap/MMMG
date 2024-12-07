using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneManager
{
    void LoadSceneName(string sceneName);
    void LoadNextScene();
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
    public void LoadSceneName(string sceneName)
    {
        /*
        if (currentSceneName != sceneName)
        {
            currentSceneName = sceneName;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log($"Scene {sceneName} is already loaded or currently loading.");
        }
        */

        currentSceneName = sceneName;
        SceneManager.LoadScene(sceneName);
        Debug.Log(sceneName + " is loaded!!!!");
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
        LoadSceneName(nextSceneName);
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
            // Optional: Update a loading bar or display progress
            //float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Normalize progress
            //Debug.Log($"Loading progress: {progress * 100}%");

            yield return null;
        }

        // Optional: Hide loading screen
        // HideLoadingScreen();

        isLoadingScene = false;
        currentSceneName = SceneManager.GetActiveScene().name;
    }
}
