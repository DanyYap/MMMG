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
        public GameObject prefab;  // The prefab (GameObject) for this canvas type.
    }

    [SerializeField] private List<CanvasEntry> canvasPrefabs;  // List of available canvas prefabs.
    private Dictionary<string, GameObject> canvasInstances;  // Dictionary to store created canvas instances.

    private void OnEnable()
    {
        // Initialize the dictionary to hold canvas instances.
        canvasInstances = new Dictionary<string, GameObject>();
    }

    public override GameObject CreateInstance(string key, Vector3 position, Quaternion rotation)
    {
        // Check if the canvas for this key has already been created.
        if (canvasInstances.ContainsKey(key))
        {
            Debug.LogWarning($"Canvas with key '{key}' already instantiated. Returning the existing instance.");
            return canvasInstances[key]; // Return GameObject with Canvas attached.
        }

        // Find the corresponding canvas prefab for this key.
        var entry = canvasPrefabs.Find(c => c.key == key);
        if (entry != null)
        {
            // Instantiate and store the canvas GameObject.
            var newCanvasObject = Instantiate(entry.prefab, position, rotation);
            canvasInstances.Add(key, newCanvasObject);

            // Ensure the GameObject has the Canvas component.
            if (newCanvasObject.GetComponent<Canvas>() == null)
            {
                Debug.LogError($"The prefab for key '{key}' does not contain a Canvas component!");
                return null;
            }

            return newCanvasObject; // Return GameObject with Canvas attached.
        }

        Debug.LogError($"Canvas prefab with key '{key}' not found!");
        return null;
    }
}
