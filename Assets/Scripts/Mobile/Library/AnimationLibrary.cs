using UnityEngine;

[CreateAssetMenu(fileName = "AnimationLibrary", menuName = "Scriptable Objects/AnimationLibrary")]
public class AnimationLibrary : ScriptableObject
{
    [Header("Player Animation")]
    public AnimationClip PlayerIdle;
    public AnimationClip PlayerRun;
}
