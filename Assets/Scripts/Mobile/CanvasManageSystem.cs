using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManageSystem : MonoBehaviour
{
    public CanvasScriptableObject canvasScriptableObject;
    private GameObject canvasInstance;
    private GameObject menuPanel;
    private GameObject inGamePanel;
    private GameObject bothPanel;
    private GameObject soloPanel;
    private GameObject multiplayerPanel;
    private bool isSolo;

    void Start()
    {
        InitializeCanvas();
        EnablePanel(menuPanel);
        DisablePanel(inGamePanel);
        AssignButtonListeners();
    }

    public void SwitchToInGamePanel()
    {
        InitializeCanvas();
        EnablePanel(inGamePanel);
        DisablePanel(menuPanel);

        EnablePanel(bothPanel);
        if (isSolo)
        {
            EnablePanel(soloPanel);
            DisablePanel(multiplayerPanel);
        }
        else
        {
            EnablePanel(multiplayerPanel);
            DisablePanel(soloPanel);
        }
    }

    private void InitializeCanvas()
    {
        // Check if the canvas already exists in the scene
        canvasInstance = GameObject.FindWithTag("UI");
        Debug.Log(canvasInstance);

        // If it doesn't exist, instantiate it
        if (canvasInstance == null)
        {
            canvasInstance = Instantiate(canvasScriptableObject.CanvasPrefab);
        }
        else
        {
            canvasInstance.SetActive(true);
        }

        // Find the Menu Panel and In Game Panel within the canvasInstance children
        foreach (Transform child in canvasInstance.transform)
        {
            if (child.name == "Menu Panel")
            {
                menuPanel = child.gameObject;
            }
            else if (child.name == "In Game Panel")
            {
                inGamePanel = child.gameObject;
                // Find Both, Solo, and Multiplayer panels within the inGamePanel children
                foreach (Transform inGameChild in inGamePanel.transform)
                {
                    switch (inGameChild.name)
                    {
                        case "Both":
                            bothPanel = inGameChild.gameObject;
                            break;
                        case "Solo":
                            soloPanel = inGameChild.gameObject;
                            break;
                        case "Multiplayer":
                            multiplayerPanel = inGameChild.gameObject;
                            break;
                    }
                }
            }
        }
    }

    private void EnablePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
            foreach (Transform child in panel.transform)
            {
                if (child != null)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    private void DisablePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
            foreach (Transform child in panel.transform)
            {
                if (child != null)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnSetSoloButtonClick(bool isSolo)
    {
        this.isSolo = isSolo;
        Debug.Log(isSolo);
    }

    public void AssignButtonListeners()
    {
        Button[] buttons = canvasInstance.GetComponentsInChildren<Button>(true);
        Debug.Log(buttons.Length);
        foreach (Button button in buttons)
        {
            switch (button.name)
            {
                case "Solo Button":
                    if (!button.onClick.GetPersistentEventCount().Equals(0))
                    {
                        button.onClick.RemoveAllListeners();
                    }
                    button.onClick.AddListener(() => OnSetSoloButtonClick(true));
                    SceneManageSystem sceneManageSystem = GetComponent<SceneManageSystem>();
                    button.onClick.AddListener(() => sceneManageSystem.OnLoadSceneButtonClick("PlayerInputTestingScene"));
                    break;
                case "Multiplayer Button":
                    if (!button.onClick.GetPersistentEventCount().Equals(0))
                    {
                        button.onClick.RemoveAllListeners();
                    }
                    button.onClick.AddListener(() => OnSetSoloButtonClick(false));
                    sceneManageSystem = GetComponent<SceneManageSystem>();
                    button.onClick.AddListener(() => sceneManageSystem.OnLoadSceneButtonClick("PlayerInputTestingScene"));
                    break;
                case "Switch Button":
                    if (!button.onClick.GetPersistentEventCount().Equals(0))
                    {
                        button.onClick.RemoveAllListeners();
                    }
                    PlayerControlSystem playerControlSystem = GetComponent<PlayerControlSystem>();
                    button.onClick.AddListener(() => playerControlSystem.OnSwitchPlayerButtonClick());
                    break;
                case "Leave Button":
                    if (!button.onClick.GetPersistentEventCount().Equals(0))
                    {
                        button.onClick.RemoveAllListeners();
                    }
                    sceneManageSystem = GetComponent<SceneManageSystem>();
                    button.onClick.AddListener(() => sceneManageSystem.OnLoadSceneButtonClick("MenuScene"));
                    break;
                    // Add more cases for other buttons as needed
            }
        }
    }

    internal void SwitchToMenuPanel()
    {
        InitializeCanvas();
        EnablePanel(menuPanel);
        DisablePanel(inGamePanel);
    }
}
