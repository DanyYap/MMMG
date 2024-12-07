using System;
using UnityEngine;

public interface IWaterShootable: IInteractable
{
    void ShootingWater();
    void StopShooting();
}

public class WaterShootable : MonoBehaviour, IWaterShootable
{
    public ParticleSystem WaterParticleSystem;
    public AudioSource ExtinguishSoundEffect;
    private event Action waterShootingEvent;
    private event Action initializeButtonEvent;
    [SerializeField] private Grabbable grabbableObject;

    private void Awake()
    {
        waterShootingEvent += ShootingWater;
        initializeButtonEvent += InitializeButton;

        if (grabbableObject == null) return;
        grabbableObject.AssignNewEvent(initializeButtonEvent);
    }

    public void Interact()
    {
        waterShootingEvent.Invoke();
    }

    public void ShootingWater()
    {
        WaterParticleSystem.Play();
        ExtinguishSoundEffect.Play();

        waterShootingEvent -= ShootingWater;
        waterShootingEvent += StopShooting;
    }

    public void StopShooting()
    {
        WaterParticleSystem.Stop();
        ExtinguishSoundEffect.Stop();

        waterShootingEvent -= StopShooting;
        waterShootingEvent += ShootingWater;
    }

    private void InitializeButton()
    {
        var gameButton = GameObject.Find(UiElementNames.Buttons.UseTool);

        if (gameButton != null)
        {
            InterfaceManageSystem.Instance.GetButtonManager().SetButtonAction(UiElementNames.Buttons.UseTool, new InteractObjectAction(this));
        }

        initializeButtonEvent -= InitializeButton;
        initializeButtonEvent += ResetButton;
    }

    private void ResetButton()
    {
        var gameButton = GameObject.Find(UiElementNames.Buttons.UseTool);

        if (gameButton != null)
        {
            InterfaceManageSystem.Instance.GetButtonManager().SetButtonAction(UiElementNames.Buttons.UseTool);
        }

        initializeButtonEvent -= ResetButton;
        initializeButtonEvent += InitializeButton;
    }
}
