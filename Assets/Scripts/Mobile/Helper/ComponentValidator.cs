using System;
using UnityEngine;

public static class ComponentValidator
{
    public static void ValidateNotNull<T>(T obj, string paramName)
    {
        if (obj == null)
        {
            Debug.LogError($"{paramName} cannot be null.");
            throw new ArgumentNullException(paramName);
        }
    }

    public static void ValidateNotNull<T>(T obj)
    {
        if (obj == null)
        {
            Debug.LogError($"{obj} cannot be null.");
            throw new ArgumentNullException(obj.ToString());
        }
    }
}
