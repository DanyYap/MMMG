using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Manager for handling UI transitions and button actions
public class InterfaceManager : MonoBehaviour
{
    public GameObject mainMenuPrefab;
    public GameObject lobbyPrefab;
    public GameObject inGamePrefab;

    private Dictionary<string, IPanel> panels;
    private IPanel currentPanel;

    void Start()
    {
        InitializePanels();
        SwitchToPanel("Main Menu"); // Start with Main Menu
    }

    private void InitializePanels()
    {
        panels = new Dictionary<string, IPanel>
        {
            { "Main Menu", new MainMenuPanel(Instantiate(mainMenuPrefab)) },
            { "Lobby", new LobbyPanel(Instantiate(lobbyPrefab)) },
            { "In Game", new InGamePanel(Instantiate(inGamePrefab)) }
        };
    }

    public void SwitchToPanel(string panelId)
    {
        currentPanel?.Hide();
        if (panels.TryGetValue(panelId, out currentPanel))
        {
            currentPanel.Show();
        }
        else
        {
            Debug.LogError($"Panel '{panelId}' not found.");
        }
    }

    public void SetupButtonActions()
{
    Button soloButton = GameObject.Find(ButtonIdentifiers.SoloGameButton)?.GetComponent<Button>();
    Button multiplayerButton = GameObject.Find(ButtonIdentifiers.MultiplayerButton)?.GetComponent<Button>();
    
    //soloButton?.onClick.AddListener(new StartSoloGameAction().Execute);
    }

}
