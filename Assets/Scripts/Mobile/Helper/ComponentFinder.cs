using UnityEngine;

public static class ComponentFinder
{
    /// <summary>
    /// Finds a GameObject by name and gets the specified component from it.
    /// </summary>
    public static T GetComponentByName<T>(string gameObjectName) where T : Component
    {
        // Find GameObject by its name in the scene
        GameObject foundGameObject = GameObject.Find(gameObjectName);
        var foundComponent = foundGameObject != null ? foundGameObject.GetComponent<T>() : null;

        if (foundComponent == null)
        {
            Debug.LogError($"Component of type '{typeof(T)}' with name '{gameObjectName}' not found in the scene.");
        }

        return foundComponent;
    }
}