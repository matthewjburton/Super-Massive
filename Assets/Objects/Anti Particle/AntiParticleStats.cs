using UnityEngine;

[CreateAssetMenu(fileName = "Anti Particle Stats", menuName = "ScriptableObjects/Anti Particle Stats")]
public class AntiParticleStats : ScriptableObject
{
    [Header("Mass")]
    [Min(0), Tooltip("The default mass of the particle")]
    public float defaultMass;

    [Min(0), Tooltip("How much a particle grows when fusing")]
    public float growthMultiplier;

    [Header("Speed")]
    [Min(0), Tooltip("The default speed of the particle")]
    public float defaultSpeed;

    [Min(0), Tooltip("Duration to lerp to target speed")]
    public float lerpDuration;

    [Range(0, 1), Tooltip("How much a particle's speed changes when fusing")]
    public float speedMultiplier;

    [Header("Color")]
    [Tooltip("The default color of the particle")]
    public Color defaultColor;

    [Header("Sounds")]
    public AudioClip[] fusionSounds;
    public AudioClip[] destroySounds; // sounds for destroying particles
    public AudioClip[] reduceSounds; // sounds for reducing particles

    [Header("Spawning")]
    [Range(0, 90), Tooltip("Angle in degrees the particle can deviate from directly towards the center of the screen")]
    public float maxAngleDeviation;

    [Header("Death")]
    public GameObject destroyParticle;
}