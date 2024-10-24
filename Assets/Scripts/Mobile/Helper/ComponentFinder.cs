using UnityEngine;

public static class ComponentFinder
{
    // Generic method to find a component in the GameObject and its children
    public static T FindComponentInHierarchy<T>(GameObject gameObject) where T : Component
    {
        // Try to get the component from the current GameObject
        T component = gameObject.GetComponent<T>();

        // If the component is found, return it
        if (component != null)
        {
            return component;
        }

        // If not found, check in the children
        foreach (Transform child in gameObject.transform)
        {
            component = FindComponentInHierarchy<T>(child.gameObject);
            if (component != null)
            {
                return component; // Return if found in children
            }
        }

        // If not found in this GameObject or any children, return null
        return null;
    }

    // Method to get the very top parent component of a specific type
    public static T GetTopParentComponent<T>(GameObject gameObject) where T : Component
    {
        Transform currentTransform = gameObject.transform;

        // Traverse up the hierarchy until we reach the root
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        // Once we reach the topmost parent, try to get the component
        return currentTransform.GetComponent<T>();
    }
}