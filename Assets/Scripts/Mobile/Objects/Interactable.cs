using System;
using UnityEngine;

public interface IInteractable
{
    void Interact(); // Method to be called on interaction
    void OnGrab(); // Method to be called when the object is grabbed
    void OnRelease(); // Method to be called when the object is released
    void OnThrow(); // Method to be called when the object is thrown
}


public class InteractableObject : MonoBehaviour, IInteractable
{
    public event Action OnGrabEvent;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void Awake()
    {
        OnGrabEvent += OnGrab;
    }

    public void Interact()
    {
        OnGrabEvent.Invoke();
    }

    public void OnGrab()
    {
        OnGrabEvent -= OnGrab;
        OnGrabEvent += OnRelease;
        
        Debug.Log("Object grabbed");
    }

    public void OnRelease()
    {
        OnGrabEvent -= OnRelease;
        OnGrabEvent += OnGrab;

        Debug.Log("Object released");
    }

    public void OnThrow()
    {
        throw new System.NotImplementedException();
    }
}
