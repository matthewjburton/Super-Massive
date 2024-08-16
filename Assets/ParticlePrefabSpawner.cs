using UnityEngine;

public class ParticlePrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
        int numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 spawnPosition = particles[i].position;
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
