using System.Collections;
using UnityEngine;

// This interface defines methods for anything that can catch fire
public interface IFireable : IInteractable
{
    void Ignite(); // Method to start fire
    void Extinguish(); // Method to extinguish fire
}

public class FireFactory
{
    // Factory Method to create fire particles
    public GameObject CreateFireParticle(GameObject target, GameObject fireParticle)
    {
        GameObject fireInstance = Object.Instantiate(
            fireParticle,
            target.transform.position,
            Quaternion.Euler(-90, 0, 0),
            target.transform);

        fireInstance.transform.localPosition = Vector3.zero; // Make sure fire is on the object
        return fireInstance;
    }
}

// This class handles the fire behavior
public class Fireable : MonoBehaviour, IFireable
{
    [SerializeField] private bool isPlayer = false;

    private ParticleEffectsScriptableObject effects;
    private FireFactory fireFactory; // Manages fire particles
    private GameObject fire; // Tracks the fire particles
    private ParticleSystem fireParticleSystem; // Access particle system directly

    private void Awake()
    {
        effects = ScriptableObjectManageSystem.Instance.EffectsLibrary;
        fireFactory = new FireFactory(); // Initialize fire particle manager
    }

    private void Start()
    {
        if (!isPlayer)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        if (fire == null) // Create fire particles if none exist
        {
            CreateFireParticle();
        }
        else
        {
            StartCoroutine(FadeInFireEffect()); // Gradually activate existing fire particles
        }

        if (isPlayer)
        {
            PlayerController playerController = GetComponentInParent<PlayerController>();
            PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
            flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsBurning = flag, true);
        }
    }

    public void Extinguish()
    {
        if (fire != null) // Check if an active fire exists
        {
            StartCoroutine(FadeOutFireEffect()); // Gradually deactivate fire particles
        }

        if (isPlayer)
        {
            PlayerController playerController = GetComponentInParent<PlayerController>();
            PlayerSwitcher.SelectedPlayer.PlayerState.SetState(
            flag => PlayerSwitcher.SelectedPlayer.PlayerState.IsBurning = flag, false);
        }
    }

    private void CreateFireParticle()
    {
        fire = fireFactory.CreateFireParticle(gameObject, effects.Fire); // Instantiate fire particles
        fireParticleSystem = fire.GetComponent<ParticleSystem>(); // Get the ParticleSystem component
    }

    private IEnumerator FadeInFireEffect()
    {
        fire.SetActive(true); // Activate the fire particles GameObject
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        // Gradually increase the alpha value to 1
        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Play(); // Start the particle system
    }

    private IEnumerator FadeOutFireEffect()
    {
        ParticleSystem.MainModule mainModule = fireParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        // Gradually decrease the alpha value to 0
        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            startColor.a = t; // Set alpha
            mainModule.startColor = startColor; // Apply changes
            yield return null; // Wait until the next frame
        }
        fireParticleSystem.Stop(); // Stop the particle system
        fire.SetActive(false); // Disable the fire particles GameObject
    }

    // Triggered when something enters the fire area
    private void OnTriggerEnter(Collider other)
    {
        // if self is player
        if (isPlayer)
        {
            // if self is burning
            PlayerController playerController = GetComponentInParent<PlayerController>();
            if (playerController.PlayerState.IsBurning)
            {

                // then others will be burnt as well
                if (other.TryGetComponent<IFireable>(out var fireable))
                {
                    Ignite();
                }
                else
                {
                    // Try to find IFireable in the children of the other GameObject
                    var fireablesInChildren = other.GetComponentsInChildren<IFireable>();

                    // Check if any fireable components were found in children
                    if (fireablesInChildren.Length > 0)
                    {
                        // Optionally, you can choose to ignite the first found fireable in the children
                        fireablesInChildren[0].Ignite(); // Call Ignite on the first found fireable
                    }
                }
            }
        }

        // if self is object
        else
        {
            if (other.TryGetComponent<IFireable>(out var fireable))
            {
                Ignite();
            }
            else
            {
                // Try to find IFireable in the children of the other GameObject
                var fireablesInChildren = other.GetComponentsInChildren<IFireable>();

                // Check if any fireable components were found in children
                if (fireablesInChildren.Length > 0)
                {
                    // Optionally, you can choose to ignite the first found fireable in the children
                    fireablesInChildren[0].Ignite(); // Call Ignite on the first found fireable
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Extinguish(); // Extinguish fire when exiting
    }

    // Interact method from IInteractable
    public void Interact()
    {
        // Implementation of interaction logic (if needed)
    }
}