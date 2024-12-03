using UnityEngine;

[CreateAssetMenu(fileName = "ParticleEffectsLibrary", menuName = "Scriptable Objects/ParticleEffectsLibrary")]
public class ParticleEffectsLibrary : ScriptableObject
{
    [Header("TargetFollower")]
    public GameObject Fire;
    public GameObject Water;
}
