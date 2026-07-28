using UnityEngine;

[CreateAssetMenu(fileName = "Anti Matter Stats", menuName = "ScriptableObjects/Anti Matter Stats")]
public class AntiMatterStats : ParticleStats
{
    [Header("Sounds")]
    public AudioClip[] destroySounds; // sounds for destroying Matters
    public AudioClip[] reduceSounds; // sounds for reducing Matters

    [Header("Death")]
    public GameObject destroyMatter;
}