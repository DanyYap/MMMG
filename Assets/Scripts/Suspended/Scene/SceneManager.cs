using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    // Enum to store all scene names
    public enum SceneNames
    {
        InputControlScene,
        PlayerInputTestingScene
    }

    [SerializeField] private SceneNames sceneName;
    
    // Variable to store the last loaded scene
    private static SceneNames lastSceneName;

    // Static instance of the SceneManager
    private static SceneManagerCustom instance;

    // Ensure there's only one instance of SceneManager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to load a scene by enum
    public void LoadScene()
    {
        lastSceneName = sceneName;
        Debug.Log($"Scene loaded: {sceneName}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());

        /*
        // Store the current scene as the last scene before loading the new one
        if (lastSceneName != sceneName)
        {
            lastSceneName = sceneName;
            Debug.Log($"Scene loaded: {sceneName}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
        }*/
    }

    // Function to get the last loaded scene
    public static SceneNames GetLastSceneName()
    {
        return lastSceneName;
    }
}
