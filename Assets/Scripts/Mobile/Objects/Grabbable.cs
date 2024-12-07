using System;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    void OnGrab(); // Called when the object is grabbed
    void OnRelease(); // Called when the object is released
}

[RequireComponent(typeof(Collider))]
public class Grabbable : MonoBehaviour, IGrabbable
{
    public event Action OnGrabEvent;
    public event Action OnReleaseEvent;
    private event Action customEvent;

    // Self references
    public bool isSelf = true;
    private Collider grabbableCollider;
    private GameObject parentObject;
    private Rigidbody parentRigidbody;
    private Outline parentOutline;

    // Player references
    private Transform playerHand;
    private PlayerController owner;

    private void Awake()
    {
        // Initialize self properties
        grabbableCollider = GetComponent<Collider>();
        grabbableCollider.isTrigger = true;
        parentObject = transform.parent?.gameObject ?? gameObject;

        // Initialize parent object references
        parentRigidbody = parentObject.GetComponent<Rigidbody>();
        parentOutline = parentObject.GetComponent<Outline>();

        // Set up outline visibility
        SetOutline(false);
    }

    private void OnEnable()
    {
        // Subscribe to events
        OnGrabEvent += OnGrab;
        OnReleaseEvent += OnRelease;
    }

    private void OnDisable()
    {
        // Unsubscribe to events
        OnGrabEvent -= OnGrab;
        OnReleaseEvent -= OnRelease;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null && owner == null && !player.PlayerState.IsGrabbing)
        {
            playerHand = player.playerGrabPoint;
            InterfaceManageSystem.Instance.GetButtonManager().SetButtonAction(UiElementNames.Buttons.Interact, new InteractObjectAction(this));
            SetOutline(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null && owner == null && !player.PlayerState.IsGrabbing)
        {
            InterfaceManageSystem.Instance.GetButtonManager().SetButtonAction(UiElementNames.Buttons.Interact);
            SetOutline(false);
        }
    }

    public void Interact()
    {
        SetOutline(false);

        if (owner == null || PlayerSwitcher.SelectedPlayer == owner)
        {
            if (PlayerSwitcher.SelectedPlayer.PlayerState.IsGrabbing)
            {
                OnReleaseEvent?.Invoke();
            }
            else
            {
                OnGrabEvent?.Invoke();
            }
        }
    }

    public void OnGrab()
    {
        owner = PlayerSwitcher.SelectedPlayer;
        SetGrabbingState(true);
        AttachToPlayerHand();

        customEvent?.Invoke();
    }

    public void OnRelease()
    {
        SetGrabbingState(false);
        DetachFromPlayerHand();
        owner = null;

        customEvent?.Invoke();
        customEvent = null;
    }

    public void AssignNewEvent(Action newEvent)
    {
        customEvent = newEvent;
    }

    private void AttachToPlayerHand()
    {
        // Disable physics interactions during grabbing
        if (parentRigidbody != null)
        {
            parentRigidbody.isKinematic = true;
            parentRigidbody.detectCollisions = false;
        }

        parentObject.transform.SetParent(playerHand);
        parentObject.transform.localPosition = Vector3.zero;
        parentObject.transform.localRotation = Quaternion.identity;

        PlayerSwitcher.SelectedPlayer.ObjectOnInteract = this;
    }

    private void DetachFromPlayerHand()
    {
        // Detach the object and re-enable physics
        Vector3 detachDirection = playerHand.forward * -1f;
        parentObject.transform.position += detachDirection;

        if (parentRigidbody != null)
        {
            parentRigidbody.isKinematic = false;
            parentRigidbody.detectCollisions = true;
        }

        parentObject.transform.SetParent(null);
        PlayerSwitcher.SelectedPlayer.ObjectOnInteract = null;
    }

    private void SetGrabbingState(bool isGrabbing)
    {
        PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
            flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsGrabbing = flag,
            isGrabbing);
    }

    private void SetOutline(bool enable)
    {
        if (parentOutline == null) return;

        parentOutline.enabled = enable;
        if (enable)
        {
            parentOutline.OutlineMode = Outline.Mode.OutlineVisible;
            parentOutline.OutlineColor = new Color(1f, 0f, 0f, 0.5f);
            parentOutline.OutlineWidth = 7.5f;
        }
    }
}
