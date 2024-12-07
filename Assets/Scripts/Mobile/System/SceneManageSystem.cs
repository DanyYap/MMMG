using UnityEngine;

public class SceneManageSystem : MonoBehaviour
{
    public static SceneManageSystem Instance { get; private set; }

    private ISceneManager sceneManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateSystem()
    {
        sceneManager = new SceneManagerCustom();
    }

    public ISceneManager GetSceneManager()
    {
        return sceneManager;
    }
}
