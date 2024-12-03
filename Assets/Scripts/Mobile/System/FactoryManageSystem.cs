using UnityEngine;

public class FactoryManageSystem : MonoBehaviour
{
    public static FactoryManageSystem Instance { get; private set; }

    public CanvasFactory CanvasFactory;
    public ParticleSystemFactory ParticleSystemFactory;

    public AnimationLibrary AnimationLibrary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
