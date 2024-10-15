using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonManagement : MonoBehaviour
{
    // Array to hold all buttons with the tag "Button"
    private Button[] btn;

    // Sprites for the button states (normal and highlighted)
    [SerializeField] private Sprite button1;
    [SerializeField] private Sprite button2;

    // Index of the currently selected button
    private int selected_Button = 0;

    // Flag to prevent multiple button presses in quick succession
    private bool buttonpressed = false;

    // Reference to the first button for panel changes
    private Button firstButton;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Navigate"].performed += OnNavigate;
        playerInput.actions["Navigate"].canceled += OnNavigateCanceled;
    }

    private void Start()
    {
        // Find all buttons with the tag "Button" once at the start
        btn = FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);
        if (btn.Length > 0)
        {
            ManageButtonSelection();
        }
    }

    private void OnDestroy()
    {
        playerInput.actions["Navigate"].performed -= OnNavigate;
        playerInput.actions["Navigate"].canceled -= OnNavigateCanceled;
    }

    // Handles input for button navigation
    private void OnNavigate(InputAction.CallbackContext context)
    {
        if (!buttonpressed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            if (input.y < 0)
            {
                buttonpressed = true;
                UpdateSelection(true, false); // Navigate down
            }
            else if (input.y > 0)
            {
                buttonpressed = true;
                UpdateSelection(false, false); // Navigate up
            }
            else if (input.x < 0)
            {
                buttonpressed = true;
                UpdateSelection(true, true); // Navigate left
            }
            else if (input.x > 0)
            {
                buttonpressed = true;
                UpdateSelection(false, true); // Navigate right
            }
        }
    }

    private void OnNavigateCanceled(InputAction.CallbackContext context)
    {
        buttonpressed = false;
    }

    // Manages the button selection logic
    private void ManageButtonSelection()
    {
        // Reset selection if the first button has changed
        if (firstButton != btn[0])
        {
            selected_Button = 0;
        }
        firstButton = btn[0];

        // Highlight the currently active button
        HighlightActiveButton();
    }

    // Highlights the currently selected button and resets others
    private void HighlightActiveButton()
    {
        for (int a = 0; a < btn.Length; a++)
        {
            if (btn[a] == null)
            {
                continue; // Skip if the button has been destroyed
            }

            var buttonImage = btn[a].GetComponent<Image>();
            if (a == selected_Button)
            {
                // Select and change sprite for the active button
                var buttonComponent = btn[a].GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.Select();
                }
                buttonImage.sprite = button2;
            }
            else
            {
                // Reset sprite for inactive buttons
                buttonImage.sprite = button1;
            }
        }
    }

    // Updates the selected button based on navigation
    private void UpdateSelection(bool isPositive, bool isHorizontal)
    {
        float closestDistance = float.MaxValue;
        int newSelectedButton = selected_Button;
        Vector3 currentButtonPos = btn[selected_Button].GetComponent<RectTransform>().position;

        for (int a = 0; a < btn.Length; a++)
        {
            if (a != selected_Button)
            {
                Vector3 otherButtonPos = btn[a].GetComponent<RectTransform>().position;
                float distance = Vector3.Distance(currentButtonPos, otherButtonPos);

                if (isHorizontal)
                {
                    if ((isPositive && otherButtonPos.x < currentButtonPos.x) || (!isPositive && otherButtonPos.x > currentButtonPos.x))
                    {
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            newSelectedButton = a;
                        }
                    }
                }
                else
                {
                    if ((isPositive && otherButtonPos.y < currentButtonPos.y) || (!isPositive && otherButtonPos.y > currentButtonPos.y))
                    {
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            newSelectedButton = a;
                        }
                    }
                }
            }
        }

        selected_Button = newSelectedButton;
        HighlightActiveButton();
    }
}