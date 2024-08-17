using UnityEngine;
using UnityEngine.UI;

public class FusionsSlider : MonoBehaviour
{
    [SerializeField] int maxFusions;
    public Slider fusionsSlider;

    void Start()
    {
        fusionsSlider = GetComponent<Slider>();
        fusionsSlider.minValue = 0;
        fusionsSlider.maxValue = maxFusions;
    }

    void Update()
    {
        int mostFusions = CalculateCombinations(ParticleManager.Instance.LargestParticle.GetComponent<Particle>());
        fusionsSlider.value = mostFusions;

        if (fusionsSlider.value > maxFusions)
        {
            Debug.Log("Win!");
        }
    }

    int CalculateCombinations(Particle particle)
    {
        int numberOfCombinations = (int)(Mathf.Log(particle.mass / particle.stats.defaultMass) / Mathf.Log(particle.stats.growthMultiplier) + 1);

        if (numberOfCombinations == 0)
        {
            Debug.Log("Cant return zero");
            return 1;
        }
        return numberOfCombinations;
    }
}
