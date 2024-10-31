using UnityEngine;

// Interface for status management
public interface IStatus
{
    float CurrentStatusValue { get; set; }
}

public class ObjectStatus : MonoBehaviour, IStatus
{
    public float CurrentStatusValue { 
        get => InitialStatusValue; 
        set => throw new System.NotImplementedException(); 
    }

    [SerializeField] private float InitialStatusValue = 100f;

    private void Awake()
    {
        CurrentStatusValue = InitialStatusValue;
    }
}
