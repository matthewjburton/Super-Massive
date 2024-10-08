using UnityEngine;
using UnityEngine.UI;

public class FusionsSlider : MonoBehaviour
{
    [SerializeField] int maxFusions;
    public int mostFusions;
    public Slider fusionsSlider;
    public Slider mostFusionsSlider;

    void Start()
    {
        fusionsSlider.minValue = 0;
        fusionsSlider.maxValue = maxFusions;

        mostFusionsSlider.minValue = 0;
        mostFusionsSlider.maxValue = maxFusions;
    }

    void Update()
    {
        mostFusions = CalculateCombinations(ParticleManager.Instance.LargestParticle.GetComponent<Particle>());
        fusionsSlider.value = mostFusions;

        if (mostFusions > mostFusionsSlider.value)
            mostFusionsSlider.value = mostFusions;


        if (fusionsSlider.value > maxFusions)
        {
            fusionsSlider.maxValue++;
            mostFusionsSlider.maxValue++;
        }
    }

    int CalculateCombinations(Particle particle)
    {
        return (int)(Mathf.Log(particle.mass / particle.stats.defaultMass) / Mathf.Log(particle.stats.growthMultiplier));
    }
}
