using UnityEngine;

[CreateAssetMenu(fileName = "Particle Settings", menuName = "ScriptableObjects/Particle Settings")]
public class ParticleSettings : ScriptableObject
{
    [Header("Mass")]
    [Min(0), Tooltip("The default mass of the particle")]
    public float defaultMass;

    [Min(0), Tooltip("The current mass of the particle")]
    public float currentMass;

    [Header("Speed")]
    [Min(0), Tooltip("The default speed of the particle")]
    public float defaultSpeed;

    [Min(0), Tooltip("The current speed of the particle")]
    public float currentSpeed;

    [Header("Color")]
    [Tooltip("The default color of the particle")]
    public Color defaultColor;

    [Tooltip("The current color of the particle")]
    public Color currentColor;
}