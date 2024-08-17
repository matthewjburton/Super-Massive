using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "ScriptableObjects/Environment")]
public class Environment : ScriptableObject
{
    [Tooltip("The type of environment")]
    public EnvironmentType environment;

    [Min(0), Tooltip("The modifier applied to particle spawning. Larger numbers increase spawn delay. Zero turns off spawning")]
    public float spawnModifier;

    [Tooltip("The multiplie applied to particle movement")]
    public float speedMultiplier;

    [Tooltip("The baackground color of the camera")]
    public Color backgroundColor;

    [Tooltip("Whether or not to display the stars")]
    public bool showStars;
}

public enum EnvironmentType
{
    Default,
    Warm,
    Void
}