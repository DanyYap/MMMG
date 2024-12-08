using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game Settings/Level Settings", order = 1)]
public class GameLevelSettings : ScriptableObject
{
    [Header("Time")]
    public CountdownTimerSettings countdownTimerSettings;

    // Additional settings for the level can go here
    [Header("Other Level Settings")]
    public int levelDifficulty;
    public string levelName;
}
