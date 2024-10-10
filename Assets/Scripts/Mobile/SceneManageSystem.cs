using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageSystem : MonoBehaviour
{
    private string _currentSceneName;

    void Start()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (_currentSceneName != SceneManager.GetActiveScene().name)
        {
            _currentSceneName = SceneManager.GetActiveScene().name;
            StartCoroutine(WaitForSceneLoad());
        }
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        OnSceneChanged();
    }

    private void OnSceneChanged()
    {
        // Trigger an event or perform actions when the scene is changed
        Debug.Log("Scene changed to: " + _currentSceneName);
        CanvasManageSystem canvasManageSystem = GetComponent<CanvasManageSystem>();
        if (_currentSceneName == "MenuScene")
        {
            canvasManageSystem.SwitchToMenuPanel();
            canvasManageSystem.AssignButtonListeners();
        }
        else
        {
            canvasManageSystem.SwitchToInGamePanel();
            canvasManageSystem.AssignButtonListeners();

            MobileInputSystem mobileInputSystem = GetComponent<MobileInputSystem>();
            mobileInputSystem.FindJoystick();

            PlayerControlSystem playerControlSystem = GetComponent<PlayerControlSystem>();
            playerControlSystem.InitializePlayers();
        }
    }

    public void OnLoadSceneButtonClick(string sceneName)
    {
        if (_currentSceneName != sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("Scene " + sceneName + " is already loaded.");
        }
    }
}
