using System;
using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleStats stats;
    public float mass;
    public float speed;
    public bool invincible;
    float targetSpeed;
    Coroutine speedLerpCoroutine;
    Rigidbody2D rb;

    public static event Action<float> OnMassChanged; // Event for mass change
    public static event Action OnFusion; // Event for particle fusion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        mass = stats.defaultMass;
        transform.localScale = new(mass, mass);

        OnMassChanged?.Invoke(mass);

        speed = stats.defaultSpeed;
        Vector2 direction = GetRandomDirectionTowardsCamera(transform.position);
        rb.velocity = direction * stats.defaultSpeed;

        EnvironmentManager.OnEnvironmentChanged += OnEnvironmentChanged;
    }

    void OnDestroy()
    {
        EnvironmentManager.OnEnvironmentChanged -= OnEnvironmentChanged;
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

        SoundManager.Instance.PlayRandomSound(stats.fusionSounds, transform, UnityEngine.Random.Range(Math.Abs((1 - mass) / 1), 1));

        Grow();

        // Notify listeners about the mass change
        OnMassChanged?.Invoke(mass);

        // Notify listeners about the mass change
        OnFusion?.Invoke();
    }

    public void Reduce(float particleSize)
    {
        StartCoroutine(nameof(HandleInvicibilty));

        for (int i = 0; i < particleSize; i++)
        {
            transform.localScale /= stats.growthMultiplier;
            mass = transform.localScale.x;
            speed /= stats.speedMultiplier;
        }
    }

    void Grow()
    {
        transform.localScale *= stats.growthMultiplier;
        mass = transform.localScale.x;
        speed *= stats.speedMultiplier;
    }

    IEnumerator HandleInvicibilty()
    {
        invincible = true;
        yield return new WaitForSeconds(stats.invincibilityDuration);
        invincible = false;
    }

    void OnEnvironmentChanged(Environment environment)
    {
        switch (environment.environment)
        {
            case EnvironmentType.Warm:
                targetSpeed = speed * environment.speedMultiplier; // Example: Increase speed in Warm environment
                break;
            case EnvironmentType.Void:
                targetSpeed = speed * environment.speedMultiplier; // Example: Decrease speed in Void environment
                break;
            default:
                targetSpeed = Mathf.Clamp(speed, 1f, 10f); // Reset to a default range if needed
                break;
        }

        // Start lerping to the new speed
        if (speedLerpCoroutine != null)
        {
            StopCoroutine(speedLerpCoroutine);
        }
        speedLerpCoroutine = StartCoroutine(LerpSpeed(targetSpeed));
    }

    IEnumerator LerpSpeed(float newSpeed)
    {
        float elapsedTime = 0f;
        float initialSpeed = speed;

        while (elapsedTime < stats.lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            speed = Mathf.Lerp(initialSpeed, newSpeed, elapsedTime / stats.lerpDuration);
            yield return null;
        }

        speed = newSpeed; // Ensure speed is exactly the target speed at the end
    }
}
