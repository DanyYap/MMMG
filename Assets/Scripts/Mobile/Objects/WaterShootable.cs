using System;
using UnityEngine;

public interface IWaterShootable
{
    void ShootingWater();
    void StopShooting();
}

public class WaterShootable : MonoBehaviour, IWaterShootable, IInteractable
{
    public ParticleSystem WaterParticleSystem;
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

        waterShootingEvent -= ShootingWater;
        waterShootingEvent += StopShooting;
    }

    public void StopShooting()
    {
        WaterParticleSystem.Stop();

        waterShootingEvent -= StopShooting;
        waterShootingEvent += ShootingWater;
    }

    private void InitializeButton()
    {
        var gameButton = GameObject.Find(ButtonIdentifiers.UseToolButton);

        if (gameButton != null)
        {
            InterfaceManageSystem.Instance.GetInputManager().SetNewInteractAction(this, 1);
            gameButton.SetActive(true);
        }

        initializeButtonEvent -= InitializeButton;
        initializeButtonEvent += ResetButton;
    }

    private void ResetButton()
    {
        var gameButton = GameObject.Find(ButtonIdentifiers.UseToolButton);

        if (gameButton != null)
        {
            InterfaceManageSystem.Instance.GetInputManager().SetNewInteractAction(null, 1);
            gameButton.SetActive(false);
        }

        initializeButtonEvent -= ResetButton;
        initializeButtonEvent += InitializeButton;
    }
}
