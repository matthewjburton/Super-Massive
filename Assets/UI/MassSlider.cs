using UnityEngine;
using UnityEngine.UI;

public class MassSlider : MonoBehaviour
{
    [SerializeField] float criticalMass;
    [SerializeField] float totalMass;
    [SerializeField] float massModifier;
    public Slider massSlider;

    void Start()
    {
        massSlider = GetComponent<Slider>();
        massSlider.minValue = 0;
        massSlider.maxValue = criticalMass;
    }

    void HandleMassChanged(float mass)
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
        float maxMass = 0;

        foreach (GameObject particle in particles)
        {
            if (IsInView(particle))
            {
                float particleMass = 0;
                if (particle.TryGetComponent(out Particle particleScript))
                    particleMass = particleScript.mass;

                if (particle.TryGetComponent(out AntiParticle antiParticleScript))
                    particleMass = antiParticleScript.mass;

                if (particleMass > maxMass)
                {
                    maxMass = particleMass;
                }
            }
        }

        massSlider.value = maxMass;

        if (maxMass > criticalMass)
        {
            Debug.Log("Create a blackhole!");
        }
    }

    bool IsInView(GameObject particle)
    {
        // Get the main camera
        Camera cam = Camera.main;

        // Convert the particle's position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(particle.transform.position);

        // Check if the particle is within the camera's view
        bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                        viewportPoint.z > 0; // Ensure the particle is in front of the camera

        return isInView;
    }

    void OnEnable()
    {
        Particle.OnMassChanged += HandleMassChanged;
        AntiParticle.OnMassChanged += HandleMassChanged;
    }

    void OnDisable()
    {
        Particle.OnMassChanged -= HandleMassChanged;
        AntiParticle.OnMassChanged -= HandleMassChanged;
    }
}
