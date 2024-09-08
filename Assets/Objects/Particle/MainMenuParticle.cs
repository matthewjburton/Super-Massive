using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuParticle : MonoBehaviour
{
    public ParticleStats stats;
    public float mass;
    public float speed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        mass = stats.defaultMass;
        transform.localScale = new(mass, mass);

        speed = stats.defaultSpeed;
        Vector2 direction = GetRandomDirectionTowardsCamera(transform.position);
        rb.velocity = direction * stats.defaultSpeed;
    }

    Vector2 GetRandomDirectionTowardsCamera(Vector3 spawnPosition)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector2 directionToCamera = (cameraPosition - spawnPosition).normalized;

        // Generate a random angle within the specified range
        float angle = UnityEngine.Random.Range(-stats.maxAngleDeviation, stats.maxAngleDeviation);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Rotate the direction vector by the random angle
        Vector2 randomDirection = rotation * directionToCamera;

        return randomDirection.normalized;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (rb == null)
            return;

        rb.velocity = rb.velocity.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out MainMenuParticle particle))
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

        SoundManager.Instance.PlayRandomSound(stats.fusionSounds, transform, UnityEngine.Random.Range(Math.Abs((1 - mass) / 1), 1));

        Grow();

        GameObject fusionText = Instantiate(stats.fusionTextPrefab, transform.position, Quaternion.identity);
        int index = Mathf.Min(Mathf.RoundToInt(CalculateCombinations()) - 2, stats.fusionTextList.Length - 1);
        fusionText.GetComponentInChildren<TextMeshProUGUI>().text = stats.fusionTextList[index];
        fusionText.GetComponentInChildren<TextMeshProUGUI>().fontSize = 6 + (index * 2);
        fusionText.GetComponentInChildren<TextMeshProUGUI>().color = stats.fusionColorList[index];
    }

    float CalculateCombinations()
    {
        if (Mathf.Log(stats.growthMultiplier) == 0)
        {
            Debug.Log("Cant Divide by zero");
            return 1;
        }

        float numberOfCombinations = Mathf.Log(mass / stats.defaultMass) / Mathf.Log(stats.growthMultiplier) + 1;

        if (numberOfCombinations == 0)
        {
            Debug.Log("Cant return zero");
            return 1;
        }
        return numberOfCombinations;
    }

    void Grow()
    {
        transform.localScale *= stats.growthMultiplier;
        mass = transform.localScale.x;
        speed *= stats.speedMultiplier;
    }
}
