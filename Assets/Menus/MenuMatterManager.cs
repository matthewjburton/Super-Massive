using System.Collections;
using UnityEngine;

public class MenuMatterManager : MonoBehaviour
{
    [SerializeField] GameObject menuMatter;
    [SerializeField] float baseCooldownTime;
    [SerializeField] float spawnOffset; // Multiplier for offsetting Matters beyond the edge

    bool onCooldown;

    // Update is called once per frame
    void Update()
    {
        SpawnMatter();
    }

    void SpawnMatter()
    {
        if (onCooldown)
            return;

        Instantiate(menuMatter, GetRandomPositionOffCameraEdge(), Quaternion.identity);
        StartCoroutine(nameof(Cooldown));
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;

        yield return new WaitForSeconds(baseCooldownTime);

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
