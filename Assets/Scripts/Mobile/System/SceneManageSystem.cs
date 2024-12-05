using UnityEngine;

public static class SceneNames
{
    public const string MenuScene = "MenuScene";
    public const string GameScene = "GameScene";
    public const string FireHoseScene = "FireHoseScene";

    public const string Level1Scene = "Level 1";
    public const string Level2Scene = "Level 2";
    public const string Level3Scene = "Level 3";
    public const string Level4Scene = "Level 4";
}

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
