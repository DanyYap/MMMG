using System;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    void OnGrab(); // Method to be called when the object is grabbed
    void OnRelease(); // Method to be called when the object is released
}

public class Grabbable : MonoBehaviour, IGrabbable
{
    public event Action OnGrabEvent;
    private Transform playerHand;

    private void Awake()
    {
        OnGrabEvent += OnGrab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Transform playerHandChild = other.transform.Find("Player Hand");
            Transform playerHandChild = other.transform;
            if (playerHandChild != null)
            {
                playerHand = playerHandChild;
            }

            InterfaceManageSystem.Instance.UpdateInteractableObject(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InterfaceManageSystem.Instance.UpdateInteractableObject(null);
    }

    public void Interact()
    {
        OnGrabEvent.Invoke();
    }

    public void OnGrab()
    {
        OnGrabEvent -= OnGrab;
        OnGrabEvent += OnRelease;

        // Attach the object to the player's hand
        transform.SetParent(playerHand);
        transform.localPosition = Vector3.zero; // Adjust the position if needed
        transform.localRotation = Quaternion.identity; // Adjust the rotation if needed
        Debug.Log("grab");
    }

    public void OnRelease()
    {
        OnGrabEvent -= OnRelease;
        OnGrabEvent += OnGrab;

        // Detach the object from the player's hand
        transform.SetParent(null);
        Debug.Log("release");
    }
}
