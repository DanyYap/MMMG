using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonIdentifiers
{
    // main menu buttons
    public const string SoloGameButton      = "Solo Game Button";
    public const string CoopButton          = "Coop Game Button";

    // pause buttons
    public const string BackToMenuButton    = "Back To Menu Button";

    // in game buttons
    public const string ExitGameButton      = "Exit Game Button";
    public const string PlayerSwitchButton  = "Player Switch Button";
    public const string InteractButton      = "Player Interact Button";
    public const string RotateCameraButton  = "Rotate Camera Button";
}

public interface IButtonComponent
{
    void InitializeButton(Button button);
    void InitializeAction(UnityAction action);
    void ExecuteAction();
}

public class GameButton : IButtonComponent
{
    private Button button;
    private UnityAction action;

    public GameButton(Button button, UnityAction action)
    {
        InitializeButton(button);
        InitializeAction(action);
    }

    public void InitializeButton(Button button)
    {
        this.button = button;
    }

    public void InitializeAction(UnityAction action)
    {
        if (action == null) return;

        if (button != null)
        {
            this.action = action;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ExecuteAction);
        }
    }

    public void ExecuteAction()
    {
        button.interactable = false;
        action();
        button.interactable = true;
    }
}