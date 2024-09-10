using UnityEngine;

[CreateAssetMenu(fileName = "Matter Stats", menuName = "ScriptableObjects/Matter Stats")]
public class MatterStats : ParticleStats
{
    [Header("Fusion Text")]
    public GameObject fusionTextPrefab;
    public string[] fusionTextList;
    public Color[] fusionColorList;
}