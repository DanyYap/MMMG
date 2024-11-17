using UnityEngine;

public class ScriptableObjectManageSystem : MonoBehaviour
{
    public static ScriptableObjectManageSystem Instance { get; private set; }

    public CanvasLibrary CanvasLibrary;
    public ParticleEffectsLibrary EffectsLibrary;

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
