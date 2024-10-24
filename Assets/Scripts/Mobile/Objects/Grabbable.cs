using System;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    void OnGrab(); // Method to be called when the object is grabbed
    void OnRelease(); // Method to be called when the object is released
}

[RequireComponent(typeof(Collider))]
public class Grabbable : MonoBehaviour, IGrabbable
{
    public event Action OnGrabEvent;
    public event Action OnReleaseEvent;

    // self
    private Collider selfCollider;

    // self parent
    private GameObject selfParent;
    private Outline parentOutline;

    // target
    private Transform playerHand;
    private PlayerController owner;
    
    private void Awake()
    {
        OnGrabEvent += OnGrab;
        OnReleaseEvent += OnRelease;

        selfCollider = GetComponent<Collider>();
        selfCollider.isTrigger = true;

        selfParent = transform.parent?.gameObject;
        if (selfParent == null) selfParent = gameObject;
        parentOutline = selfParent.GetComponent<Outline>();
        OutlineObject(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && owner == null && !player.PlayerState.IsGrabbing)
        {
            playerHand = other.transform;
            InterfaceManageSystem.Instance.UpdateInteractableObject(this);
            OutlineObject(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && owner == null && !player.PlayerState.IsGrabbing)
        {
            InterfaceManageSystem.Instance.UpdateInteractableObject(null);
            OutlineObject(false);
        }
    }

    public void Interact()
    {
        OutlineObject(false);

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
        selfParent.GetComponent<Collider>().isTrigger = false;

        selfParent.transform.SetParent(playerHand);
        selfParent.transform.localPosition = Vector3.zero;
        selfParent.transform.localRotation = Quaternion.identity;

        PlayerSwitcher.SelectedPlayer.ObjectOnInteract = this;
    }

    private void DetachFromPlayerHand()
    {
        selfParent.GetComponent<Collider>().isTrigger = true;

        selfParent.transform.SetParent(null);

        PlayerSwitcher.SelectedPlayer.ObjectOnInteract = null;
    }

    private void SetPlayerGrabbingState(bool isGrabbing)
    {
        PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
            flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsGrabbing = flag,
            isGrabbing);
    }

    private void OutlineObject(bool enable)
    {
        if (parentOutline == null) return;

        if (enable)
        {
            parentOutline.enabled = true;

            parentOutline.OutlineMode = Outline.Mode.OutlineVisible;

            // Create a new color instance and modify it
            Color outlineColor = Color.red;
            outlineColor.a = 0.5f; // Set the alpha value

            // Assign the modified color back to OutlineColor
            parentOutline.OutlineColor = outlineColor;

            parentOutline.OutlineWidth = 7.5f;
        }
        else
        {
            parentOutline.enabled = false;
        }
    }
}