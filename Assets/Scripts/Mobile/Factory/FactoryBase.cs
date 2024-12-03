using UnityEngine;

// Interface to ensure factories can create objects of a specific type.
public interface IFactory<T> where T : Object
{
    // Creates a GameObject using a key, position, and rotation, and assigns the T component to it.
    GameObject CreateInstance(string key, Vector3 position, Quaternion rotation);
}


// Base class for factories. Ensures all factories define a CreateInstance method.
public abstract class ScriptableFactoryBase<T> : ScriptableObject, IFactory<T> where T : Object
{
    public abstract GameObject CreateInstance(string key, Vector3 position, Quaternion rotation);
}
