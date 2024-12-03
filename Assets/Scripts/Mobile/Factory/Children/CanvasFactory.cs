using System.Collections.Generic;
using UnityEngine;

// Factory for creating and managing multiple Canvas instances.
[CreateAssetMenu(fileName = "CanvasFactory", menuName = "Scriptable Objects/CanvasFactory")]
public class CanvasFactory : ScriptableFactoryBase<Canvas>
{
    [System.Serializable]
    public class CanvasEntry
    {
        public string key;  // Key to identify the canvas (e.g., "MainMenu", "HUD").
        public GameObject prefab;  // The GameObject prefab for this canvas type.
    }

    [SerializeField] private List<CanvasEntry> canvasPrefabs;  // List of available canvas GameObject prefabs.
    private Dictionary<string, Canvas> canvasInstances;  // Dictionary to store created canvas instances.

    private void OnEnable()
    {
        // Initialize the dictionary to hold canvas instances.
        canvasInstances = new Dictionary<string, Canvas>();

        // Validate that each prefab has a Canvas component.
        foreach (var entry in canvasPrefabs)
        {
            if (entry.prefab != null && entry.prefab.GetComponent<Canvas>() == null)
            {
                Debug.LogError($"Prefab for key '{entry.key}' does not have a Canvas component attached!");
            }
        }
    }

    public override GameObject CreateInstance(string key, Vector3? position = null, Quaternion? rotation = null)
    {
        // Default position and rotation if not provided.
        Vector3 spawnPosition = position ?? Vector3.zero;  // Default to (0,0,0) if null
        Quaternion spawnRotation = rotation ?? Quaternion.identity;  // Default to no rotation if null

        // Check if a canvas with this key already exists in the scene.
        Canvas existingCanvas = FindExistingCanvasInScene(key);
        if (existingCanvas != null)
        {
            Debug.LogWarning($"Canvas with key '{key}' already exists in the scene. Returning the existing instance.");
            return existingCanvas.gameObject; // Return the existing GameObject with the Canvas component
        }

        // Find the corresponding canvas prefab for this key.
        var entry = canvasPrefabs.Find(c => c.key == key);
        if (entry != null)
        {
            // Instantiate the new GameObject (which contains the Canvas component)
            var newCanvasGO = Instantiate(entry.prefab, spawnPosition, spawnRotation);
            return newCanvasGO; // Return GameObject containing the Canvas component
        }

        Debug.LogError($"Canvas prefab with key '{key}' not found!");
        return null;
    }

    // Helper method to search for an existing canvas in the scene.
    private Canvas FindExistingCanvasInScene(string key)
    {
        // Find all canvases in the scene and check for the one with the matching key.
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.InstanceID);
        foreach (var canvas in canvases)
        {
            if (canvas.gameObject.name == key)
            {
                return canvas; // Return the existing canvas if found.
            }
        }
        return null; // Return null if no matching canvas is found.
    }
}
