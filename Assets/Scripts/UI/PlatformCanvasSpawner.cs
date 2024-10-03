using UnityEngine;

[CreateAssetMenu(fileName = "PlatformCanvasSpawner", menuName = "Scriptable Objects/PlatformCanvasSpawner")]
public class PlatformCanvasSpawner : ScriptableObject
{
    public GameObject MobileCanvas;
    public GameObject DesktopCanvas;

    public void SpawnCanvas(PlatformInput input)
    {
        GameObject canvas = input == PlatformInput.Mobile ? MobileCanvas : DesktopCanvas;
        if (GameObject.Find(canvas.name) == null)
        {
            Instantiate(canvas);
        }
    }
}