using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] float baseCooldownTime;
    [SerializeField] float voidCooldownMultiplier = 2f; // Multiplier to increase cooldown in void environment
    [SerializeField] float maxAngleDeviation = 45f; // Angle in degrees
    [SerializeField] float spawnOffset = 1.5f; // Multiplier for offsetting particles beyond the edge

    bool onCooldown = false;

    // Update is called once per frame
    void Update()
    {
        SpawnParticle();
    }

    void SpawnParticle()
    {
        if (onCooldown)
            return;

        Vector3 spawnPosition = GetRandomPositionOffCameraEdge();
        GameObject newParticle = Instantiate(particle, spawnPosition, Quaternion.identity);

        if (newParticle.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 direction = GetRandomDirectionTowardsCamera(spawnPosition);
            float speed = newParticle.GetComponent<Particle>().speed;
            rb.velocity = direction * speed;
        }

        StartCoroutine(nameof(Cooldown));
    }

    IEnumerator Cooldown()
    {
        onCooldown = true;

        // Get the current environment from the EnvironmentManager
        var environmentManager = EnvironmentManager.Instance;
        float cooldownTime = baseCooldownTime;

        if (environmentManager != null && environmentManager.currentEnvironment == EnvironmentManager.Environment.Void)
        {
            cooldownTime *= voidCooldownMultiplier; // Increase cooldown in Void environment
        }

        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    Vector2 GetRandomDirectionTowardsCamera(Vector3 spawnPosition)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector2 directionToCamera = (cameraPosition - spawnPosition).normalized;

        // Generate a random angle within the specified range
        float angle = Random.Range(-maxAngleDeviation, maxAngleDeviation);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Rotate the direction vector by the random angle
        Vector2 randomDirection = rotation * directionToCamera;

        return randomDirection.normalized;
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

}
