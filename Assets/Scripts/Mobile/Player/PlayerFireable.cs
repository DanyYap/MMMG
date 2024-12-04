// Class for player fire behavior
using UnityEngine;

public class PlayerFireable : FireBase
{
    private PlayerController self;

    private void Awake()
    {
        self = GetComponentInParent<PlayerController>();
    }

    public override void Ignite()
    {
        StartCoroutine(FadeInFireEffect());

        // Set player burning state
        PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
            flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsBurning = flag, true);
    }

    public override void Extinguish()
    {
        if (fire != null)
        {
            StartCoroutine(FadeOutFireEffect());

            // Reset player burning state
            PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
                flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsBurning = flag, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleFireInteraction(other); // Add additional behavior specific to ObjectFireable.
    }

    private void HandleFireInteraction(Collider other)
    {
        if (!self.PlayerState.IsBurning) return;

        if (other.TryGetComponent<IFireable>(out var fireable))
        {
            fireable.Ignite();
        }
        else
        {
            var fireablesInChildren = other.GetComponentsInChildren<IFireable>();
            if (fireablesInChildren.Length > 0)
            {
                fireablesInChildren[0].Ignite();
            }
        }
    }

    public override void Interact()
    {
        
    }
}