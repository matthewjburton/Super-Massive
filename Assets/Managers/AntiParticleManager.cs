using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AntiParticleManager : MonoBehaviour
{
    [SerializeField] GameObject antiParticle;
    [SerializeField] float baseCooldownTime;
    [SerializeField] float spawnOffset; // Multiplier for offsetting particles beyond the edge

    bool onCooldown = false;

    [Header("Controlling when antimatter starts spawing")]
    [Range(0, 1), SerializeField, Tooltip("A decimal representing the percent of mass towards critical mass that the player must reach before spawning spawning antimatter")]
    float percentToSpawnAntimatter;
    [SerializeField] Slider massSlider;

    // Update is called once per frame
    void Update()
    {
        if (massSlider.value / massSlider.maxValue < percentToSpawnAntimatter)
            return;

        SpawnParticle();
    }

    void SpawnParticle()
    {
        if (onCooldown)
            return;

        Instantiate(antiParticle, GetRandomPositionOffCameraEdge(), Quaternion.identity);
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
}
