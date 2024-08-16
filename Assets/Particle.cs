using System;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float mass;
    public float speed;
    [SerializeField] float growthModifer;

    public static event Action<float> OnMassChanged; // Event for mass change

    void Start()
    {
        mass = transform.localScale.x;
        OnMassChanged?.Invoke(mass);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Particle particle))
        {
            if (particle.mass.Equals(mass))
            {
                CombineParticles(other.gameObject);
            }
        }
    }

    void CombineParticles(GameObject other)
    {
        Destroy(other);

        transform.localScale *= growthModifer;
        mass = transform.localScale.x;
        speed /= mass;

        // Notify listeners about the mass change
        OnMassChanged?.Invoke(mass);
    }
}
