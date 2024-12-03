using UnityEngine;

// Interface to ensure factories can create objects of a specific type.
public interface IFactory<T> where T : Object
{
    // Creates a GameObject using a key, position, and rotation, and assigns the T component to it.
    // If position or rotation is not assigned, they default to (0,0,0) for position, and no rotation.
    GameObject CreateInstance(string key, Vector3? position = null, Quaternion? rotation = null);
}

// Base class for factories. Ensures all factories define a CreateInstance method.
public abstract class ScriptableFactoryBase<T> : ScriptableObject, IFactory<T> where T : Object
{
    public abstract GameObject CreateInstance(string key, Vector3? position = null, Quaternion? rotation = null);
}
