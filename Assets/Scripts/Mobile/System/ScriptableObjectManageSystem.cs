using UnityEngine;

public class ScriptableObjectManageSystem : MonoBehaviour
{
    public static ScriptableObjectManageSystem Instance { get; private set; }

    public CanvasScriptableObject CanvasLibrary;
    public ParticleEffectsScriptableObject EffectsLibrary;

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
