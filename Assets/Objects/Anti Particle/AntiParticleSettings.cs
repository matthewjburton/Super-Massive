using UnityEngine;

[CreateAssetMenu(fileName = "Anti Particle Settings", menuName = "ScriptableObjects/Anti Particle Settings")]
public class AntiParticleSettings : ScriptableObject
{
    [Header("Mass")]
    [Min(0), Tooltip("The default mass of the AntiParticle")]
    public float defaultMass;

    [Min(0), Tooltip("The current mass of the AntiParticle")]
    public float currentMass;

    [Header("Speed")]
    [Min(0), Tooltip("The default speed of the AntiParticle")]
    public float defaultSpeed;

    [Min(0), Tooltip("The current speed of the AntiParticle")]
    public float currentSpeed;

    [Header("Color")]
    [Tooltip("The default color of the AntiParticle")]
    public Color defaultColor;

    [Tooltip("The current color of the AntiParticle")]
    public Color currentColor;
}