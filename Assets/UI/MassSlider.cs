using UnityEngine;
using UnityEngine.UI;

public class MassSlider : MonoBehaviour
{
    [SerializeField] float criticalMass;
    [SerializeField] float totalMass;
    [SerializeField] float massModifier;
    Slider massSlider;

    void Start()
    {
        massSlider = GetComponent<Slider>();
        massSlider.minValue = 0;
        massSlider.maxValue = criticalMass;
    }

    void Update()
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
        float temporarySum = 0;

        foreach (GameObject particle in particles)
        {
            if (IsInView(particle))
                temporarySum += Mathf.Pow(particle.GetComponent<Particle>().mass, massModifier);
        }

        if (temporarySum > totalMass)
        {
            totalMass = temporarySum;
        }

        massSlider.value = totalMass;

        if (totalMass > criticalMass)
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
}
