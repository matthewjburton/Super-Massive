using System.Collections;
using System.Linq;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }
    public GameObject LargestParticle { get; private set; }
    [SerializeField] GameObject particle;
    [SerializeField] float baseCooldownTime;
    [SerializeField] float spawnOffset; // Multiplier for offsetting particles beyond the edge

    bool onCooldown;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnParticle();
        UpdateLargestParticle();
    }

    void SpawnParticle()
    {
        if (onCooldown)
            return;

        Instantiate(particle, GetRandomPositionOffCameraEdge(), Quaternion.identity);
        StartCoroutine(nameof(Cooldown));
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;

        // Get the current environment from the EnvironmentManager
        var environmentManager = EnvironmentManager.Instance;
        float cooldownTime = baseCooldownTime;

        cooldownTime *= environmentManager.environment.spawnModifier;

        yield return new WaitForSeconds(cooldownTime);

        onCooldown = false;
    }

    Vector3 GetRandomPositionOffCameraEdge()
    {
        // Get the camera's dimensions in world space
        float screenAspect = Camera.main.aspect;
        float cameraHeight = Camera.main.orthographicSize * spawnOffset;
        float cameraWidth = Camera.main.orthographicSize * 2 * screenAspect;

        cameraWidth *= spawnOffset;

        // Randomly choose one of the four edges
        int edge = Random.Range(0, 4);
        Vector3 position = Vector3.zero;

        switch (edge)
        {
            case 0: // Top edge
                position = new Vector3(Random.Range(-cameraWidth / 2, cameraWidth / 2), cameraHeight, 0);
                break;
            case 1: // Bottom edge
                position = new Vector3(Random.Range(-cameraWidth / 2, cameraWidth / 2), -cameraHeight, 0);
                break;
            case 2: // Left edge
                position = new Vector3(-cameraWidth / 2, Random.Range(-cameraHeight, cameraHeight), 0);
                break;
            case 3: // Right edge
                position = new Vector3(cameraWidth / 2, Random.Range(-cameraHeight, cameraHeight), 0);
                break;
        }
        position += Camera.main.transform.position;
        position.z = 0;

        return position;
    }

    void UpdateLargestParticle()
    {
        // Find the largest particle currently on screen
        var particles = FindObjectsOfType<Particle>()
            .Where(p => IsInView(p.transform.position)) // Filter to only those in view
            .OrderByDescending(p => p.mass) // Order by mass, descending
            .FirstOrDefault();

        LargestParticle = particles?.gameObject;
    }

    bool IsInView(Vector3 worldPosition)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z > 0; // z > 0 means the object is in front of the camera
    }
}
