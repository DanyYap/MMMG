using UnityEngine;

[CreateAssetMenu(fileName = "PlatformCanvasSpawner", menuName = "Scriptable Objects/PlatformCanvasSpawner")]
public class PlatformCanvasSpawner : ScriptableObject
{
    public GameObject MobileCanvas;
    public GameObject DesktopCanvas;

    public void SpawnCanvas(PlatformInput input)
    {
        if (input == PlatformInput.Mobile)
        {
            Instantiate(MobileCanvas);
        }
        else if (input == PlatformInput.KeyboardOrConsole)
        {
            Instantiate(DesktopCanvas);
        }
    }
}
