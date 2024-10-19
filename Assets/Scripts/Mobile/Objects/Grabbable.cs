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
    public event Action OnReleaseEvent;

    private Transform playerHand;
    private PlayerController owner;

    private void Awake()
    {
        OnGrabEvent += OnGrab;
        OnReleaseEvent += OnRelease;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && owner == null && !player.PlayerState.IsGrabbing)
        {
            playerHand = other.transform;
            InterfaceManageSystem.Instance.UpdateInteractableObject(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (owner == null)
        {
            InterfaceManageSystem.Instance.UpdateInteractableObject(null);
        }
    }

    public void Interact()
    {
        if (owner == null || PlayerSwitcher.SelectedPlayer == owner)
        {
            if (PlayerSwitcher.SelectedPlayer.PlayerState.IsGrabbing)
            {
                OnReleaseEvent.Invoke();
            }
            else
            {
                OnGrabEvent.Invoke();
            }
        }
    }

    public void OnGrab()
    {
        owner = PlayerSwitcher.SelectedPlayer; // Assign the owner
        SetPlayerGrabbingState(true);
        AttachToPlayerHand();
        Debug.Log("grab");
    }

    public void OnRelease()
    {
        SetPlayerGrabbingState(false);
        DetachFromPlayerHand();
        owner = null; // Clear the owner on release
        Debug.Log("release");
    }

    private void AttachToPlayerHand()
    {
        transform.SetParent(playerHand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        PlayerSwitcher.SelectedPlayer.ObjectOnInteract = this;
    }

    private void DetachFromPlayerHand()
    {
        transform.SetParent(null);

        PlayerSwitcher.SelectedPlayer.ObjectOnInteract = null;
    }

    private void SetPlayerGrabbingState(bool isGrabbing)
    {
        PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
            flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsGrabbing = flag,
            isGrabbing);
    }
}