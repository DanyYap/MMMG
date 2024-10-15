using UnityEngine;

[CreateAssetMenu(fileName = "PlatformCanvasSpawner", menuName = "Scriptable Objects/PlatformCanvasSpawner")]
public class PlatformCanvasSpawner : ScriptableObject
{
    public GameObject MobileCanvas;
    public GameObject DesktopCanvas;

    public void SpawnCanvas(PlayerInputManager playerInputManager, PlatformInput input)
    {
        // Choose canvas based on platform
        GameObject canvas = input == PlatformInput.Mobile ? MobileCanvas : DesktopCanvas;

        // Check if canvas already exists
        GameObject existingCanvas = GameObject.Find(canvas.name);
        if (existingCanvas == null)
        {
            // Create the canvas
            existingCanvas = Instantiate(canvas);
        }

        // Get or add UiComponentAttacher
        if (!existingCanvas.TryGetComponent(out UiComponentAttacher uiComponentAttacher))
        {
            uiComponentAttacher = existingCanvas.AddComponent<UiComponentAttacher>();
        }

        // Initialize UiComponentAttacher
        uiComponentAttacher.Initialize(playerInputManager);
    }
}