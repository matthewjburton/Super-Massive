using UnityEngine;

public class ParticleStats : ScriptableObject
{
    [Header("Mass")]
    [Min(0), Tooltip("The default mass of the particle")]
    public float defaultMass;

    [Min(0), Tooltip("How much a particle grows when fusing")]
    public float sizeMultiplier;

    [Header("Speed")]
    [Min(0), Tooltip("The default speed of the particle")]
    public float defaultSpeed;

    [Min(0), Tooltip("Duration to lerp to target speed")]
    public float lerpDuration;

    [Header("Color")]
    [Tooltip("The default color of the particle")]
    public Color defaultColor;

    [Header("Invincibility")]
    [Tooltip("How long a particle is invincible after being reduced")]
    public float invincibilityDuration;

    [Header("Sounds")]
    public AudioClip[] fusionSounds;

    [Header("Spawning")]
    [Range(0, 90), Tooltip("Angle in degrees the particle can deviate from directly towards the center of the screen")]
    public float maxAngleDeviation;
}